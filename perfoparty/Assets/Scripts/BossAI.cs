using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossAI : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float punchRange, chaseRange;
    [SerializeField] LayerMask playerLayer;
    [SerializeField] float punchKnockback;
    [SerializeField] float animationDelay;
    [SerializeField] string attackingBoolParam, movementFloatParam;
    [SerializeField] float rotationSpeed;
    [SerializeField] Animator animator;
    public bool active;
    private bool isAttacking;
    private NavMeshAgent agent;
    private Transform currentTarget;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
    }

    private void Update()
    {
        if (active) UpdateAI();
    }

    private void UpdateAI()
    {
        Collider[] players = Physics.OverlapSphere(transform.position, chaseRange, playerLayer);
        List<Transform> playersTransforms = new List<Transform>();
        foreach (Collider player in players) playersTransforms.Add(player.transform);
        currentTarget = DetectionHelper.GetClosest(playersTransforms, transform.position);

        if (currentTarget == null) return;

        RotateTowardsContinually(currentTarget.position);

        if (Vector3.Distance(transform.position, currentTarget.position) <= punchRange)
        {
            if (!isAttacking) StartCoroutine(Attack());
        }
        else
        {
            animator.SetFloat(movementFloatParam, agent.velocity.magnitude);
            agent.SetDestination(currentTarget.position);
        }
    }

    public void RotateTowardsContinually(Vector3 targetPos, float rotationSpeedMultiplier = 1f)
    {
        Quaternion targetRotation = Quaternion.LookRotation(targetPos - transform.position);

        //rotate Smoothly towards target
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation,
            Time.deltaTime * rotationSpeed * rotationSpeedMultiplier);

        //set the x and y rotations to 0 so the object won't glitch with the navmesh agent
        Vector3 eulerAngles = transform.rotation.eulerAngles;
        eulerAngles.x = 0;
        eulerAngles.z = 0;

        // Set the new rotation back
        transform.rotation = Quaternion.Euler(eulerAngles);
    }

    private IEnumerator Attack()
    {
        isAttacking = true;
        animator.SetBool(attackingBoolParam, true);
        yield return new WaitForSeconds(animationDelay);
        if (Vector3.Distance(currentTarget.position, transform.position) <= punchRange)
        {
            // PUNCH
            Rigidbody targetRb = currentTarget.GetComponent<Rigidbody>();
            if (targetRb != null) PunchController.ApplyPunchKnockback(targetRb.position, targetRb, punchKnockback, transform.forward);
        }

        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length - animationDelay);
        
        isAttacking = false;
        animator.SetBool(attackingBoolParam, false);

    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, chaseRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, punchRange);
    }
}

