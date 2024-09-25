using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private HeroKnight playerScript;

    [SerializeField] private Animator animator;

    public int maxHealth = 100;
    private int currentHealth;

    public float maxStamina = 100f;
    private float currentStamina;
    public float CurrentStamina { get { return currentStamina; } }
    private float staminaRegenRate = 0.1f;
    private float m_timeSinceLastAttack = 0.0f;

    private int attackDamage = 10;
    [SerializeField] private Vector3 attackOffset;
    private float attackRange = 0.5f;
    [SerializeField] private LayerMask attackMask;

    private float blockChance = 0.2f;

    private void Start()
    {
        playerScript = FindObjectOfType<HeroKnight>();

        currentHealth = maxHealth;
        currentStamina = maxStamina;
    }

    private void Update()
    {
        if (playerScript.transform.position.x + 1.8f >= gameObject.transform.position.x)
        {
            if (playerScript.isRunning)
            {
                playerScript.UpdateGameMode(true);
            }

            m_timeSinceLastAttack += Time.deltaTime;

            if (m_timeSinceLastAttack > 1f && currentStamina < maxStamina)
            {
                currentStamina += staminaRegenRate;
                currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
            }
        }
    }

    public void TakeDamage(int damage)
    {
        if (Random.value < blockChance)
        {
            animator.SetBool("Block", true);
            StartCoroutine(StopBlock());

            return;
        }

        currentHealth -= damage;
        animator.SetTrigger("Hurt");
        Debug.Log(currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private IEnumerator StopBlock()
    {
        yield return new WaitForSeconds(0.5f);
        animator.SetBool("Block", false);
    }

    private void Die()
    {
        GameManager.Instance.UpdatePlayerScore(100);

        animator.SetTrigger("Death");
        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;
        StartCoroutine(DestroyEnemy());
    }

    private IEnumerator DestroyEnemy()
    {
        yield return new WaitForSeconds(2f);
        playerScript.UpdateGameMode(false);
        Destroy(gameObject);
    }

    public void Attack()
    {
        Vector3 pos = transform.position;
        pos += transform.right * attackOffset.x;
        pos += transform.up * attackOffset.y;

        int attackStaminaDrain = 20;

        Collider2D hit = Physics2D.OverlapCircle(pos, attackRange, attackMask);
        if (hit != null)
        {
            currentStamina -= attackStaminaDrain;
            hit.GetComponent<HeroKnight>().TakeDamage(attackDamage);
            m_timeSinceLastAttack = 0.0f;
        }
    }

    public float GetHealthPercentage()
    {
        return Mathf.Clamp((float)currentHealth / maxHealth, 0, 1);
    }

    public float GetStaminaPercentage()
    {
        return Mathf.Clamp((float)currentStamina / maxStamina, 0, 1);
    }
}
