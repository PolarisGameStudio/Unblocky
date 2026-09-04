using TMPro;
using UnityEngine;

namespace Flavor
{
    public class GameplayUI : BaseUI
    {
        [Header("Gameplay UI")]
        [SerializeField] private CountdownUI _countdownUI;

        public CountdownUI CountdownUI => _countdownUI;
        public override void Initialize()
        {
            base.Initialize();
        }
    }
}