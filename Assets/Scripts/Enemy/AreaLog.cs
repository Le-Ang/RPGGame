using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaLog : Log
{
    public BoxCollider2D boundary;

    public override void CheckDistance()
    {
        if (Vector3.Distance(target.position, transform.position) <= distanceView
            && Vector3.Distance(target.position, transform.position) > distanceAttack
            && boundary.bounds.Contains(target.transform.position))
        {
            if (state == EnemyState.idle || state == EnemyState.move
                && state != EnemyState.stagger)
            {
                Vector3 temp = Vector3.MoveTowards(transform.position, target.position,
                    moveSpeed * Time.deltaTime);
                changeAnim(temp - transform.position);
                rb.MovePosition(temp);
                ChangeState(EnemyState.move);
                anim.SetBool("WakeUp", true);
            }
        }
        else if (Vector3.Distance(target.position, transform.position) > distanceView 
            || !boundary.bounds.Contains(target.transform.position))
        {
            anim.SetBool("WakeUp", false);
        } 
    }
}
