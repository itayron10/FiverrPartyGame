using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunchController : MonoBehaviour
{
    [SerializeField] string punchAnimatorTrigger;
    [SerializeField] float punchRange;
    [SerializeField] float punchDamage;
    [SerializeField] LayerMask punchLayer;
    [SerializeField] Vector3 punchOriginOffset;
    private PlayerMovement playerMovement;
    private Animator animator;

    private void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        animator = playerMovement.animator;
    }

    public void Punch()
    {
        animator.SetTrigger(punchAnimatorTrigger);
        Collider[] colliders = Physics.OverlapSphere(transform.position + transform.TransformVector(punchOriginOffset), punchRange, punchLayer);
        foreach (Collider collider in colliders)
        {
            if (collider.transform.gameObject == gameObject) { continue; }
            if (collider.transform.TryGetComponent<BasicHealth>(out BasicHealth health)) health.TakeDamage(punchDamage, collider.ClosestPoint(transform.position + punchOriginOffset)); 
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position + transform.TransformVector(punchOriginOffset), punchRange);
    }
}
