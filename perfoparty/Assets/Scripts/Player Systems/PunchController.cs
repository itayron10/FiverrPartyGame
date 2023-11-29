using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunchController : MonoBehaviour
{
    [SerializeField] string punchAnimatorTrigger;
    [SerializeField] float punchRange;
    [SerializeField] float punchDamage;
    [SerializeField] float punchKnockBack;
    [SerializeField] LayerMask punchLayer;
    [SerializeField] Vector3 punchOriginOffset;
    [SerializeField] ScreenShakeSettingsSO punchShake;
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
            if (collider.gameObject == gameObject) { continue; }

            Vector3 hitPos = collider.ClosestPoint(transform.position + punchOriginOffset);

            if (collider.TryGetComponent<BasicHealth>(out BasicHealth health)) health.TakeDamage(punchDamage, hitPos);
            if (collider.TryGetComponent<Rigidbody>(out Rigidbody rb))
            {
                float punchForce = punchKnockBack / (1f - rb.drag * Time.fixedDeltaTime);
                rb.AddForceAtPosition(transform.forward * punchForce, hitPos, ForceMode.VelocityChange);
/*                rb.AddForce((rb.position - transform.position).normalized
                    * punchKnockBack / (1f - rb.drag * Time.fixedDeltaTime), ForceMode.Impulse);*/
                rb.AddForceAtPosition(transform.up * punchForce / 2f, hitPos, ForceMode.VelocityChange);
                CinemachineShake.instance.Shake(punchShake);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position + transform.TransformVector(punchOriginOffset), punchRange);
    }
}
