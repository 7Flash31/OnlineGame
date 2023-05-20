using System.Collections;
using UnityEngine;
using TMPro;
using Mirror.Experimental;

public class WeaponController : MonoBehaviour
{
    [Header("ObjectReferences")]
    public GameObject muzzleFlashObj;
    public ObjectReferences muzzleFlash;
    public Transform muzzleFlashSpawn;

    [Header("Audio")]
    [SerializeField] private AudioClip shoot;
    [SerializeField] private AudioClip reload;
    [SerializeField] private AudioSource audioSource;

    [Header("Other")]
    [SerializeField] private Camera playerCamera;
    [SerializeField] private TMP_Text ammoText;
    [SerializeField] private Animator meshAnimator;
    [SerializeField] private Animator armAnimator;
    [SerializeField] private PlayerNetwork playerNetwork;

    [Header("Characteristics")]
    [SerializeField] private int damage;
    [SerializeField] private float range;
    [SerializeField] private float fireRate;
    [SerializeField] private float bulletForse;
    [SerializeField] private bool isPistol;

    [HideInInspector] public int maxAmmo;
    [HideInInspector] public int currentAmmo;
    [HideInInspector] public bool isReload;

    private bool isCanShoot;
    private float nextFire = 0;

    private void Start()
    {
        currentAmmo = maxAmmo;
    }

    private void Update()
    {
        Debug.DrawLine(playerCamera.transform.position, playerCamera.transform.forward * range, Color.red);
        
        ammoText.text = currentAmmo + "/" + maxAmmo;

        if(currentAmmo > 0)
            isCanShoot = true;
        else
            isCanShoot = false;

        if(Input.GetKeyDown(KeyCode.R) && !isReload && currentAmmo != maxAmmo)
        {
            audioSource.PlayOneShot(reload);
            meshAnimator.Play("reloading", 1);
            armAnimator.Play("Reload_Full");
            currentAmmo = maxAmmo;
            StartCoroutine(Reload());
        }

        if(Input.GetKey(KeyCode.Mouse0) && Time.time > nextFire && isCanShoot && !isReload && !isPistol) 
        {
            currentAmmo--;
            nextFire = Time.time + 1f / fireRate;
            Shoot();

            GameObject muzzleFlash = Instantiate(muzzleFlashObj, muzzleFlashSpawn.position, muzzleFlashSpawn.rotation, muzzleFlashSpawn);
            Destroy(muzzleFlash, 5f);
        }

        if(Input.GetKeyDown(KeyCode.Mouse0) && isCanShoot && !isReload && isPistol && Time.time > nextFire)
        {
            currentAmmo--;
            nextFire = Time.time + 1f / fireRate;
            Shoot();

            GameObject muzzleFlash = Instantiate(muzzleFlashObj, muzzleFlashSpawn.position, muzzleFlashSpawn.rotation, muzzleFlashSpawn);
            Destroy(muzzleFlash, 5f);
        }
    }

    private void Shoot()
    {
        audioSource.PlayOneShot(shoot);
        meshAnimator.Play("firing rifle", 1);
        armAnimator.Play("Fire");
        RaycastHit hit;

        if(Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, range))
        {
            string hitObject = "Concrete";
            //Transform parent = hit.collider.transform;

            if(hit.collider.GetComponent<Rigidbody>() != null)
            {
                Vector3 hitNormal = hit.normal;
                playerNetwork.CmdAddForse(hit.transform.gameObject, hitNormal, bulletForse);
            }

            if(hit.transform.gameObject.tag == "Player" && hit.collider.TryGetComponent<PlayerNetwork>(out var hitPlayerNetwork))
            {
                hitPlayerNetwork.DamagePlayer(damage);
                hitObject = "Player";
            } 

            if(hit.transform.gameObject.tag == "GasCylinder" && hit.collider.TryGetComponent<ExplosionController>(out var explosionController))
            {
                explosionController.isCanExplosion = true;
            } 

            if(hit.transform.gameObject.tag == "Concrete")
                hitObject = "Concrete";

            if(hit.transform.gameObject.tag == "Metal")
                hitObject = "Metal";

            if(hit.transform.gameObject.tag == "Wood")
                hitObject = "Wood";

            if(hit.transform.gameObject.tag == "Sand")
                hitObject = "Sand";

            if(hit.transform.gameObject.tag == "GasCylinder")
                hitObject = "GasCylinder";

            playerNetwork.CmdSetImpact(hit.point, Quaternion.LookRotation(hit.normal), hitObject);
            //playerNetwork.CmdSetMuzzle(muzzleFlashSpawn.transform.position, Quaternion.identity, muzzleFlashSpawn.transform);
        }
    }
     
    private IEnumerator Reload()
    {
        isReload = true;
        yield return new WaitForSeconds(2f);

        isReload = false;
    }
}