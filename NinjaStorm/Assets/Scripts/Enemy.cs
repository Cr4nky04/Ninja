using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{

    [SerializeField] private float attackRange;
    [SerializeField] private float moveSpeed;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private GameObject attackArea;

    private IState currentState;
    private bool isRight = true;
    private Character target;
    public Character Target => target;


    private void Update()
    {
        if (currentState != null && !isDead)
        {
            currentState.OnExecute(this);
        }
    }

    public override void OnInit()
    {
        base.OnInit();
        ChangeState(new IdleState());
        DeactiveAttack();
    }

    public override void OnDespawn()
    {

        base.OnDespawn();
        Destroy(healthBar.gameObject);
        Destroy(gameObject);
    }

    public override void OnDeath()
    {
        ChangeState(null);
        base.OnDeath();
    }



    public void ChangeState(IState newState)
    {
        if (currentState != null)
        {
            currentState.OnExit(this);
        }
        currentState = newState;
        
        if (currentState != null)
        {
            currentState.OnEnter(this);
        }

    }

    internal void SetTarget(Character character)
    {
        this.target = character;
        if (isTargerInRange())
        {
            ChangeState(new AttackState());
        }
        else
        if (Target != null)
        {
            ChangeState(new PatrolState());
        }
        else
        {
            
            ChangeState(new IdleState());
        }
    }
    public void Moving()
    {
        ChangeAnim("Run");
        rb.velocity = transform.right * moveSpeed;
    }

    public void StopMoving()
    {   
            ChangeAnim("Idle");
            rb.velocity = Vector2.zero;             
    }

    public void IdleThenAttack()
    {
        ChangeAnim("Idle");
    }

    public void Attack()
    {

        ChangeAnim("Attack");
        ActiveAttack();
        Invoke(nameof(DeactiveAttack), 0.5f);
    }


    public bool isTargerInRange()
    {
        if (target != null && Vector2.Distance(target.transform.position, transform.position) < attackRange)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void ActiveAttack()
    {
        attackArea.SetActive(true);
    }

    private void DeactiveAttack()
    {
        attackArea.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "EnemyWal")
        {
            ChangeDirection(!isRight);
        }
    }

    public void ChangeDirection(bool isRight)
    {
        this.isRight = isRight;
        transform.rotation = isRight ? Quaternion.Euler(Vector3.zero) : Quaternion.Euler(Vector3.up * 180);
    }


}
