using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolLog : Log
{
    public Transform[] path;
    public int currentPoint;
    public Transform currentGoal;
    public float roundingDistance;
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
                //ChangeState(EnemyState.move);
                anim.SetBool("WakeUp", true);
            }
        }
        else if (Vector3.Distance(target.position, transform.position) > distanceView)
        {
            if(Vector3.Distance(transform.position, path[currentPoint].position)
                > roundingDistance)
            {
                Vector3 temp = Vector3.MoveTowards(transform.position, path[currentPoint].position,
                        moveSpeed * Time.deltaTime);
                changeAnim(temp - transform.position);
                rb.MovePosition(temp);
            }
            else
            {
                ChangeGoal();
            }

        }
    }
    private void ChangeGoal()
    {
        if(currentPoint == path.Length - 1)
        {
            currentPoint = 0;
            currentGoal = path[0];
        }
        else
        {
            currentPoint++;
            currentGoal = path[currentPoint];
        }
    }
}
