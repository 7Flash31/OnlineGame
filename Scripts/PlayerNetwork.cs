using System.Collections;
using UnityEngine;
using Mirror;
using Unity.Burst.CompilerServices;

public class PlayerNetwork : NetworkBehaviour
{
    [SerializeField] private PlayerControll playerControll;
    [SerializeField] private Animator animMesh;

    [SerializeField] private WeaponController weaponControllerS;
    [SerializeField] private WeaponController weaponControllerP;
    [SerializeField] private GrenadeController grenadeController;
    [SerializeField] private GameObject grenade;

    [SerializeField] private Component[] clientComp;
    [SerializeField] private GameObject[] client;
    [SerializeField] private GameObject[] local;
    [SerializeField] private GameObject[] death;


    [SerializeField] private bool randomRespawn;
    [SerializeField] private bool pointRespawn;

    private void Start()
    {
        if(!isLocalPlayer)
        {
            for(int i = 0; i < client.Length; i++)
            {
                Destroy(client[i]);
            }
            for(int i = 0; i < clientComp.Length; i++)
            {
                Destroy(clientComp[i]);
            }
        }

        if(isLocalPlayer)
        {
            for(int i = 0; i < local.Length; i++)
            {
                Destroy(local[i]);
            }
        }
    }

    public void DamagePlayer(int damage)
    {
        if(isLocalPlayer)
            return;
        animMesh.Play("hit reaction");
        int health = playerControll.playerHealth;
        health -= damage; 
        if(health <= 0)
            KillCount();
        CmdHealth(damage);
    }

    //Health

    [Command(requiresAuthority = false)]
    private void CmdHealth(int damage)
    {
        RpcHealth(damage);
    }

    [ClientRpc]
    private void RpcHealth(int damage)
    {
        playerControll.playerHealth -= damage;
    }

    //Respawn

    [Command(requiresAuthority = false)]
    public void CmdRespawn()
    {
        RpcRespawn();
    }

    [ClientRpc]
    private void RpcRespawn()
    {
        StartCoroutine(Respawn());
    }

    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(0.01f);

        if(randomRespawn)
        {
            Vector3 pos = new Vector3(Random.Range(-12, 12), 2f, Random.Range(-16, 16));
            transform.position = pos;
        }

        if(pointRespawn)
        {
            Vector3 pos = ObjectReferences.instance.respawnPoint[Random.Range(0, ObjectReferences.instance.respawnPoint.Length)].transform.position;
            transform.position = pos;
        }

        for(int i = 0; i < death.Length; i++)
        {
            if(death[i] != null)
                death[i].SetActive(false);
        }

        playerControll.playerHealth = 100;
        weaponControllerS.currentAmmo = weaponControllerS.maxAmmo;
        weaponControllerP.currentAmmo = weaponControllerP.maxAmmo;



        yield return new WaitForSeconds(5f);

        for(int i = 0; i < death.Length; i++)
        {
            if(death[i] != null)
                death[i].SetActive(true);
        }
    }

    //KillCount

    private void KillCount()
    {
        playerControll.kills++;
    }

    [Command]
    public void CmdSetImpact(Vector3 position, Quaternion rotation, string hitObject)
    {
        if(hitObject == "Player")
        {
            GameObject impact = Instantiate(ObjectReferences.instance.impactBlood, position, rotation);
            NetworkServer.Spawn(impact);
        }

        if(hitObject == "Metal")
        {
            GameObject impact = Instantiate(ObjectReferences.instance.impactMetal, position, rotation);
            NetworkServer.Spawn(impact);
        }

        if(hitObject == "Concrete")
        {
            GameObject impact = Instantiate(ObjectReferences.instance.impactConcrete, position, rotation);
            NetworkServer.Spawn(impact);
        }

        if(hitObject == "Wood")
        {
            GameObject impact = Instantiate(ObjectReferences.instance.impactWood, position, rotation);
            NetworkServer.Spawn(impact);
        }

        if(hitObject == "Sand")
        {
            GameObject impact = Instantiate(ObjectReferences.instance.impactSand, position, rotation);
            NetworkServer.Spawn(impact);
        }

        if(hitObject == "GasCylinder")
        {
            GameObject impact = Instantiate(ObjectReferences.instance.flame, position, rotation);
            NetworkServer.Spawn(impact);
        }
    }

    [Command]
    public void CmdSpawnGrenade(Vector3 position, Quaternion rotation)
    {
        GameObject grenade = Instantiate(ObjectReferences.instance.grenade, position, rotation);
        grenade.GetComponent<ExplosionController>().isCanExplosion = true;
        NetworkServer.Spawn(grenade);
        grenadeController.Throw(grenade);
    }

    [Command(requiresAuthority = false)]
    public void CmdAddForse(GameObject obj, Vector3 hitNormal, float bulletForce)
    {
        RcpAddForse(obj, hitNormal, bulletForce);
    }

    [ClientRpc]
    public void RcpAddForse(GameObject obj, Vector3 hitNormal, float bulletForce)
    {
        Rigidbody rb = obj.GetComponent<Rigidbody>();
        rb.AddForce(-hitNormal * bulletForce);
    }


    //[Command(requiresAuthority = false)]
    //public void CmdAddForse(Rigidbody rb, float bulletForse)
    //{
    //    rb.AddForce(-transform.forward * bulletForse);
    //}

    //[Command]
    //public void CmdSetMuzzle(Vector3 position, Quaternion rotation, Transform spawner)
    //{

    //    GameObject impact = Instantiate(weaponController.muzzleFlashObj, position, rotation, spawner);
    //    NetworkServer.Spawn(impact);
    //}
}
