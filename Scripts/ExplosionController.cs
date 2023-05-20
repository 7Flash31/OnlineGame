using UnityEngine;
using Mirror;

public class ExplosionController : MonoBehaviour
{
    [SerializeField] private GameObject explosionEffect;
    [SerializeField] private float delay;
    [SerializeField] private float explosionForce;
    [SerializeField] private float explosionRadius;

    [SerializeField] private bool isGasCylinder;
    [SerializeField] private bool isGrenade;

    public bool isCanExplosion;

    private bool hasExplosion;

    private void Start()
    {
        hasExplosion = false;
    }

    private void Update()
    {
        if(isCanExplosion)
            delay -= Time.deltaTime;

        if(delay <= 0 && !hasExplosion && isGrenade)
        {
            ExplodeGrenade();
            hasExplosion = true;
        }

        if(delay <= 0 && !hasExplosion && isGasCylinder)
        {
            ExplodeGrenade();
            hasExplosion = true;
        }
    }

    private void ExplodeGrenade()
    {
        Instantiate(explosionEffect, transform.position, transform.rotation);

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
