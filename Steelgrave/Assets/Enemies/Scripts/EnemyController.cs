using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public EnemyStats stats;
    public Transform target;

    public float currentHealth;
    public float stopDistance = 0.5f; // Adjust as needed

    private void Start()
    {
        currentHealth = stats.maxHealth;
    }

    private void Update()
    {
        if (target != null)
        {
            Vector3 direction = (target.position - transform.position).normalized;
            float distance = Vector3.Distance(transform.position, target.position);

            // Move if not at the center
            if (distance > stopDistance)
            {
                transform.position += direction * stats.speed * Time.deltaTime;

                // Rotate to face target
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, 10f * Time.deltaTime);
            }
            else
            {
                // Reached the center — destroy self
                Destroy(gameObject);
            }
        }
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
      
    }

    public void Die()
    {
        Destroy(gameObject);
    }
}
