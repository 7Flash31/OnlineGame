using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform playerBody;
    [SerializeField] private float speedRotate;
    private float xRot;

    private void LateUpdate()
    {
        Vector2 mou = new Vector2(Input.GetAxisRaw("Mouse X") * speedRotate, Input.GetAxisRaw("Mouse Y") * speedRotate);

        xRot -= mou.y;
        xRot = Mathf.Clamp(xRot, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRot, 0f, 0f);
        playerBody.Rotate(Vector3.up * mou.x);
    }
}