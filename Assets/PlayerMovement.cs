// Credit to brackeys: https://www.youtube.com/watch?v=_QajrabyTJc
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float speed = 12f;
    [SerializeField] private GameObject staminaUI;
    [SerializeField] private GameObject EscapeBar;
    [SerializeField] private GameObject PauseMenu;
    private bool paused = false;
    private ProgressBar progress;
    private GameObject GrabbedBy;
    public bool grabbed = false;
    bool isGrounded;
    float oldHeight = 2.0f;
    float walkSpeed;
    float crouchSpeed;
    float sprintSpeed;
    public float stamina = 100f;
    public float escapeCurrent = 0f;
    public float escapeMax = 35f;
    KeyCode[] EscapeKeys = { KeyCode.Space, KeyCode.LeftShift, KeyCode.Q, KeyCode.E, KeyCode.F, KeyCode.A, KeyCode.D };
    KeyCode EscapeKey = KeyCode.E;
    CharacterController controller;
    Vector3 velocity;
    void Start()
    {
        controller = transform.GetComponent<CharacterController>();
        progress = EscapeBar.GetComponent<ProgressBar>();
        oldHeight = controller.height;
        crouchSpeed = speed / 2;
        walkSpeed = speed;
        sprintSpeed = speed * 2;
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!paused)
            {
                paused = true;
                Time.timeScale = 0;
                PauseMenu.SetActive(true);
                Cursor.lockState = CursorLockMode.None;
            }
            else if (paused)
            {
                paused = false;
                Time.timeScale = 1;
                PauseMenu.SetActive(false);
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
        var height = 2.0f;
        speed = walkSpeed;
        isGrounded = Physics.CheckSphere(groundCheck.position, 0.4f, groundMask);
        if (isGrounded && velocity.y < 0) { velocity.y = -2f; }
        if (grabbed) // we're grabbed, so moving is not an option.
        {
            if (Input.GetKeyUp(EscapeKey))
            {
                escapeCurrent += 1;
                progress.SetProgress(escapeCurrent);
                // Debug.Log("Escape key Pressed " + escapeCurrent + "  times!");
                if (escapeCurrent == escapeMax)
                {
                    //We're free at last!
                    grabbed = false;
                    escapeCurrent = 0;
                    //Zombie Reaction Animation
                    Animator zombieAnim = GrabbedBy.GetComponent<Animator>();
                    zombieAnim.SetBool("Grab", false);
                    zombieAnim.SetTrigger("GrabBreak");
                    // Reset ProgressBar and Hide it
                    EscapeBar.GetComponent<Slider>().value = 0;
                    progress.SetProgress(0);
                    EscapeBar.SetActive(false);
                    //Reset Zombie Attack Cooldown, 5s grace period
                    ZombieBrain GrabBrain = GrabbedBy.GetComponent<ZombieBrain>();
                    GrabBrain.canAttack = false;
                    GrabBrain.Grabbing = false;
                    GrabBrain.AttackCooldown = 5f; // So the zombie can't grab you again immediately, slight grace period where they just chase you without attacking
                    GrabbedBy = null;
                }
            }
            return;
        }
        if (isGrounded && Input.GetButtonDown("Jump") && stamina >= 10)
        {
            velocity.y = 10.85f;
            stamina -= 10;
        }
        if (isGrounded && Input.GetKey("c"))
        {
            height = 1.0f;
            speed = crouchSpeed;
        }
        if (isGrounded && speed != crouchSpeed && Input.GetKey(KeyCode.LeftShift) && (Input.GetAxis("Horizontal") > 0 || Input.GetAxis("Vertical") > 0) && stamina > 0)
        {
            speed = sprintSpeed;
            stamina = Mathf.Clamp(stamina - (25.0f * Time.deltaTime), 0.0f, 100);
        }
        if (speed != crouchSpeed && Input.GetKeyUp(KeyCode.LeftShift) || stamina == 0)
        {
            speed = walkSpeed;
        }
        oldHeight = controller.height;
        controller.height = Mathf.Lerp(controller.height, height, 5 * Time.deltaTime);
        transform.position += new Vector3(0, (controller.height - oldHeight) / 2, 0);
        velocity.y -= 19.62f * Time.deltaTime;
        controller.Move((transform.right * Input.GetAxis("Horizontal") * speed + transform.up * velocity.y + transform.forward * Input.GetAxis("Vertical") * speed) * Time.deltaTime);
        if (speed == crouchSpeed || (Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0) && stamina < 100)
        {
            stamina = Mathf.Clamp(stamina + (15.0f * Time.deltaTime), 0.0f, 100);
        }
        else if (speed == walkSpeed && stamina < 100)
        {
            stamina = Mathf.Clamp(stamina + (5.0f * Time.deltaTime), 0.0f, 100);
        }
        staminaUI.GetComponent<TMP_Text>().text = "Stamina: " + Mathf.Round(stamina);
    }
    // called on movement when grabbed, stores the zombie grabbing us and sets out grabbed value to true,
    public void Grabbed(GameObject Grabber)
    {
        //Grabbed and Grabber
        Mathf.Clamp(stamina - 25, 0.0f, 100);
        staminaUI.GetComponent<TMP_Text>().text = "Stamina: " + Mathf.Round(stamina);
        grabbed = true;
        GrabbedBy = Grabber; // we'll need this later
        //Escape Values
        escapeCurrent = 0;
        escapeMax = Mathf.Round(Random.Range(5f, 15f)); // Set the amount of button presses we need
        EscapeKey = EscapeKeys[Random.Range(0, EscapeKeys.Length - 1)]; // Pick a random key from the list to be clicked
        // Progress Bar
        progress.fillSpeed = escapeMax / 2;
        EscapeBar.GetComponent<Slider>().maxValue = escapeMax;
        EscapeBar.SetActive(true);
        EscapeBar.GetComponentInChildren<TMP_Text>().text = "Press " + EscapeKey.ToString() + " Repeatedly!";
    }
}
