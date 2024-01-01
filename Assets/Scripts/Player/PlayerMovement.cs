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
    public Inventory playerInventory;
    public SpriteRenderer receivedItemSprite;
    public SignalSender playerHit;
    public SignalSender reduceMagic;

    [Header("Projectile Stuff")]
    public GameObject projectile;
    public Item bow;

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
        //Is the player in an interation
        if(currentState == PlayerState.Interract)
        {
            return;
        }
        change = Vector3.zero;
        change.x = Input.GetAxis("Horizontal");
        change.y = Input.GetAxis("Vertical");
        if(Input.GetButtonDown("Attack") && currentState != PlayerState.Attacking 
            && currentState != PlayerState.Stagger)
        {
            StartCoroutine(AttackingCo());
        }
        else if (Input.GetButtonDown("SecondWeapon") && currentState != PlayerState.Attacking
            && currentState != PlayerState.Stagger)
        {
            if (playerInventory.CheckForItem(bow))
            {
                StartCoroutine(SecondAttackCo());
            }            
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
        if(currentState != PlayerState.Interract)
        {
            currentState = PlayerState.Moving;
        }

    }

    private IEnumerator SecondAttackCo()
    {
        //animator.SetBool("Attacking", true);
        currentState = PlayerState.Attacking;
        yield return null;
        MakeArrow();
        //animator.SetBool("Attacking", false);
        yield return new WaitForSeconds(0.3f);
        if (currentState != PlayerState.Interract)
        {
            currentState = PlayerState.Moving;
        }
    }

    private void MakeArrow()
    {
        if(playerInventory.currentMagic > 0)
        {
            Debug.Log("Make Arrow");
            Vector2 temp = new Vector2(animator.GetFloat("MoveX"), animator.GetFloat("MoveY"));
            Arrow arrow = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<Arrow>();
            arrow.Setup(temp, ChooseArrowDirection());
            playerInventory.ReduceMagic(arrow.magicCost);
            reduceMagic.Raise();
        }        
    }

    Vector3 ChooseArrowDirection()
    {
        float temp = Mathf.Atan2(animator.GetFloat("MoveY"), animator.GetFloat("MoveX") * Mathf.Rad2Deg);
        return new Vector3(0, 0, temp);
    }
    public void RaiseItem()
    {
        if(playerInventory.currentItem != null)
        {
            if(currentState != PlayerState.Interract)
            {
                animator.SetBool("Receive Item", true);
                currentState = PlayerState.Interract;
                receivedItemSprite.sprite = playerInventory.currentItem.itemSprite;
            }
            else
            {
                animator.SetBool("Receive Item", false);
                currentState = PlayerState.Idle;
                receivedItemSprite.sprite = null;
                playerInventory.currentItem = null;
            }
        }
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
            this.gameObject.SetActive(false);
        }
    }
    private IEnumerator KnockCo(float knockTime)
    {
        playerHit.Raise();
        if (rb != null)
        {
            yield return new WaitForSeconds(knockTime);
            rb.velocity = Vector2.zero;
            currentState = PlayerState.Idle;
            rb.velocity = Vector2.zero;
        }
    }
}
