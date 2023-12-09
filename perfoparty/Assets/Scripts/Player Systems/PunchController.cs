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
    [SerializeField] GameObject stunEffect;
    private List<BasicHealth> playerPunchedPralysis = new List<BasicHealth>();
    private PlayerMovement playerMovement;
    private Animator animator;
    private SoundManager soundManager;
    private TntHolder tntHolder;
    private WeaponManager weaponManager;
    private FinalBossMiniGameManager finalBoss;
    private ParkourMiniGameManager parkourManager;
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
        finalBoss = FindObjectOfType<FinalBossMiniGameManager>();
        parkourManager = FindObjectOfType<ParkourMiniGameManager>();
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
        if (parkourManager) hasPunchAbility = false;
        animator.SetTrigger(punchAnimatorTrigger);
        Collider[] colliders = Physics.OverlapSphere(transform.position + transform.TransformVector(punchOriginOffset), punchRange, punchLayer);
        foreach (Collider collider in colliders)
        {
            if (collider.gameObject == gameObject) { continue; }

            Vector3 hitPos = collider.ClosestPoint(transform.position + punchOriginOffset);

            if (collider.TryGetComponent<BasicHealth>(out BasicHealth health))
            {
                if (playerPunchedPralysis.Contains(health)) return;
                health.TakeDamage(punchDamage, hitPos);
                if (finalBoss) StartCoroutine(StunPlayer(health.GetComponent<InputHandler>().playerConfig, health));
            }


            if (collider.TryGetComponent<Rigidbody>(out Rigidbody rb))
            {
                ApplyPunchKnockback(hitPos, rb, punchKnockBack, transform.forward);
                CinemachineShake.instance.Shake(punchShake);
                soundManager.PlaySound(punchSound);
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

    private IEnumerator StunPlayer(PlayerConfiguration player, BasicHealth health)
    {
        playerPunchedPralysis.Add(health);
        player.inputHandler.GetComponent<PlayerMovement>().SetElectro(true);
        player.Input.DeactivateInput();
        ParticleManager.InstanciateParticleEffect(stunEffect, player.inputHandler.transform.position, Quaternion.identity, 3, player.inputHandler.transform);
        yield return new WaitForSeconds(3f);
        player.Input.ActivateInput();
        player.inputHandler.GetComponent<PlayerMovement>().SetElectro(false);
        playerPunchedPralysis.Remove(health);
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
