using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : Log
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public override void CheckDistance()
    {
        if (Vector3.Distance(target.position, transform.position) <= distanceView
            && Vector3.Distance(target.position, transform.position) > distanceAttack)
        {
            if (state == EnemyState.idle || state == EnemyState.move
                && state != EnemyState.stagger)
            {
                Vector3 temp = Vector3.MoveTowards(transform.position, target.position,
                    moveSpeed * Time.deltaTime);
                changeAnim(temp - transform.position);
                rb.MovePosition(temp);
                ChangeState(EnemyState.move);
            }
        }
        else if (Vector3.Distance(target.position, transform.position) <= distanceView
            && Vector3.Distance(target.position, transform.position) <= distanceAttack)
        {
            if (state == EnemyState.move && state != EnemyState.stagger)
            {
                StartCoroutine(AttackCo());
            }
        }
    }
    public IEnumerator AttackCo()
    {
        state = EnemyState.attack;
        anim.SetBool("Attack", true);
        yield return new WaitForSeconds(1f);
        state = EnemyState.move;
        anim.SetBool("Attack", false);
    }
}
