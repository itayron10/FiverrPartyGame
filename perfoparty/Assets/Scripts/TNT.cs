using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class TNT : MonoBehaviour
{
    [SerializeField] float timeToExplode;
    [SerializeField] SoundScriptableObject explosionSound;
    [SerializeField] GameObject explosionEffect;
    [SerializeField] TextMeshProUGUI displayText;
    private BasicHealth tntHolder;
    public BasicHealth GetTntHolder => tntHolder;
    public void SetTNTHolder(BasicHealth basicHealth) => tntHolder = basicHealth;
    private SoundManager soundManager;
    private TNTManager tntManager;
    private float explodeTimer;
    private bool tntActive;
    public void SetTntActive(bool active) => tntActive = active;


    private void Start()
    {
        soundManager = FindObjectOfType<SoundManager>();
        tntManager = FindObjectOfType<TNTManager>();
    }

    private void Update()
    {
        displayText.text = (timeToExplode - explodeTimer).ToString("n0");
        
        if (tntActive)
        {
            explodeTimer += Time.deltaTime;
            if (explodeTimer >= timeToExplode)
            {
                Explode();
            }
        }
    }


    private void Explode()
    {
        soundManager.PlaySound(explosionSound);
        ParticleManager.InstanciateParticleEffect(explosionEffect, transform.position, transform.rotation);
        tntHolder.TakeDamage(tntHolder.GetMaxHealth);
        tntManager.SpawnNewTnt();
        Destroy(gameObject);
        explodeTimer = 0f;
    }
}
