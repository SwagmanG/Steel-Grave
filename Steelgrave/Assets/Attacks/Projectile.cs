using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed = 50f;
    [SerializeField] private float damage = 5f;
    [SerializeField] private float heat = 5f;

    private void Awake()
    {
       GameManager.Instance.HeatGenerate(heat);
    }

    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
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
