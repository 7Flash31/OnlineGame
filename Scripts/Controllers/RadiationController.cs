using System.Collections;
using UnityEngine;

public class RadiationController : MonoBehaviour
{
    private Coroutine damagePlayerCoroutine;
    //private Coroutine damageFilterCoroutine;

    private void OnTriggerStay(Collider other)
    {
        InventoryController inventoryController = other.gameObject.GetComponent<InventoryController>();
        GasMaskController gasMaskController = other.gameObject.GetComponent<GasMaskController>();
        PlayerControll playerControll = other.gameObject.GetComponent<PlayerControll>();

        if(inventoryController != null && gasMaskController != null)
        {
            if(inventoryController.gasMaskFiltersCount > 0 && gasMaskController.playerHaveGasMask)
            {
                gasMaskController.FilterWork();
            }

            else
            {
                if(damagePlayerCoroutine == null)
                {
                    damagePlayerCoroutine = StartCoroutine(DamagePlayer(playerControll));
                }
            }

            if(!gasMaskController.filterReady && damagePlayerCoroutine == null)
            {
                damagePlayerCoroutine = StartCoroutine(DamagePlayer(playerControll));
            }
        }
    }

    public IEnumerator DamagePlayer(PlayerControll playerControll)
    {
        yield return new WaitForSeconds(2f);

        if(playerControll != null)
            playerControll.playerHealth -= 5;
        damagePlayerCoroutine = null;
    }
}
