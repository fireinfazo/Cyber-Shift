using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class healthBar : MonoBehaviour
{
    public Slider healthSlider;
    public Slider easeHealthSlider;
    public float maxHealth = 100f;
    public float health;
    private float LerpSpeed = 0.03f;
    private float lasttimedrain = 0f;
    void Start()
    {
        health = maxHealth;
    }
    void Update()
    {
        if(healthSlider.value != health)
        {
            healthSlider.value = health;  
        }

        if(healthSlider.value != easeHealthSlider.value)
        {
            easeHealthSlider.value = Mathf.Lerp(easeHealthSlider.value, health, LerpSpeed);
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            DrainHP(10);
        }
    }

    void TakeDamage(float damage)
    {
        health -= damage;
    }

    public void DrainHP(float hpdrain)
    {
        if(Time.deltaTime < lasttimedrain + 10f)
        {
            // StartCoroutine(DrainHpCoroutine(hpdrain));
            health -= hpdrain;
            //health = Mathf.Max(health, 0);
            lasttimedrain = Time.deltaTime;
        }
       
    }
    IEnumerator DrainHpCoroutine(float hpdrain)
    {
        
        yield return new WaitForSeconds(1f);
    }
}
