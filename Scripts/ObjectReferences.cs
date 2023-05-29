using UnityEngine;

public class ObjectReferences : MonoBehaviour
{
    [Header("Impact")]
    public GameObject impactMetal;
    public GameObject impactConcrete;
    public GameObject impactWood;
    public GameObject impactSand;
    public GameObject impactDirt;
    public GameObject impactStone;
    public GameObject impactBlood;

    [Header("Object")]
    public GameObject grenade;
    public GameObject flame;

    public GameObject[] respawnPoint;

    public static ObjectReferences instance = null;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else if(instance == this)
        {
            Destroy(gameObject);
        }
    }
}
