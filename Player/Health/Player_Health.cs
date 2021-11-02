using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Health : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;

    public float invicibilityFlashDelay = 0.2f;
    public float invicibilityTimeAfterHit = 3f;
    public bool isInvicible = false;

    public SpriteRenderer graphics;
    public HealthBar healthBar;

    public bool isDead = false;

    public Animator animator;

    public Enemy_Script Enemy;

    public GameOverManager gameOverManager;

    public CameraFollow cameraFollow;

    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    public void TakeDamage(int damage)
    {
        if (!isInvicible)
        {
            currentHealth -= damage;
            healthBar.SetHealth(currentHealth);

            if (currentHealth <= 0)
            {
                Die();
                return;
            }

            isInvicible = true;
            StartCoroutine(InvincibilityFlash());
            StartCoroutine(HandleInvicibilityDelay());
        }

    }

    public IEnumerator InvincibilityFlash()
    {
        animator.SetTrigger("Damaged");
        yield return new WaitForSeconds(0.7f);
        while (isInvicible)
        {
            graphics.color = new Color(1f, 1f, 1f, 0f);
            yield return new WaitForSeconds(invicibilityFlashDelay);
            graphics.color = new Color(1f, 1f, 1f, 1f);
            yield return new WaitForSeconds(invicibilityFlashDelay);
        }
    }

    public IEnumerator HandleInvicibilityDelay()
    {
        yield return new WaitForSeconds(invicibilityTimeAfterHit);
        isInvicible = false;
    }

    public void Die()
    {
        PlayerMovement.instance.enabled = false;
        cameraFollow.enabled = false;
        Debug.Log("Player Dead");
        animator.SetTrigger("Dead");
        gameOverManager.OnPlayerDeath();
    }
}

