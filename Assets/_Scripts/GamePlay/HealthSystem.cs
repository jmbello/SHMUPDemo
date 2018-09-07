using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour {

    private string tagName;
    private float currentHealth;

    public bool isEnemy = true;
    public float maxHealth = 10f;
    public GameObject hitEffect;
    public GameObject healthBar;


	// Use this for initialization
	void OnEnable () {
        if (!isEnemy)
        {
            tagName = "Bullet";
        }
        else
        {
            tagName = "EnemyBullet";
        }
        currentHealth = maxHealth;
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(tagName))
        {
            Vector3 triggerPosition = 
                other.ClosestPointOnBounds(transform.position);
            Vector3 direction = 
                triggerPosition - transform.position;
            GameObject fx = PoolingManager.Instance.UseObject
                (hitEffect, triggerPosition, Quaternion.LookRotation(direction));
            PoolingManager.Instance.ReturnObject(fx, 1f);
            print("enter OnTriggerEnter");
            // do damage here
            float damage = float.Parse(other.name);
            TakeDamage(damage);
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
    }
}
