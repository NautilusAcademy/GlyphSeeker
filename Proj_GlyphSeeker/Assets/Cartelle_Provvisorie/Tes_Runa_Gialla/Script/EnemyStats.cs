using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    public AudioSource audioSource;
    public int health = 10;
    public MeshRenderer enemyRenderer;

    public void TakeDamage(int damage)
    {
        health -= damage;
        audioSource.Play();
        StartCoroutine(ChangeColor());

        if (health <= 0)
        {
            Die();
        }
    }

    IEnumerator ChangeColor()
    {
        enemyRenderer.material.color = Color.red;
        yield return new WaitForSeconds(0.25f);
        enemyRenderer.material.color = Color.white;
    }

    public void Die()
    {
        Destroy(gameObject);
    }
}