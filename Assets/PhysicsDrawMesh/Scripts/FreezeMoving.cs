using UnityEngine;

namespace DrawMesh
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class FreezeMoving : MonoBehaviour
    {
        private Rigidbody2D rigidbody2d;
        private RigidbodyType2D startbodyType;
        private Vector2 velocity;
        private float angularVelocity;

        private void Awake()
        {
            rigidbody2d = GetComponent<Rigidbody2D>();
            startbodyType = rigidbody2d.bodyType;
        }

        private void Start()
        {
            DrawController.Instance.OnFreezing += OnFreezing;
        }

        private void OnDestroy()
        {
            DrawController.Instance.OnFreezing -= OnFreezing;
        }

        private void OnFreezing(bool freezing)
        {
            if (freezing)
            {
                Freezing();
            }
            else
            {
                RestoreNormal();
            }
        }

        /// <summary>
        /// frozen object
        /// </summary>
        private void Freezing()
        {
            velocity = rigidbody2d.linearVelocity;
            angularVelocity = rigidbody2d.angularVelocity;
            rigidbody2d.bodyType = RigidbodyType2D.Static;
        }

        /// <summary>
        /// restore object
        /// </summary>
        private void RestoreNormal()
        {
            rigidbody2d.bodyType = startbodyType;
            rigidbody2d.linearVelocity = velocity;
            rigidbody2d.angularVelocity = angularVelocity;
        }
    }
}
