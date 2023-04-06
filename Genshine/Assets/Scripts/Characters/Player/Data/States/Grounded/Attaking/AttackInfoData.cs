using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AttackInfoData
{
    [field: SerializeField] public string AttackName { get; private set; }
    [field: SerializeField] public int ComboStateIndex { get; private set; }
    [field: SerializeField][field: Range(0f, 1f)] public float ComboTransitionTime { get; private set; }

    [field: SerializeField][field: Range(0f, 3f)] public float ForceTransitionTime { get; private set; }
    [field: SerializeField][field: Range(0f, 5f)] public float ForceTime { get; private set; }
    [field: SerializeField][field: Range(-10f, 10f)] public float ForceDistance { get; private set; }
    [field: SerializeField] public int Damage { get; private set; }
    
}
