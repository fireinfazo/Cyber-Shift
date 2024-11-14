using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class healthBar : MonoBehaviour
{
    public Slider healthSlider;
    public Slider easeHealthSlider;
    public Slider RenewableHealthSlider;
    public float maxHealth = 100f;
    public float health;
    private float LerpSpeed = 0.03f;
    private float lasttimedrain = 0f;
    private float lasttimegain = 0f;
    [SerializeField] private int countofheal = 3;
    void Start()
    {
        health = maxHealth;
    }
    void Update()
    {
        if (healthSlider.value != health)
        {
            healthSlider.value = health;
        }

        if (healthSlider.value != easeHealthSlider.value)
        {
            easeHealthSlider.value = Mathf.Lerp(easeHealthSlider.value, health, LerpSpeed);
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            DrainHP(90);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            GainHP(25);
        }
    }

    void TakeDamage(float damage)
    {
        health -= damage;
    }

    public void DrainHP(float hpdrain)
    {
        if (Time.time > lasttimedrain + 1f)
        {
            // StartCoroutine(DrainHpCoroutine(hpdrain));
            health -= hpdrain;
            health = Mathf.Max(health, 0);
            lasttimedrain = Time.time;
        }

    }
    public void GainHP(float hpgain)
    {
        if (Time.time > lasttimegain + 1f && countofheal > 0 && health < maxHealth)
        {
            health += hpgain;
            countofheal -= 1;
            if(health > maxHealth)
            {
                health = maxHealth;
            }
            //health = Mathf.Max(health, 100);
            lasttimegain = Time.time;
        }
    }
}
