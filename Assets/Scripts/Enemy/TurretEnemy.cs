using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretEnemy : Log
{
    public GameObject projectile;
    public float fireDelay;
    private float fireDelaySeconds;
    public bool canFire = true;

    private void Update()
    {
        fireDelaySeconds -= Time.deltaTime;
        if( fireDelaySeconds <= 0)
        {
            canFire = true;
            fireDelaySeconds = fireDelay;
        }
    }
    public override void CheckDistance()
    {
        if (Vector3.Distance(target.position, transform.position) <= distanceView
            && Vector3.Distance(target.position, transform.position) > distanceAttack)
        {
            if (state == EnemyState.idle || state == EnemyState.move
                && state != EnemyState.stagger)
            {
                if (canFire)
                {
                    Vector3 tempVector = target.transform.position - transform.position;
                    GameObject current = Instantiate(projectile, transform.position, Quaternion.identity);
                    current.GetComponent<Projectile>().Launch(tempVector);
                    canFire = false;
                    ChangeState(EnemyState.move);
                    anim.SetBool("WakeUp", true);   
                }
                
            }
        }
        else if (Vector3.Distance(target.position, transform.position) > distanceView)
        {
            anim.SetBool("WakeUp", false);
        }
    }
}
