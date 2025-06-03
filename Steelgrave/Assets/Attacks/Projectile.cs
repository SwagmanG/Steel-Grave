using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private ProjectileType p;

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
            //other.TakeDamage(projectileDamage);
        }
    }



}
