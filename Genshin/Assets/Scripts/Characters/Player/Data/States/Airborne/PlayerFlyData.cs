using System;
using UnityEngine;

namespace RPGStateMachineSystem
{
    [Serializable]
    public class PlayerFlyData
    {
        [field: Tooltip("Having higher numbers might not read collisions with shallow colliders correctly.")]
        [field: SerializeField] [field: Range(0f, 10f)] public float FallSpeedLimit { get; private set; } = 3f;
       
    }
}