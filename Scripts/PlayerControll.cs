using UnityEngine;
using UnityEngine.UI;
using Mirror;
using TMPro;

public class PlayerControll : NetworkBehaviour
{
    [Header("Weapon")]
    [SerializeField] private GameObject saiga;
    [SerializeField] private GameObject pistol;
    [SerializeField] private GameObject bulletSpawnS;
    [SerializeField] private GameObject bulletSpawnP;
    [SerializeField] private GameObject playerCamera;

    [Header("Animator")]
    [SerializeField] private Animator animMesh;
    [SerializeField] private Animator animArmS;
    [SerializeField] private Animator animArmP;
    [SerializeField] private TMP_Text healthText;

    [Header("Other")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Image bloodImage;
    [SerializeField] private PlayerNetwork playerNetwork;
    [SerializeField] private LayerMask groundMask;

    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;
    [SerializeField] private float jumpForce;

    public int playerHealth;
    public int kills;

    private WeaponController weaponControllerS;
    private WeaponController weaponControllerP;
    private CharacterController characterController;
    private CapsuleCollider playerCollider;

    private float groundDictsnse = 0.4f;
    private float gravitry = -19.62f;
    private float animationInterpolation = 1f;
    private Vector3 velocity;

    private float currentSpeed;

    private float h;
    private float v;

    private bool isGrounded;
    private bool isOpen;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        characterController = GetComponent<CharacterController>();
        playerCollider = GetComponent<CapsuleCollider>();
        weaponControllerS = saiga.GetComponent<WeaponController>();
        weaponControllerP = pistol.GetComponent<WeaponController>();
    }

    private void Update()
    {
        healthText.text = playerHealth.ToString();

        if(transform.position.y <= -15)
            playerHealth = 0;

        if(playerHealth <= 0)
            playerNetwork.CmdRespawn();


        ChangeWeapon();
        HandleMovement();
        UseKey();
    }

    private void ChangeWeapon()
    {
        float mw = Input.GetAxisRaw("Mouse ScrollWheel");

        if(mw > 0 && !(weaponControllerS.isReload || weaponControllerP.isReload))
        {
            Transform[] children = bulletSpawnS.GetComponentsInChildren<Transform>();
            foreach(Transform child in children)
            {
                if(child != bulletSpawnS.transform)
                {
                    Destroy(child.gameObject);
                }
            }

            if(saiga.activeSelf)
            {
                saiga.SetActive(false);
                pistol.SetActive(true);
                return;
            }
            if(pistol.activeSelf)
            {
                saiga.SetActive(true);
                pistol.SetActive(false);
                return;
            }
        }

        if(mw < 0 && !(weaponControllerS.isReload || weaponControllerP.isReload))
        {
            Transform[] children = bulletSpawnP.GetComponentsInChildren<Transform>();
            foreach(Transform child in children)
            {
                if(child != bulletSpawnP.transform)
                {
                    Destroy(child.gameObject);
                }
            }

            if(saiga.activeSelf)
            {
                saiga.SetActive(false);
                pistol.SetActive(true);
                return;
            }
            if(pistol.activeSelf)
            {
                saiga.SetActive(true);
                pistol.SetActive(false);
                return;
            }
        }
    }

    private void HandleMovement()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDictsnse, groundMask);

        if(isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        h = Input.GetAxisRaw("Horizontal");
        v = Input.GetAxisRaw("Vertical");
        Vector3 move = transform.right * h + transform.forward * v;
        characterController.Move(move * currentSpeed * Time.deltaTime);
        velocity.y += gravitry * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
    }

    private void UseKey()
    {
        if(Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpForce * -2f * gravitry);
            isGrounded = false;
        }

        if(Input.GetKeyDown(KeyCode.Escape) && !isOpen)
        {
            isOpen = true;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            return;
        }

        if(Input.GetKeyDown(KeyCode.Escape) && isOpen)
        {
            isOpen = false;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        if(Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.LeftShift))
        {
            if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
                Walk();

            else
                Run();
        }

        else
            Walk();

        if(Input.GetKeyDown(KeyCode.Space))
            animMesh.Play("rifle jump", 0);

        if(Input.GetKey(KeyCode.LeftControl))
        {
            animMesh.SetBool("IsCrouching", true);
            characterController.height = 1.3f;
            characterController.center = new Vector3(0, -0.3f, 0);
            playerCollider.height = 1.3f;
            playerCollider.center = new Vector3(0, -0.3f, 0);
            playerCamera.transform.localPosition = new Vector3(0, 0.134f, 0);
            Debug.Log(1);
        }
        else
        {
            animMesh.SetBool("IsCrouching", false);
            characterController.height = 1.5f;
            characterController.center = new Vector3(0, -0.2f, 0);
            playerCollider.height = 1.5f;
            playerCollider.center = new Vector3(0, -0.2f, 0);
            playerCamera.transform.localPosition = new Vector3(0, 0.4f, 0);
            Debug.Log(2);
        }
    }

    private void Run()
    {
        animationInterpolation = Mathf.Lerp(animationInterpolation, 1.5f, Time.deltaTime * 3);
        animMesh.SetFloat("x", h * animationInterpolation);
        animMesh.SetFloat("y", v * animationInterpolation);

        currentSpeed = Mathf.Lerp(currentSpeed, runSpeed, Time.deltaTime * 3);
    }

    private void Walk()
    {
        animationInterpolation = Mathf.Lerp(animationInterpolation, 1f, Time.deltaTime * 3);
        animMesh.SetFloat("x", h * animationInterpolation);
        animMesh.SetFloat("y", v * animationInterpolation);

        if(h > 0f || v > 0f || h < 0f || v < 0f)
        {
            animArmS.SetBool("Walk", true);
            animArmP.SetBool("Walk", true);
        }
        else
        {
            animArmS.SetBool("Walk", false);
            animArmP.SetBool("Walk", false);
        }

        currentSpeed = Mathf.Lerp(currentSpeed, walkSpeed, Time.deltaTime * 3);
    }

    //public IEnumerator SetBlood()
    //{
    //    bloodImage.color = new Color(0.9622642f, 0.240566f, 0.240566f, 1);

    //    yield return new WaitForSeconds(2);

    //    for(float alpha = 1f; alpha >= 0; alpha -= 0.01f)
    //    {
    //        bloodImage.color = new Color(0.9622642f, 0.240566f, 0.240566f, alpha);
    //        yield return null;
    //    }
    //}
}