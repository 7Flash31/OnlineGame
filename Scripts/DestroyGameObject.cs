using System.Collections;
using UnityEngine;

public class DestroyGameObject : MonoBehaviour
{
    [SerializeField] private float time;

    private void Start()
    {
        StartCoroutine(Destroy());
    }

    private IEnumerator Destroy()
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
}