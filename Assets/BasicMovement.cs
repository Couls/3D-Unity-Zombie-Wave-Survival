using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicMovement : MonoBehaviour
{
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float speed = 12f;
    bool isGrounded;
    float oldHeight = 2.0f;
    float walkSpeed;
    float crouchSpeed;
    CharacterController controller;
    Vector3 velocity;
    void Start()
    {
        controller = transform.GetComponent<CharacterController>();
        oldHeight = controller.height;
        crouchSpeed = speed / 2;
        walkSpeed = speed;
    }
    // Update is called once per frame
    void Update()
    {
        var height = 2.0f;
        speed = walkSpeed;
        isGrounded = Physics.CheckSphere(groundCheck.position, 0.4f, groundMask);
        if (isGrounded && velocity.y < 0) { velocity.y = -2f; }
        if (isGrounded && Input.GetButtonDown("Jump")) { velocity.y = 10.85f; }
        if (isGrounded && Input.GetKey("c"))
        {
            height = 1.0f;
            speed = crouchSpeed;
        }
        oldHeight = controller.height;
        controller.height = Mathf.Lerp(controller.height, height, 5 * Time.deltaTime);
        transform.position += new Vector3(0, (controller.height - oldHeight) / 2, 0);
        velocity.y -= 19.62f * Time.deltaTime;
        controller.Move((transform.right * Input.GetAxis("Horizontal") * speed + transform.up * velocity.y + transform.forward * Input.GetAxis("Vertical") * speed) * Time.deltaTime);
    }
}
