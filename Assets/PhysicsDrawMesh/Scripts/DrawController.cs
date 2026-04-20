using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace DrawMesh
{
    public class DrawController : MonoBehaviour
    {
        public static DrawController Instance { get; private set; }

        [SerializeField]
        private MouseTarget mouseTarget;
        [SerializeField]
        private Transform drawMeshTemplate;
        [SerializeField()]
        private DrawSettings drawSettings;

        public event Action OnStartDraw;
        public event Action OnDrawing;
        public event Action<DrawMesh> OnEndDraw;
        public event Action<bool> OnFreezing;
        public event Action<float, float> OnLengthChanged;

        private float currentLength;
        private DrawMesh drawingMesh;
        private Camera mainCamera;
        private bool isDrawing;
        private bool isHitCantDraw;

        private List<Vector2> drawLineVertices = new List<Vector2>();
        private List<Vector2> polygonPoints = new List<Vector2>();
        private Dictionary<Vector2, float> drawLineVerticesAndWeights = new Dictionary<Vector2, float>();
        private Plane zPlaneZero = new Plane(Vector3.forward, Vector3.zero);

        public DrawSettings DrawSettings { get { return drawSettings; } }

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }

        private void Start()
        {
            mainCamera = Camera.main;
            currentLength = 0;

            mouseTarget.Init(drawSettings);
            drawMeshTemplate.gameObject.SetActive(false);
        }

        private void Update()
        {
            if (Input.GetMouseButton(0))
            {
                Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                if (zPlaneZero.Raycast(ray, out float enter))
                {
                    Vector3 mousePosOffsetted = ray.GetPoint(enter);

                    if (isDrawing == false)
                    {
                        if (CheckCanDraw(mousePosOffsetted))
                        {
                            InitDraw();
                        }
                    }
                    else
                    {
                        Drawing(mousePosOffsetted);
                    }
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                if (isDrawing)
                {
                    EndDraw();
                }
            }
        }

        private bool CheckCanDraw(Vector3 mousePosOffseted)
        {
            // Update mouseTarget and its target position
            mouseTarget.transform.position = mousePosOffseted;
            mouseTarget.SetTarget(mousePosOffseted);

            // Drawing from obstacles is not allowed
            if (mouseTarget.HitCantDrawObject())
            {
                return false;
            }

            return true;
        }

        private void InitDraw()
        {
            if (drawSettings.overlapType == OverlapType.FollowEdge)
            {
                mouseTarget.SetTrigger(false);
            }

            drawLineVertices.Clear();
            drawLineVertices.Add(mouseTarget.transform.position);

            drawingMesh = Instantiate(drawMeshTemplate, Vector3.zero, Quaternion.identity).GetComponent<DrawMesh>();
            drawingMesh.Init(drawSettings);
            drawingMesh.gameObject.SetActive(true);

            OnStartDraw?.Invoke();
            if (drawSettings.freezeObjects)
            {
                OnFreezing?.Invoke(true);
            }

            isDrawing = true;
        }

        private void Drawing(Vector3 mousePosOffseted)
        {
            // Update mouseTarget position.
            mouseTarget.SetTarget(mousePosOffseted);
            float lengthIncrease = Vector2.Distance(mouseTarget.transform.position, drawLineVertices.Last());
            if (lengthIncrease >= drawSettings.stepLength)
            {
                switch (drawSettings.overlapType)
                {
                    case OverlapType.None:
                    case OverlapType.FollowEdge:
                        AddNewToDrawLine(mouseTarget.transform.position, lengthIncrease);
                        break;
                    case OverlapType.CutOff:
                        // No more new vertices are added to it as soon as a hit is detected.
                        if (isHitCantDraw == false)
                        {
                            isHitCantDraw = mouseTarget.HitCantDrawObject();
                        }
                        if (isHitCantDraw == false)
                        {
                            AddNewToDrawLine(mouseTarget.transform.position, lengthIncrease);
                        }
                        break;
                }
            }
        }

        private void EndDraw()
        {
            // If the number of vertices is not enough, delete it.
            if (drawLineVertices.Count < 2 || (drawSettings.startWidth == 0 && drawSettings.endWidth == 0))
            {
                drawingMesh.Destroy();
            }
            else
            {
                Vector2 centerOfMass = CalculateCenterOfMass();
                drawingMesh.EndDraw(centerOfMass);
            }

            OnEndDraw?.Invoke(drawingMesh);
            if (drawSettings.freezeObjects)
            {
                OnFreezing?.Invoke(false);
            }

            mouseTarget.SetTrigger(true);
            isHitCantDraw = false;

            drawingMesh = null;
            isDrawing = false;
        }

        /// <summary>
        /// Generate Polygon Points according to the Draw Line list and pass it into DrawMesh and call the Generate Mesh function.
        /// </summary>
        private void AddNewToDrawLine(Vector2 targetPos, float distanceIncrase)
        {
            // Determine whether the maximum length is exceeded
            if (drawSettings.lengthLimit && currentLength + distanceIncrase > drawSettings.maxLength)
            {
                return;
            }

            currentLength += distanceIncrase;
            OnLengthChanged?.Invoke(currentLength, drawSettings.maxLength);

            drawLineVertices.Add(targetPos);

            polygonPoints.Clear();
            drawLineVerticesAndWeights.Clear();

            float cos = 0, sin = 0;
            for (int i = 0; i < drawLineVertices.Count - 1; i++)
            {
                // Calculate the angle of each side on the line, and then rotate 90° is what we need.
                float colliderAngle = GetAngleFromVector(drawLineVertices[i + 1] - drawLineVertices[i]);
                colliderAngle -= Mathf.Deg2Rad * 90f;
                cos = Mathf.Cos(colliderAngle);
                sin = Mathf.Sin(colliderAngle);

                // Interpolate the corresponding width according to the width of the head and tail, and only take half to achieve the optimization purpose.
                float halfTempWidth = Mathf.Lerp(drawSettings.startWidth, drawSettings.endWidth, (float)i / drawLineVertices.Count) * 0.5f;
                drawLineVerticesAndWeights.Add(drawLineVertices[i], halfTempWidth);

                // The points of PolygonCollider2D is a ring. In order to form such a vertex structure, it is necessary to insert the upper and lower points at the same time.
                polygonPoints.Add(new Vector2(drawLineVertices[i].x + halfTempWidth * cos, drawLineVertices[i].y + halfTempWidth * sin));
                polygonPoints.Insert(0, new Vector2(drawLineVertices[i].x - halfTempWidth * cos, drawLineVertices[i].y - halfTempWidth * sin));
            }

            // The angle of the last point is the same as the previous one, and the width is the last width.
            Vector2 lastVertex = drawLineVertices.Last();
            float halfEndWidth = drawSettings.endWidth * 0.5f;

            drawLineVerticesAndWeights.Add(lastVertex, halfEndWidth);

            polygonPoints.Add(new Vector2(lastVertex.x + halfEndWidth * cos, lastVertex.y + halfEndWidth * sin));
            polygonPoints.Insert(0, new Vector2(lastVertex.x - halfEndWidth * cos, lastVertex.y - halfEndWidth * sin));
            drawingMesh.SetPoints(polygonPoints, distanceIncrase);
            OnDrawing?.Invoke();
        }

        /// <summary>
        /// Get the angle based on the direction (in radians).
        /// </summary>
        private float GetAngleFromVector(Vector2 direction)
        {
            float radians = Mathf.Atan2(direction.y, direction.x);
            return radians;
        }

        /// <summary>
        /// Each point is multiplied by its corresponding width (half is used here, a meaning), all accumulated and divided by the total width to get the position of the center of gravity.
        /// </summary>
        private Vector2 CalculateCenterOfMass()
        {
            Vector2 pointsSum = Vector2.zero;
            float weightSum = 0;
            foreach (var item in drawLineVerticesAndWeights)
            {
                pointsSum += item.Key * item.Value;
                weightSum += item.Value;
            }
            return pointsSum / weightSum; ;
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (isDrawing && drawLineVertices != null && drawLineVertices.Count > 0)
            {
                Gizmos.color = Color.black;
                Gizmos.DrawSphere(drawLineVertices[0], 0.1f);

                for (int i = 0; i < drawLineVertices.Count - 1; i++)
                {
                    Gizmos.color = Color.black;
                    Gizmos.DrawSphere(drawLineVertices[i + 1], 0.1f);
                    Gizmos.color = Color.cyan;
                    Gizmos.DrawLine(drawLineVertices[i], drawLineVertices[i + 1]);
                }
            }
        }
#endif
    }
}