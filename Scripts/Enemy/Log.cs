using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Log : Enemy
{
    public Rigidbody2D rb;
    [Header("Target Variables")]
    public Transform target;
    public float distanceView;
    public float distanceAttack;

    [Header("Animator")]
    public Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        state = EnemyState.idle;
        rb = GetComponent<Rigidbody2D>();
        target = GameObject.FindWithTag("Player").transform;
        anim = gameObject.GetComponent<Animator>();
        anim.SetBool("WakeUp", true);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        CheckDistance();
    }

    public virtual void CheckDistance()
    {
        if(Vector3.Distance(target.position, transform.position) <= distanceView
            && Vector3.Distance(target.position, transform.position) > distanceAttack)
        {
            if(state == EnemyState.idle || state == EnemyState.move
                && state != EnemyState.stagger)
            {
                Vector3 temp = Vector3.MoveTowards(transform.position, target.position,
                    moveSpeed * Time.deltaTime);
                changeAnim(temp - transform.position);
                rb.MovePosition(temp);
                ChangeState(EnemyState.move);
                anim.SetBool("WakeUp",true); 
            }
        }else if(Vector3.Distance(target.position, transform.position) > distanceView)
        {
            anim.SetBool("WakeUp", false);
        }
    }

    public void SetAnimFloat(Vector2 setVector)
    {
        anim.SetFloat("MoveX", setVector.x);
        anim.SetFloat("MoveY", setVector.y);
    }
    public void changeAnim(Vector2 direction) 
    {
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            if(direction.x > 0)
            {
                SetAnimFloat(Vector2.right);
            }else if(direction.x < 0)
            {
                SetAnimFloat(Vector2.left);
            }
        }else if(Mathf.Abs(direction.x) < Mathf.Abs(direction.y))
        {
            if(direction.y > 0)
            {
                SetAnimFloat(Vector2.up);
            }
            else if(direction.y < 0)
            {
                SetAnimFloat(Vector2.down);
            }
        }
    }
    public void ChangeState(EnemyState newState)
    {
        if(state != newState)
        {
            state = newState;
        }
    }
}
