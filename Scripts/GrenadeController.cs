using UnityEngine;
using Mirror;

public class GrenadeController : NetworkBehaviour
{
    [SerializeField] private Transform playerCamera;
    [SerializeField] private Transform attactPoint;
    [SerializeField] private GameObject grenade;
    [SerializeField] private PlayerNetwork playerNetwork;

    [Header("Settings")]
    [SerializeField] private int totalThrows;
    [SerializeField] private float throwCooldown;

    [Header("Settings")]
    [SerializeField] private float throwForce;
    [SerializeField] private float throwUpwardForce;

    private bool readyToTwow;

    private void Start()
    {
        readyToTwow = true;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.G) && readyToTwow && totalThrows > 0)
        {
            if(playerCamera != null && attactPoint != null)
                playerNetwork.CmdSpawnGrenade(attactPoint.position, Quaternion.identity);
        }
    }

    public void Throw(GameObject grenade)
    {
        readyToTwow = false;

        Rigidbody rb = grenade.GetComponent<Rigidbody>();
        Vector3 forceDirection = transform.forward;

        Vector3 forceToAdd = transform.forward * throwForce + transform.up * throwUpwardForce;
        rb.AddForce(forceToAdd, ForceMode.Impulse);

        totalThrows--;

        Invoke(nameof(ResetThrow), throwCooldown);
    }

    //private void Throw()
    //{
    //    readyToTwow = false;

    //    GameObject grenadeObj = Instantiate(grenade, attactPoint.position, playerCamera.rotation);
    //    Rigidbody rb = grenadeObj.GetComponent<Rigidbody>();
    //    Vector3 forceDirection = playerCamera.transform.forward;

    //    RaycastHit hit;
    //    if(Physics.Raycast(playerCamera.position, playerCamera.forward, out hit, 500))
    //    {
    //        forceDirection = (hit.point - attactPoint.position).normalized;
    //    }

    //    Vector3 forceToAdd = playerCamera.transform.forward * throwForce + transform.up * throwUpwardForce;
    //    rb.AddForce(forceToAdd, ForceMode.Impulse);

    //    totalThrows--;

    //    Invoke(nameof(ResetThrow), throwCooldown);
    //}

    private void ResetThrow()
    {
        readyToTwow = true;
    }
}
