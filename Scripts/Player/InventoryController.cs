using UnityEngine;

public class InventoryController : MonoBehaviour
{
    [SerializeField] private GameObject playerCamera;
    [SerializeField] private PlayerNetwork playerNetwork;
    [SerializeField] private PlayerControll playerControll;
    [SerializeField] private GasMaskController gasMaskController;
    [SerializeField] private float range;

    public int grenadeCount;
    public int medicalKitCount;
    public int gasMaskFiltersCount;
    public int gasMaskCount;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
            TakeItem();

        if(Input.GetKeyDown(KeyCode.C) && medicalKitCount > 0 && playerControll.playerHealth < 100)
            UseMedicalKit();

        if(gasMaskCount > 1)
            gasMaskController.playerHaveGasMask = true;

    }


    private void TakeItem()
    {
        RaycastHit hit;

        if(Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, range))
            if(hit.collider.TryGetComponent<ItemController>(out var itemController))
                itemController.Item(this);
    }

    private void UseMedicalKit()
    {
        medicalKitCount--;
        playerNetwork.CmdRestoreHealth(10);
    }
}
