using Cysharp.Threading.Tasks;
using System.Collections;
using UnityEngine;

namespace Flavor
{
    public abstract class BaseSystem : MonoBehaviour, ISystem
    {
        [Header("System Settings")]
        [SerializeField] private int _priority = 0;
        public bool IsInitialized { get; protected set; }
        public int Priority => _priority;

        protected void Start() { }


        #region LifeCycle

        /// <summary>
        /// Kh?i t?o h? th?ng (Ch?y ? Start ho?c ???c GameManager g?i)
        /// </summary>
        public virtual void Initialize()
        {
            if (IsInitialized) return;
        }

        /// <summary>
        /// D?n d?p h? th?ng khi ??i Scene ho?c t?t game
        /// </summary>
        public virtual void Dispose()
        {
            if (!IsInitialized) return;
            // D?n d?p logic riêng
            Dispose();
            // G? b? kh?i ServiceLocator
            IsInitialized = false;
        }

        public virtual async UniTask LoadDataAsync()
        {
            
        }

        #endregion

    }
}