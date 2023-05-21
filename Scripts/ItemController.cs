using UnityEngine;
using Mirror;

public class ItemController : NetworkBehaviour
{
    [SerializeField] private bool isGrenade;
    [SerializeField] private bool isMedicalKit;

    public void Item(InventoryController inventoryController)
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
