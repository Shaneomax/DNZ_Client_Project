using UnityEngine;

namespace DrawMesh
{
    /// <summary>
    /// After the specified time, it will disappear automatically; 
    /// freezing does not count as time
    /// </summary>
    public class DelayDestroy : MonoBehaviour
    {
        private float survivalTimer;
        private float survivalTime;
        private bool isFreezing;

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
            isFreezing = freezing;
        }

        /// <summary>
        /// Set survival time
        /// </summary>
        public void SetSurvivalTime(float survivalTime)
        {
            this.survivalTime = survivalTime;
            survivalTimer = 0;
        }

        private void Update()
        {
            if (isFreezing) return;

            survivalTimer += Time.deltaTime;
            if (survivalTimer >= survivalTime)
            {
                Destroy(gameObject);
            }
        }
    }
}