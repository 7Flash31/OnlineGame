using UnityEngine;
using Mirror;

public class GrenadeExplosion : MonoBehaviour
{
    [SerializeField] private GameObject explosionEffect;
    [SerializeField] private float delay;
    [SerializeField] private float explosionForce;
    [SerializeField] private float explosionRadius;

    private float countdown;
    private bool hasExplosion;

    private void Start()
    {
        countdown = delay;
        hasExplosion = false;
    }

    private void Update()
    {
        countdown -= Time.deltaTime;

        if(countdown <= 0 && !hasExplosion)
        {
            Explode();
            hasExplosion = true;
        }
    }

    private void Explode()
    {
        GameObject grenade = Instantiate(explosionEffect, transform.position, transform.rotation);

        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach(Collider nearbyObject in colliders)
        {
            Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
            if(rb != null)
            {
                rb.AddExplosionForce(explosionForce, transform.position, explosionRadius);
            }
        }

        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
