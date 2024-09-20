using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private HeroKnight playerScript;

    [SerializeField] private Animator animator;

    public int maxHealth = 100;
    private int currentHealth;

    private int attackDamage = 20;
    [SerializeField] private Vector3 attackOffset;
    private float attackRange = 0.5f;
    [SerializeField] private LayerMask attackMask;

    private float blockChance = 0.5f;

    private void Start()
    {
        playerScript = FindObjectOfType<HeroKnight>();

        currentHealth = maxHealth;
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
        animator.SetTrigger("Death");
        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;
        StartCoroutine(DestroyEnemy());
    }

    private IEnumerator DestroyEnemy()
    {
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }

    public void Attack()
    {
        Vector3 pos = transform.position;
        pos += transform.right * attackOffset.x;
        pos += transform.up * attackOffset.y;

        Collider2D hit = Physics2D.OverlapCircle(pos, attackRange, attackMask);
        if (hit != null)
        {
            hit.GetComponent<HeroKnight>().TakeDamage(attackDamage);
        }
    }
}
