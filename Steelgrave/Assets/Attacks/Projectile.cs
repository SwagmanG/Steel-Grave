using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.UI.Image;

public class Projectile : MonoBehaviour
{
    private ProjectileType p;

    // for rocket
    private RaycastHit[] hits;
    public float splashRadius = 5f;

    public GameObject ExplosionPrefab;
    public float explosionLength;

    public void Init(ProjectileType attack)
    {
        p = attack;
        transform.localScale = Vector3.one * p.size;
        GameManager.Instance.HeatGenerate(p.heat);
    }

    void Update()
    {
        transform.Translate(Vector3.forward * p.speed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Debug.LogWarning("Projectile hit the enemy!");

            EnemyController enemy = other.GetComponent<EnemyController>();

            DealDamage(enemy);

            if(p.name == "Rocket")
            {
                Debug.Log("HIT WITH ROCKET");
                SplashDamage();
            }


            Destroy(gameObject);

        }
        


    }

    void DealDamage(EnemyController enemy)
    {
        if (enemy != null)
        {
            enemy.TakeDamage(p.damage);

            if (enemy.currentHealth <= 0)
            {
                GameManager.Instance.IncreaseScore(enemy.stats.points);
                enemy.Die();
            }
        }
    }
    void SplashDamage()
    {
        GameObject explosion = Instantiate(ExplosionPrefab, transform.position, Quaternion.identity);
        explosion.transform.localScale = Vector3.one * splashRadius * 2;

        Destroy(explosion, explosionLength);

        hits = Physics.SphereCastAll(transform.position, splashRadius, transform.forward, 10f);

        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.CompareTag("Enemy"))
            {
                EnemyController enemy = hit.transform.gameObject.GetComponent<EnemyController>();

                DealDamage(enemy);
            }
        }
    }
}