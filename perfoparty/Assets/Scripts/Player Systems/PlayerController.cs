using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float jumpStrength = 8f;
    [SerializeField] float movementSpeed = 5.0f;
    [SerializeField] float maxVelocity;
    [SerializeField] float stoppingDrag;
    [SerializeField] float animationInterpelationSpeed;
    [SerializeField] float rotationVelocity = 0.0f; // Velocidad de rotación gradual
    [SerializeField] float groundCheckLength;
    [SerializeField] float fallMultiplier = 2.5f, lowJumpMultiplier = 2f;
    [SerializeField] string isGroundedAnimatorBool, jumpingAnimatorBool, danceAnimatorBool;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] ParticleSystem puffParticleEffect;
    [SerializeField] ScreenShakeSettingsSO landShake;
    [SerializeField] SoundScriptableObject jumpSound, landSound;
    private SoundManager soundManager;
    public Animator animator { get; set; }
    private bool hasLanded;

    private Rigidbody rb;
    private bool isGrounded = true;
    private Vector3 moveDirection;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        soundManager = FindObjectOfType<SoundManager>();
    }



    private void OnLevelWasLoaded(int level)
    {
        soundManager = FindObjectOfType<SoundManager>();
    }


    void Update()
    {

        /*        if (rb.velocity.magnitude > maxVelocity)
                    rb.AddForce(-movmentVector, ForceMode.Force);*//*
                if (movmentVector.magnitude <= 0 && isGrounded) rb.drag = stoppingDrag;
                else rb.drag = 1.5f;*/

        HandleRotation();
        animator.SetFloat("VelX", Mathf.Lerp(animator.GetFloat("VelX"), moveDirection.x, Time.deltaTime * animationInterpelationSpeed));
        animator.SetFloat("VelY", Mathf.Lerp(animator.GetFloat("VelY"), moveDirection.z, Time.deltaTime * animationInterpelationSpeed));
    }

    private void FixedUpdate()
    {
        Vector3 movementVector = moveDirection * movementSpeed;
        rb.MovePosition(Vector3.Lerp(transform.position, transform.position + movementVector, Time.deltaTime * 2f));

        if (rb.velocity.y < 0f && !isGrounded)
        {
            // falling
            Fall();
        }

        if (!isGrounded) hasLanded = false;
        isGrounded = Physics.Raycast(transform.position + new Vector3(0f, 1f, 0f), Vector3.down, groundCheckLength, groundLayer);
        animator.SetBool(isGroundedAnimatorBool, isGrounded);
        if (isGrounded && !hasLanded)
        {
            //Land
            ParticleManager.StartParticle(puffParticleEffect);
            soundManager?.PlaySound(landSound);
            Debug.Log("Playing Land sound " + soundManager);
            CinemachineShake.instance.Shake(landShake);
            hasLanded = true;
        }
    }


    public void Jump()
    {
        if (!isGrounded || Time.timeScale <= 0) return;

        // can jump
        soundManager.PlaySound(jumpSound);
        animator.SetBool(danceAnimatorBool, false);
        ResetAnimatorTransform();
        animator.SetBool(jumpingAnimatorBool, true);
        rb.AddForce(new Vector3(0, jumpStrength, 0), ForceMode.Impulse);
    }


    public void Dance()
    {
        animator.SetBool(danceAnimatorBool, true);
    }

    private void ResetAnimatorTransform()
    {
        animator.transform.localRotation = Quaternion.identity;
        animator.transform.localPosition = Vector3.zero;
    }

    private void HandleRotation()
    {
        rb.angularVelocity = Vector3.zero;
        if (moveDirection.magnitude <= 0f) { return; }

        animator.SetBool(danceAnimatorBool, false);
        ResetAnimatorTransform();
        Quaternion rotationDirection = Quaternion.LookRotation(moveDirection);
        //transform.localPosition = Vector3.zero;
        transform.rotation = Quaternion.Lerp(transform.rotation, rotationDirection, Time.deltaTime * rotationVelocity);
    }

    public void UodateMovement(Vector2 movementVector)
    {
        moveDirection = new Vector3(movementVector.x, 0f, movementVector.y);
    }


    public void Fall()
    {
        animator.SetBool(jumpingAnimatorBool, false);
/*        float jumpPressingValue = inputHandler.GetControls.Player.Jump.ReadValue<float>();
        float roundedYVelocity = Mathf.Round(rb.velocity.y);
        if (roundedYVelocity < 0f)
            rb.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1f) * Time.deltaTime;
        else if (roundedYVelocity > 0f && jumpPressingValue <= 0f)
            rb.velocity += Vector3.up * Physics.gravity.y * (lowJumpMultiplier - 1f) * Time.deltaTime;*/
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * groundCheckLength);
    }
}