using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ProjectileType", menuName = "Game/ProjectileType")]
public class ProjectileType : ScriptableObject
{
    [SerializeField] public float size = 1f;
    [SerializeField] public float speed = 50f;
    [SerializeField] public float damage = 5f;
    [SerializeField] public float heat = 5f;

}
