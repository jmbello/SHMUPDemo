﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoShoot : MonoBehaviour
{
    public ShootProfile shootProfile;
    public GameObject bulletPrefabs;
    public Transform firePoint;

    private float totalSpread;
    private WaitForSeconds rate, interval;

    // Use this for initialization
    void OnEnable()
    {
        interval = new WaitForSeconds(shootProfile.interval);
        rate = new WaitForSeconds(shootProfile.fireRate);

        if (firePoint == null)
            firePoint = transform;

        totalSpread = shootProfile.spread * shootProfile.amount;

        StartCoroutine(ShootingSequence());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    IEnumerator ShootingSequence()
    {
        yield return rate;

        while (true)
        {
            float angle = 0f;

            for (int i = 0; i < shootProfile.amount; i++)
            {
                angle = totalSpread * (i / (float)shootProfile.amount);
                angle -= (totalSpread / 2f) - (shootProfile.spread / shootProfile.amount);

                Shoot(angle);

                if (shootProfile.fireRate > 0f)
                    yield return rate;
            }

            yield return interval;
        }
    }

    void Shoot(float angle)
    {
        GameObject temp = PoolingManager.Instance.UseObject(bulletPrefabs, firePoint.position, Quaternion.identity);
        temp.name = shootProfile.damage.ToString();
        temp.transform.Rotate(Vector3.up, 0f);
        temp.SetActive(true);
        temp.GetComponent<BulletMove>().speed = shootProfile.speed;
        PoolingManager.Instance.ReturnObject(temp, shootProfile.destroyRate);
    }
}