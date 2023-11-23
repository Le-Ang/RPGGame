using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum PlayerState {Idle, Moving, Attacking, Interract, Stagger}
public class PlayerMovement : MonoBehaviour
{
    public PlayerState currentState;
    public float speed;
    private Rigidbody2D rb;
    private Vector3 change;
    private Animator animator;
    public FloatValue currentHealth;
    public SignalSender playerHealthSignal;
    public VectorValue startingPosition;

    // Start is called before the first frame update
    private void Start()
    {
        currentState = PlayerState.Moving;
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        animator.SetFloat("MoveX", 0);
        animator.SetFloat("MoveY", -1);
        transform.position = startingPosition.Initialvalue;
    }

    // Update is called once per frame
    private void Update()
    {
        change = Vector3.zero;
        change.x = Input.GetAxis("Horizontal");
        change.y = Input.GetAxis("Vertical");
        if(Input.GetButtonDown("Attack") && currentState != PlayerState.Attacking 
            && currentState != PlayerState.Stagger)
        {
            StartCoroutine(AttackingCo());
        }
        else if(currentState == PlayerState.Moving || currentState == PlayerState.Idle)
        {
            UpdateAnimationAndMove(); 
        }          
    }
     
    private IEnumerator AttackingCo()
    {
        animator.SetBool("Attacking", true);
        currentState = PlayerState.Attacking;
        yield return null;
        animator.SetBool("Attacking", false);
        yield return new WaitForSeconds(0.3f);
        currentState = PlayerState.Moving;
    }
    private void UpdateAnimationAndMove()
    {
        if(change != Vector3.zero)
        {
            animator.SetFloat("MoveX",change.x);
            animator.SetFloat("MoveY",change.y);
            animator.SetBool("Moving", true);
        }
        else
        {
            animator.SetBool("Moving", false) ;
        }
    }
    private void FixedUpdate()
    {
        change.Normalize();
        rb.MovePosition(transform.position + change * speed * Time.deltaTime);
    }
    public void Knock(float knockTime, float damage)
    {
        currentHealth.RuntimeValue -= damage;
        playerHealthSignal.Raise();
        if (currentHealth.RuntimeValue > 0)
        {
            StartCoroutine(KnockCo(knockTime));
        }
        else
        {
            //this.gameObject.SetActive(false);
            Debug.Log("PlayerDeath");
        }
    }
    private IEnumerator KnockCo(float knockTime)
    {
        if (rb != null)
        {
            yield return new WaitForSeconds(knockTime);
            rb.velocity = Vector2.zero;
            currentState = PlayerState.Idle;
            rb.velocity = Vector2.zero;
        }
    }
}
