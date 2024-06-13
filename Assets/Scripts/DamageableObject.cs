using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageableObject : MonoBehaviour
{
    public float health;
    private float maxHealth;
    public Image healthImg;
    public Canvas canv;

    private void Start()
    {
        InitStartHealth();
    }

    public void TakeDamage(float damageAmount)
    {
        Debug.Log("Получил урон " + damageAmount);
        health -= damageAmount;
        UpdateHealthImg();
        if (health <= 0)
            Die();
    }

    private void UpdateHealthImg()
    {
        canv.enabled = true;
        healthImg.fillAmount = health / maxHealth;
    }

    public void InitStartHealth()
    {
        maxHealth = health;
        canv = GetComponentInChildren<Canvas>();
        healthImg = canv.transform.GetChild(1).GetComponent<Image>();
        canv.enabled = false;
    }

    virtual public void Die()
    {
        Destroy(gameObject);
    }
}
