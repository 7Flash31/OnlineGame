using UnityEngine;
using Mirror;

public class ItemController : NetworkBehaviour
{
    [SerializeField] private bool isGrenade;
    [SerializeField] private bool isMedicalKit;
    [SerializeField] private bool isGasMask;
    [SerializeField] private bool isGasMaskFilter;

    public void Item(InventoryController inventoryController, GasMaskController gasMaskController)
    {
        if(isGrenade)
        {
            inventoryController.grenadeCount++;
            CmdDestroyItem();
        }

        else if(isMedicalKit)
        {
            inventoryController.medicalKitCount++;
            CmdDestroyItem();
        }

        else if(isGasMask)
        {
            gasMaskController.playerHaveGasMask = true;
            CmdDestroyItem();
        }

        else if(isGasMaskFilter)
        {
            inventoryController.gasMaskFiltersCount++;
            CmdDestroyItem();
        }
    }

    [Command(requiresAuthority = false)]
    private void CmdDestroyItem()
    {
        RpcDestroyItemh();
    }

    [ClientRpc]
    private void RpcDestroyItemh()
    {
        Destroy(gameObject);
    }
}
