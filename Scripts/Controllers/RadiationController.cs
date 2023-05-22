using System.Collections;
using UnityEngine;

public class RadiationController : MonoBehaviour
{
    private Coroutine damagePlayerCoroutine;
    private Coroutine damageFilterCoroutine;

    private void OnTriggerStay(Collider other)
    {
        if(other.TryGetComponent<InventoryController>(out var inventoryController)
            && other.TryGetComponent<GasMaskController>(out var gasMaskController)
            && inventoryController.gasMaskFiltersCount > 0
            && inventoryController.gasMaskCount > 0)
        {
            gasMaskController.FilterWork();

            Debug.Log(2);

        }

        else
            if(damagePlayerCoroutine == null)
                damagePlayerCoroutine = StartCoroutine(DamagePlayer());
    }

    public IEnumerator DamagePlayer()
    {
        yield return new WaitForSeconds(2);
        damagePlayerCoroutine = null;
    }

    public IEnumerator DamageFilter(InventoryController inventoryController)
    {
        //inventoryController.gasMaskFiltersCount

        yield return new WaitForSeconds(2);
        damageFilterCoroutine = null;
    }
}
