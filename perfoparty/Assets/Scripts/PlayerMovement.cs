using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float jumpStrength = 8f;
    [SerializeField] float movementSpeed = 5.0f;
    [SerializeField] float rotationVelocity = 0.0f; // Velocidad de rotación gradual
    [SerializeField] Animator animator;
    [SerializeField] LayerMask groundLayer;
    public int index;

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

        HandleJump();
        HandleDance();
        HandleRotation();

    }

    private void FixedUpdate()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 10f, groundLayer))
        {
            // can jump
        }
    }

    private void HandleJump()
    {
        if (isGrounded)
        {
            if (Input.GetButtonDown("jump"))
            {
                animator.SetBool("dance", false);
                animator.SetBool("Salte", true);
                rb.AddForce(new Vector3(0, jumpStrength, 0), ForceMode.Impulse);
            }
            animator.SetBool("tocarsuelo", true);
        }
        else
        {
            Fall();
        }
    }

    private void HandleDance()
    {
        if (Input.GetButtonDown("dance"))
        {
            animator.SetBool("dance", true);
        }
    }

    private void HandleRotation()
    {
        // Si hay movimiento, gira el personaje hacia la dirección de movimiento gradualmente
        if (moveDirection.magnitude <= 0f) { return; }
        animator.SetBool("dance", false);
        Quaternion rotacionDeseada = Quaternion.LookRotation(moveDirection);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotacionDeseada, Time.deltaTime * rotationVelocity);
    }

    public void UodateMovement(Vector2 movementVector)
    {
        // Calcula la dirección de movimiento
        moveDirection = new Vector3(movementVector.x, 0f, movementVector.y);

        // Actualiza las variables del Animator
        animator.SetFloat("VelX", movementVector.x);
        animator.SetFloat("VelY", movementVector.y);
    }


    public void Fall()
    {
        animator.SetBool("tocarsuelo", false);
        animator.SetBool("Salte", false);
    }
}