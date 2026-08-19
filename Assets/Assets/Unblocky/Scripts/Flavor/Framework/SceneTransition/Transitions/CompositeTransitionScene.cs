using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Flavor
{
    public class CompositeTransitionScene : BaseTransitionScene
    {
        [SerializeField] private BaseTransitionScene[] _transitions;

        private void Awake()
        {
            if (_transitions == null || _transitions.Length == 0)
            {
                _transitions = GetComponentsInChildren<BaseTransitionScene>(true)
                               .Where(t => t != this) // Loại bỏ chính nó
                               .ToArray();
            }
        }

        public override IEnumerator TransitionIn()
        {
            // Bắt đầu tất cả hiệu ứng cùng lúc (Song song)
            Debug.Log($"[TransitionsIn] count {_transitions.Count()}");
            var coroutines = new List<Coroutine>();
            foreach (var transition in _transitions)
            {
                if (transition != null)
                    coroutines.Add(StartCoroutine(transition.TransitionIn()));
            }
            // Chờ tất cả chạy xong
            foreach (var coroutine in coroutines)
            {
                yield return coroutine;
            }

        }
        public override void SetProgress(float progress)
        {
            foreach (var transition in _transitions)
            {
                if (transition != null)
                    transition.SetProgress(progress);
            }
        }
        public override IEnumerator TransitionOut()
        {
            var coroutines = new List<Coroutine>();
            foreach (var transition in _transitions)
            {
                if (transition != null)
                    coroutines.Add(StartCoroutine(transition.TransitionOut()));
            }
            foreach (var coroutine in coroutines)
            {
                yield return coroutine;
            }
        }


    }
}