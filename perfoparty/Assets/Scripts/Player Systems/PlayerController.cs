using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float jumpStrength = 8f;
    [SerializeField] float movementSpeed = 5.0f;
    [SerializeField] float rotationVelocity = 0.0f; // Velocidad de rotación gradual
    [SerializeField] float groundCheckLength;
    [SerializeField] string isGroundedAnimatorBool, jumpingAnimatorBool, danceAnimatorBool;
    [SerializeField] LayerMask groundLayer;
    public Animator animator;

    private Rigidbody rb;
    private bool isGrounded = true;
    private Vector3 moveDirection;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }


    void Update()
    {
        transform.Translate(moveDirection * movementSpeed * Time.deltaTime, Space.World);
        HandleRotation();
    }

    private void FixedUpdate()
    {
        isGrounded = Physics.Raycast(transform.position + new Vector3(0f, 1f, 0f), Vector3.down, groundCheckLength, groundLayer);
        animator.SetBool(isGroundedAnimatorBool, isGrounded);

        if (rb.velocity.y < 0f && !isGrounded)
        {
            // falling
            Fall();
        }
    }


    public void Jump()
    {
        if (isGrounded)
        {
            // can jump
            animator.SetBool(danceAnimatorBool, false);
            animator.SetBool(jumpingAnimatorBool, true);
            rb.AddForce(new Vector3(0, jumpStrength, 0), ForceMode.Impulse);
        }
    }


    public void Dance()
    {
        animator.SetBool(danceAnimatorBool, true);
    }

    private void HandleRotation()
    {
        if (moveDirection.magnitude <= 0f) { return; }

        animator.SetBool(danceAnimatorBool, false);
        Quaternion rotationDirection = Quaternion.LookRotation(moveDirection);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotationDirection, Time.deltaTime * rotationVelocity);
    }

    public void UodateMovement(Vector2 movementVector)
    {
        moveDirection = new Vector3(movementVector.x, 0f, movementVector.y);

        animator.SetFloat("VelX", movementVector.x);
        animator.SetFloat("VelY", movementVector.y);
    }


    public void Fall()
    {
        animator.SetBool(jumpingAnimatorBool, false);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * groundCheckLength);
    }
}