using System;
using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] protected HealthBar healthBar;
    [SerializeField] private CombatText combatTextPrefab;
    private float hp;
    private string currentAnim;

    public bool isDead => hp <= 0;

    private void Start()
    {
        OnInit();
        UIManager.instance.SetCoin(0);
    }

    public virtual void OnInit()
    {
        hp = 100;
        healthBar.Oninit(100f,transform);
    }

    public virtual void OnDespawn()
    {
        
    }
    public virtual void OnDeath()
    {
        
        ChangeAnim("Die");
        Invoke(nameof(OnDespawn), 1.5f);
    }

    public void OnHit(float damage)
    {
        Debug.Log("Hit");
        if(!isDead)
        {
            hp -= damage;
            if(isDead)
            {
                hp = 0f;
                OnDeath();
            }
            healthBar.SetNewHP(hp);
            Instantiate(combatTextPrefab, transform.position + Vector3.up, Quaternion.identity).Oninit(damage);
        }
    }

    

    protected void ChangeAnim(string animName)
    {

        if (currentAnim != animName)
        {
            Debug.Log(animName);
            anim.ResetTrigger(currentAnim);
            currentAnim = animName;
            anim.SetTrigger(currentAnim);

        }
    }

    
}
