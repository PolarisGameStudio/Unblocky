using Cysharp.Threading.Tasks;
using System.Collections;
using UnityEngine;

namespace Flavor
{
    public abstract class BaseSystem : BaseMono, ISystem
    {
        [Header("System Settings")]
        [SerializeField] private int _priority = 0;
        public bool IsInitialized { get; protected set; }
        public int Priority => _priority;

        #region LifeCycle

        /// <summary>
        /// Kh?i t?o h? th?ng (Ch?y ? Start ho?c ???c GameManager g?i)
        /// </summary>
        public override void Initialize()
        {
            if (IsInitialized) return;
        }

        /// <summary>
        /// D?n d?p h? th?ng khi ??i Scene ho?c t?t game
        /// </summary>
        public override void Dispose()
        {
            base.Dispose();
            if (!IsInitialized) return;
            // G? b? kh?i ServiceLocator
            IsInitialized = false;
        }

        public virtual async UniTask LoadDataAsync()
        {
            
        }

        #endregion

    }
}