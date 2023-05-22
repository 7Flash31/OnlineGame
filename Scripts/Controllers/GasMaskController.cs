using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GasMaskController : MonoBehaviour
{
    [SerializeField] private InventoryController inventoryController;

    public float timeFilter; // 120
    private float currentTimeFilter;
    public bool playerHaveGasMask;

    private void Start()
    {
        timeFilter = currentTimeFilter;
    }


    public void FilterWork()
    {
        if(inventoryController.gasMaskFiltersCount > 0)
        {
            if(timeFilter > 0)
            {
                timeFilter -= Time.deltaTime;
            }

            else
            {
                inventoryController.gasMaskFiltersCount--;
                timeFilter = currentTimeFilter;
            }
        }
    }
}
