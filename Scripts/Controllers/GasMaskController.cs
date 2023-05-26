using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GasMaskController : MonoBehaviour
{
    [SerializeField] private Slider sliderFilter;
    [SerializeField] private Slider sliderGasMask;
    [SerializeField] private Slider sliderFilterTime;
    [SerializeField] private Image imageBackFilterTime;
    [SerializeField] private Image imageFilFilterTime;
    [SerializeField] private GameObject meshGasMask;


    public float timeFilter; // 120
    public bool playerHaveGasMask;

    public float currentTimeFilter;
    public bool filterReady;
    public bool gasMaskReady;

    private Coroutine putGasMaskCoroutine;
    private Coroutine changeFilterCoroutine;
    private InventoryController inventoryController;

    private void Start()
    {
        meshGasMask.SetActive(false);
        sliderFilterTime.gameObject.SetActive(false);

        sliderFilter.value = 0;
        sliderGasMask.value = 0;
        currentTimeFilter = timeFilter;
        sliderFilterTime.value = currentTimeFilter;
        sliderFilterTime.maxValue = currentTimeFilter;
        inventoryController = GetComponent<InventoryController>();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.V)
            && playerHaveGasMask 
            && putGasMaskCoroutine == null 
            && !gasMaskReady)
            putGasMaskCoroutine = StartCoroutine(PutGasMask());

        if(Input.GetKeyDown(KeyCode.B)
            && inventoryController.gasMaskFiltersCount > 0
            && gasMaskReady
            && changeFilterCoroutine == null 
            && !filterReady)
            changeFilterCoroutine = StartCoroutine(ChangeFilter());
    }

    public void FilterWork()
    {
        if(filterReady)
        {
            if(currentTimeFilter > 0)
            {
                currentTimeFilter -= Time.deltaTime;
                sliderFilterTime.value = currentTimeFilter;
                if(sliderFilterTime.value < timeFilter / 2)
                {
                    imageBackFilterTime.color = Color.yellow;
                    imageFilFilterTime.color = Color.yellow;
                }

                if(sliderFilterTime.value < timeFilter / 4)
                {
                    imageBackFilterTime.color = Color.red;
                    imageFilFilterTime.color = Color.red;
                }
            }

            else
            {
                inventoryController.gasMaskFiltersCount--;
                filterReady = false;
            }
        }
    }

    public IEnumerator PutGasMask()
    {
        sliderGasMask.gameObject.SetActive(true);
        if(meshGasMask != null)
            meshGasMask.SetActive(true);

        for(float i = 1f; i <= 100; i += 1)
        {
            sliderGasMask.value += i;
            yield return new WaitForSeconds(0.1f);
            if(sliderGasMask.value == 100)
            {
                sliderGasMask.gameObject.SetActive(false);
                break;
            }
        }

        gasMaskReady = true;
        putGasMaskCoroutine = null;
        sliderGasMask.value = 0;
        sliderFilterTime.gameObject.SetActive(true);
        sliderFilterTime.value = 0;

        if(sliderFilterTime.value < timeFilter / 2)
        {
            imageBackFilterTime.color = Color.yellow;
            imageFilFilterTime.color = Color.yellow;
        }

        if(sliderFilterTime.value < timeFilter / 4)
        {
            imageBackFilterTime.color = Color.red;
            imageFilFilterTime.color = Color.red;
        }

        yield return null;
    }

    public IEnumerator ChangeFilter()
    {
        sliderFilter.gameObject.SetActive(true);

        for(float i = 1f; i <= 100; i += 1)
        {
            sliderFilter.value += i;
            yield return new WaitForSeconds(0.1f);
            if(sliderFilter.value == 100)
            {
                sliderFilter.gameObject.SetActive(false);
                break;
            }
        }
        currentTimeFilter = timeFilter;
        sliderFilterTime.value = currentTimeFilter;
        filterReady = true;
        changeFilterCoroutine = null;
        sliderFilter.value = 0;

        imageBackFilterTime.color = Color.white;
        imageFilFilterTime.color = Color.white;

        yield return null;
    }
}
