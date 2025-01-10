using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class healthBar : MonoBehaviour
{
    public Slider healthSlider;
    public float maxHealth = 100f;
    public float health;
    [SerializeField] private float defense = 10;
    [SerializeField] private float hplimit = 400;
    [SerializeField] private float testdamage = 10;
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
        if (maxHealth >= hplimit)
        {
            maxHealth = hplimit;
        }

        if (health > maxHealth)
        {
            health = maxHealth;
        }

        if (healthSlider.value != health)
        {
            healthSlider.value = health;
        }

        if (healthSlider.maxValue != maxHealth)
        {
            healthSlider.maxValue = maxHealth;
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            if (testdamage <= 4)
            {
                DrainHP(testdamage);
            }
            else
            {
                DrainHP(testdamage * (testdamage / (testdamage + defense)));
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            GainHP(10);
        }
    }

    void TakeDamage(float damage)
    {
        health -= damage - (defense / 2);
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
