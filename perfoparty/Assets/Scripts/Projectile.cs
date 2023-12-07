using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("Params")]
    [SerializeField] float speed;
    [Tooltip("the allowed range for the projectile to go in before destroyed if 'destroyOutOfRange' is true")]
    [SerializeField] float destroyRange;
    [Tooltip("the damage amount the projectile will take")]
    [SerializeField] float damage;
    [Tooltip("is the projectile get destroyedonce he is out of range")]
    [SerializeField] bool destroyOutOfRange; // if true the projectile is destroyed once he leaves the range
    [Tooltip("if the projectile hit something before traveling in that range he will ignore the hit")]
    [SerializeField] float minHitRange = 1f;
    [SerializeField] GameObject destroyEffect;
    
    [Header("References")]
    private Rigidbody rb;
    private Vector3 startPos; // the position of the projectile in the start

    public void SetProjectileRange(float range) { this.destroyRange = range; }
    public void SetProjectileDamage(float damage) { this.damage = damage; }


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        startPos = transform.position;
        rb.velocity = transform.forward * speed;
    }

    private void Update()
    {
        if (!destroyOutOfRange) { return; }
        
        if (Vector3.Distance(startPos, transform.position) > destroyRange)
        {
            Destroy(gameObject);
            Debug.Log($"Destroyed By out of range");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // check if we close to the start point so the projectile won't get destroyed if hit the shooter collider
        Debug.Log(startPos);
        Debug.Log(transform.position);
        if (Vector3.Distance(startPos, transform.position) < minHitRange) { return; }
        // if we hit someone with health we damage hit and self destroy
        if (other.TryGetComponent<BasicHealth>(out BasicHealth damagable))
            damagable.TakeDamage(damage, transform.position, Quaternion.Inverse(transform.rotation));

        Debug.Log($"Destroyed By {other.gameObject.name}");
        Debug.Log(Vector3.Distance(startPos, transform.position));
        if (destroyEffect) ParticleManager.InstanciateParticleEffect(destroyEffect, transform.position, transform.rotation);
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, destroyRange);
    }
}
