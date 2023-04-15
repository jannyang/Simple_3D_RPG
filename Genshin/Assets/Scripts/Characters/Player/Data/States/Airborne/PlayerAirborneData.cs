using System;
using UnityEngine;

namespace RPGStateMachineSystem
{
    [Serializable]
    public class PlayerAirborneData
    {
        [field: SerializeField] public PlayerJumpData JumpData { get; private set; }
        [field: SerializeField] public PlayerFallData FallData { get; private set; }
        [field: SerializeField] public PlayerFlyData FlyData { get; private set; }
    }
}