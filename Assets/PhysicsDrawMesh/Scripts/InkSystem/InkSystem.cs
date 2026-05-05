using UnityEngine;

namespace DrawMesh
{
    public class InkSystem : MonoBehaviour
    {
        public static InkSystem Instance { get; private set; }

        [Header("Ink Settings")]
        [SerializeField] private float maxInk = 10f;
        [SerializeField] private bool useInk = true;

        [Range(0f, 50f)]
        [SerializeField] private float debugInk;

        public float CurrentInk { get; private set; }

        public float InkPercent
        {
            get
            {
                if (maxInk <= 0) return 0;
                return CurrentInk / maxInk;
            }
        }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            ResetInk();
        }

        private void Update()
        {
            debugInk = CurrentInk;
        }

        /// <summary>
        /// Try to consume ink when drawing
        /// </summary>
        public bool TryUseInk(float amount)
        {
            if (!useInk) return true;

            if (CurrentInk >= maxInk)
                return false;

            float allowed = Mathf.Min(amount, maxInk - CurrentInk);
            CurrentInk += allowed;

            return allowed > 0;
        }

        /// <summary>
        /// Hard check (pen stop style)
        /// </summary>
        public bool TryConsume(float amount)
        {
            if (!useInk) return true;

            if (CurrentInk + amount > maxInk)
                return false;

            CurrentInk += amount;
            return true;
        }

        public bool CanDraw()
        {
            if (!useInk) return true;
            return CurrentInk < maxInk;
        }

        public void ResetInk()
        {
            CurrentInk = 0f;
        }

        public void AddInk(float amount)
        {
            CurrentInk = Mathf.Clamp(CurrentInk - amount, 0, maxInk);
        }
    }
}