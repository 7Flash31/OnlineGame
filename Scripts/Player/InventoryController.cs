using UnityEngine;

public class InventoryController : MonoBehaviour
{
    [SerializeField] private GameObject playerCamera;
    [SerializeField] private float range;
    [SerializeField] private LayerMask ignoreBullet;

    public int grenadeCount;
    public int medicalKitCount;
    public int gasMaskFiltersCount;

    private PlayerNetwork playerNetwork;
    private PlayerControll playerControll;
    private GasMaskController gasMaskController;

    private void Start()
    {
        playerNetwork = GetComponent<PlayerNetwork>();
        playerControll = GetComponent<PlayerControll>();
        gasMaskController = GetComponent<GasMaskController>();
    }

    private void Update()
    {
        Debug.DrawRay(playerCamera.transform.position, playerCamera.transform.forward * range, Color.red);

        if(Input.GetKeyDown(KeyCode.E))
            TakeItem();

        if(Input.GetKeyDown(KeyCode.C) && medicalKitCount > 0 && playerControll.playerHealth < 100)
            UseMedicalKit();
    }


    private void TakeItem()
    {
        RaycastHit hit;

        if(Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, range, ignoreBullet))
            if(hit.collider.TryGetComponent<ItemController>(out var itemController))
                itemController.Item(this, gasMaskController);
    }

    private void UseMedicalKit()
    {
        medicalKitCount--;
        playerNetwork.CmdRestoreHealth(10);
    }
}
