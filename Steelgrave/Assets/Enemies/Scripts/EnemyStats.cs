using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "NewEnemyStats", menuName = "Enemies/Enemy Stats")]
public class EnemyStats : ScriptableObject
{
    public float maxHealth = 15f;
    public float damage = 10f;
    public float speed = 5f;

    // added later
    public float size = 1f;
    public int points = 1;
}
