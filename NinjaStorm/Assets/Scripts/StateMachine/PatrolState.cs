using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : IState
{
    float timer;
    float randomTime;
    public void OnEnter(Enemy enemy)
    {
        timer = 0;
        randomTime = Random.Range(3f, 6f);
    }

    public void OnExecute(Enemy enemy)
    {

        timer += Time.deltaTime;

        if(enemy.Target != null)
        {
            //Doi huong enemy toi huong cua Player
            
            enemy.ChangeDirection(enemy.Target.transform.position.x > enemy.transform.position.x);
            if(enemy.isTargerInRange())
            {
                enemy.ChangeState(new AttackState());
                
            }
            else
            {
                Debug.Log("Keep Moving");
                enemy.Moving();
            }
            
        }
        else 
        {
            if (timer < randomTime)
            {
                Debug.Log("Keep Moving");
                enemy.Moving();
            }
            else
            {
                
                enemy.ChangeState(new IdleState());
            }
        }

        

    }

    public void OnExit(Enemy enemy)
    {
        
    }

   
}
