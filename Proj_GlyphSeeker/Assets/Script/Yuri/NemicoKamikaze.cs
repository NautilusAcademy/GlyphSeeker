using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NemicoKamikaze : EnemyBase
{
    [SerializeField] private float radiusForExplosion;
    [SerializeField] private Color startColor, endColor;
    [SerializeField] private Renderer renderer;
    protected override void Start()
    {
        base.Start();
        renderer = GetComponent<Renderer>();
    }

    protected override void Update()
    {
        base.Update();
        if (Vector3.Distance(transform.position, target.position) <= attackRange)
        {
            StartCoroutine(Explode());
        }
    }

    IEnumerator Explode()
    {
        float tick = 0f;

        while (renderer.material.color != endColor)
        {
            tick += Time.deltaTime / 2;
            renderer.material.color = Color.Lerp(startColor, endColor, tick);
            yield return null;
        }

        Collider[] colliders = Physics.OverlapSphere(transform.position, radiusForExplosion);

        foreach (Collider nearbyObject in colliders)
        {
            if (nearbyObject.CompareTag("Player"))
            {
                PlayerStats player = nearbyObject.GetComponent<PlayerStats>();
                player.TakeDamage(1);
            }
            if (nearbyObject.CompareTag("Destroy"))
            {
                Destroy(nearbyObject.gameObject);
            }
        }

        Destroy(gameObject);
    }
}
