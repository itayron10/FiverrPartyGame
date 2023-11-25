using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] BasicHealth health;
    public BasicHealth GetHealth => health;
    [SerializeField] Image healthBarImage;
    [SerializeField] float lerpAmount = 1f;

    private void Start()
    {
        FindPrivateObjects();
    }

    private void OnDestroy()
    {
        health.OnTakingDamage -= Health_OnTakingDamage;
    }


    public virtual void FindPrivateObjects()
    {
        health.OnTakingDamage += Health_OnTakingDamage;
    }

    public virtual void Health_OnTakingDamage(float damage)
    {
        StartCoroutine(UpdateHealth(health, healthBarImage));
    }

    public virtual IEnumerator UpdateHealth(BasicHealth health, Image healthBarImage)
    {
        //Debug.Log("Updating");
        float t = 0f;
        while (t < 1f)
        {
            healthBarImage.fillAmount = Mathf.Lerp
               (healthBarImage.fillAmount, health.GetCurrentHealth / health.GetMaxHealth, t);
            t += lerpAmount * Time.deltaTime;
            yield return null;
        }
    }    
}
