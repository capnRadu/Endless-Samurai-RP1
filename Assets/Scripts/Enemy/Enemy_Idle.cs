using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Idle : StateMachineBehaviour
{
    private Transform player;
    private HeroKnight playerScript;
    private Enemy enemyScript;

    private float attackRange = 2f;
    private float attackCooldown;
    private float lastAttackTime = 0f;
    private int attackStaminaDrain = 20;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        playerScript = FindObjectOfType<HeroKnight>();
        player = playerScript.gameObject.transform;
        enemyScript = animator.GetComponent<Enemy>();

        attackCooldown = Random.Range(0, 4f);
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (Vector2.Distance(player.position, animator.transform.position) <= attackRange)
        {
            if (Time.time >= lastAttackTime + attackCooldown)
            {
                if (enemyScript.CurrentStamina - attackStaminaDrain >= 0)
                {
                    animator.SetTrigger("Attack");
                    lastAttackTime = Time.time;
                }
            }
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("Attack");
        animator.SetBool("Block", false);
    }
}
