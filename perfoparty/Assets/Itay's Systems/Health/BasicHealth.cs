using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BasicHealth : MonoBehaviour
{
    [Header("Settings")]
    // the health that the object start with and the max heath the object can get to
    [Tooltip("How much health the object get on default")]
    [SerializeField] float maxHealth = 100f;
    // hit sound is the sound that will be played when TakeDamage is called
    // death sound will be played when Die is called
    [SerializeField] SoundScriptableObject hitSound, deathSound;
    [SerializeField] GameObject deathEffectPrefab, hitEffectPrefab;

    [Header("References")]

    // reference for the current health amount 
    private float currentHealth;
    private bool initialized;

    // reference for the audio source of this health component, used to play the hit and death sounds
    private AudioSource healthAudioSource;
    private SoundManager soundManager;

    // event that invokes when taking damage and it containes the damage amount the object was damaged in
    public delegate void OnHit(float damage);
    public event OnHit OnTakingDamage;

    public float GetCurrentHealth => currentHealth;
    public float GetMaxHealth => maxHealth;


    private void Start() => currentHealth = maxHealth;


    /// <summary>
    /// thie method called on start and you can override this method and get private objects
    /// </summary>
    public virtual void FindPrivateObjects()
    {
        // get private objects
        soundManager = FindObjectOfType<SoundManager>();
        healthAudioSource = GetComponent<AudioSource>();
        initialized = true;
    }



    /// <summary>
    /// this method is called for damaging the object and it gets a damage amount
    /// and invokes the OnTakingDamage event 
    /// can be overriden for different kinds of actions when taking damage
    /// </summary>
    public virtual void TakeDamage(float damageAmount, Vector3 damagePos = default, Quaternion hitEffectRotation = default)
    {
        if (!initialized) FindPrivateObjects();
        // clamps the current health between 0f and the max health 
        // decrease the damageAmount from the currentHealth
        currentHealth = Mathf.Clamp(currentHealth - damageAmount, 0f, maxHealth);
        // invokes the OnTakingDamage event with the damage amount
        OnTakingDamage?.Invoke(damageAmount);
        // not playing hit effect for healing
        if (damageAmount < 0f) { return; }
        // instantiate a hit effect
        ParticleManager.InstanciateParticleEffect(hitEffectPrefab,
            damagePos != default ? damagePos : transform.position, hitEffectRotation != default ? hitEffectRotation : transform.rotation);
        // if the current health is 0 or lower initiate the die function
        // if we are not dying then play the hit sound
        if (currentHealth <= 0f)
            Die(damagePos);
        else
            soundManager.PlaySound(hitSound, healthAudioSource);
    }

    /// <summary>
    /// this method is called in the TakeDamage method when the currentHealth is 0 or lower and it can be ovveriden
    /// for different kinds of actions when dying 
    /// </summary>
    public virtual void Die(Vector3 damagePos)
    {
        if (!initialized) FindPrivateObjects();
        // plays the death effects
        PlayDeathEffects();
        // destroyes the gameobject
        Destroy(gameObject);
    }

    protected void PlayDeathEffects()
    {
        // play sound on death
        SoundManager.PlaySoundAtPosition(deathSound, transform.position, 1);
        // instantiate the death effect
        ParticleManager.InstanciateParticleEffect(deathEffectPrefab, transform.position, Quaternion.identity);
        CinemachineShake.instance.Shake();
    }

}
