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
    [SerializeField] float punchCoolDown;
    [SerializeField] SoundScriptableObject punchSound;
    private PlayerMovement playerMovement;
    private Animator animator;
    private SoundManager soundManager;
    private TntHolder tntHolder;
    private WeaponManager weaponManager;
    private bool canPunch = true;
    public bool hasPunchAbility = true;

    public float GetKnockBack => punchKnockBack;
    public void SetKnockBack(float knockBack) => punchKnockBack = knockBack;


    private void Awake()
    {
        PlayerConfigurationManager.Instance.originalPlayerKnockBack = punchKnockBack;
    }

    private void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        tntHolder = GetComponent<TntHolder>();
        weaponManager = GetComponent<WeaponManager>();
        soundManager = FindObjectOfType<SoundManager>();
        animator = playerMovement.animator;
        canPunch = true;
    }


    private void OnLevelWasLoaded(int level)
    {
        soundManager = FindObjectOfType<SoundManager>();
        hasPunchAbility = true;
    }

    public void Punch()
    {
        if (weaponManager.GetCurrentWeapon != null) return;
        if (!hasPunchAbility) return;
        if (!canPunch) return;
        HandlePunch();
    }

    private void HandlePunch()
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
                ApplyPunchKnockback(hitPos, rb, punchKnockBack, transform.forward);
                CinemachineShake.instance.Shake(punchShake);
                soundManager.PlaySound(punchSound);
            }

            if (collider.TryGetComponent<BossHealth>(out BossHealth boss))
            {
                boss.PralysisPlayers(this.gameObject);
            }

            if (collider.TryGetComponent<TntHolder>(out TntHolder tntHolder))
            {
                if (this.tntHolder.GetTnt != null)
                {
                    tntHolder.EquipTnt(this.tntHolder.GetTnt);
                    this.tntHolder.UnequipTnt();
                }
            }
        }
        StartCoroutine(ActivePunchCooldown());
    }

    private IEnumerator ActivePunchCooldown()
    {
        canPunch = false;
        yield return new WaitForSeconds(punchCoolDown);
        canPunch = true;

    }

    public static void ApplyPunchKnockback(Vector3 hitPos, Rigidbody rb, float punchKnockbackForce, Vector3 direction)
    {
        float punchForce = punchKnockbackForce / (1f - rb.drag * Time.fixedDeltaTime);
        rb.AddForceAtPosition(direction * punchForce, hitPos, ForceMode.VelocityChange);
        /*                rb.AddForce((rb.position - transform.position).normalized
                            * punchKnockBack / (1f - rb.drag * Time.fixedDeltaTime), ForceMode.Impulse);*/
        rb.AddForceAtPosition(Vector3.up * punchForce / 2f, hitPos, ForceMode.VelocityChange);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position + transform.TransformVector(punchOriginOffset), punchRange);
    }
}
