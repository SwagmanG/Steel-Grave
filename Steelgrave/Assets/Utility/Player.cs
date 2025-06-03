using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint; // where projectile originates from
    [SerializeField] private Transform cannon; // for cannon anim

    public void Shoot(ProjectileType attack)
    {
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        projectile.GetComponent<Projectile>().Init(attack);

        StartCoroutine(ShootCoroutine());

    }

    IEnumerator ShootCoroutine()
    {
        float timer = 0f;

        while (timer < 0.05)
        {
            timer += Time.deltaTime;
            CannonDown(true);
            yield return null;
        }

        CannonDown(false);
    }

    public void CannonDown(bool isDown)
    {
        if (isDown)
        {
            cannon.transform.localPosition = new Vector3(0, -0.007f, -0.004f);
        }
        else
        {
            cannon.transform.localPosition = new Vector3(0, -0.01f, -0.004f);
        }
    }

}
