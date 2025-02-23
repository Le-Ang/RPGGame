using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum PlayerState {Idle, Moving, Attacking, Interact, Stagger, Ability}
public class PlayerMovement : MonoBehaviour
{
    public PlayerState currentState;
    public float speed;
    private Rigidbody2D rb;
    private Vector3 change;
    private Animator animator;
    [SerializeField] private GenericAbility currentAbility;

    private Vector2 tempMovement = Vector2.down;
    private Vector2 facingDirection = Vector2.down;

    //TODO HEALTH
    /*
    public FloatValue currentHealth;
    public SignalZelda playerHealthSignal;
    */

    public VectorValue startingPosition;

    //TODO INVENTORY break off the player inventory into its own component
    public Inventory playerInventory;
    public SpriteRenderer receivedItemSprite;

    //TODO HEALTH player hit should be part of the health system
    public SignalZelda playerHit;

    //TODO MAGIC player magic should be part of the magic system
    public SignalZelda reduceMagic;

    //TODO IFRAME break off the iframe stuff into it own script
    [Header("IFrame Stuff")]
    public Color flashColor;
    public Color regularColor;
    public float flashDuration;
    public int numberOfFlashes;
    public Collider2D triggerCollider;
    public SpriteRenderer mySprite;

    //TODO ABILITY break this off with the player ability system
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
        transform.position = startingPosition.initialvalue;
    }

    // Update is called once per frame
    private void Update()
    {
        //Is the player in an interation
        if(currentState == PlayerState.Interact)
        {
            return;
        }
        change.x = Input.GetAxisRaw("Horizontal");
        change.y = Input.GetAxisRaw("Vertical");
        if(Input.GetButtonDown("Weapon Attack") && currentState != PlayerState.Attacking 
            && currentState != PlayerState.Stagger)
        {
            StartCoroutine(AttackingCo());
        }
        else if (Input.GetButtonDown("Ability"))
        {
            if (!IsRestrictedState(currentState))
            {
                StartCoroutine(AbilityCo(currentAbility.duration));
            }
        }
        //TODO ABILITY
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
    bool IsRestrictedState(PlayerState currentState)
    {
        if (currentState == PlayerState.Attacking || currentState == PlayerState.Ability)
        {
            return true;
        }
        return false;
    }
    private IEnumerator AttackingCo()
    {
        animator.SetBool("Attacking", true);
        currentState = PlayerState.Attacking;
        yield return null;
        animator.SetBool("Attacking", false);
        yield return new WaitForSeconds(0.3f);
        if(currentState != PlayerState.Interact)
        {
            currentState = PlayerState.Moving;
        }

    }

    //TODO ABILITY
    private IEnumerator SecondAttackCo()
    {
        //animator.SetBool("Attacking", true);
        currentState = PlayerState.Attacking;
        yield return null;
        MakeArrow();
        //animator.SetBool("Attacking", false);
        yield return new WaitForSeconds(0.3f);
        if (currentState != PlayerState.Interact)
        {
            currentState = PlayerState.Moving;
        }
    }

    //TODO ABILITY this should be part of the ability itself
    private void MakeArrow()
    {
        if(playerInventory.currentMagic > 0)
        {
            Vector2 temp = new Vector2(animator.GetFloat("MoveX"), animator.GetFloat("MoveY"));
            Arrow arrow = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<Arrow>();
            arrow.Setup(temp, ChooseArrowDirection());
            playerInventory.ReduceMagic(arrow.magicCost);
            reduceMagic.Raise();
        }        
    }

    //TODO ABILITY this should also be part the ability
    Vector3 ChooseArrowDirection()
    {
        float temp = Mathf.Atan2(animator.GetFloat("MoveY"), animator.GetFloat("MoveX")) * Mathf.Rad2Deg;
        return new Vector3(0, 0, temp);
    }
    public void RaiseItem()
    {
        if(playerInventory.currentItem != null)
        {
            if(currentState != PlayerState.Interact)
            {
                animator.SetBool("Receive Item", true);
                currentState = PlayerState.Interact;
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
            MoveCharacter();
            change.x = Mathf.Round(change.x);
            change.y = Mathf.Round(change.y);
            animator.SetFloat("MoveX",change.x);
            animator.SetFloat("MoveY",change.y);
            animator.SetBool("Moving", true);
            facingDirection = change;
        }
        else
        {
            animator.SetBool("Moving", false) ;
        }
    }

    void MoveCharacter()
    {
        change.Normalize();
        rb.MovePosition(transform.position + change * speed * Time.deltaTime);
    }
    private void FixedUpdate()
    {
        change.Normalize();
        rb.MovePosition(transform.position + change * speed * Time.deltaTime);
    }

    //TODO KNOCKBACK move the knockback to its own script
    public void Knock(float knockTime)
    {
        StartCoroutine(KnockCo(knockTime));

        ////TODO HEALTH
        //currentHealth.RuntimeValue -= damage;
        //playerHealthSignal.Raise();
        //if (currentHealth.RuntimeValue > 0)
        //{
        //    //TODO HEALTH
        //    playerHit.Raise();
        //}
        //else
        //{
        //    this.gameObject.SetActive(false);
        //}

    }
    private IEnumerator KnockCo(float knockTime)
    {
        if (rb != null)
        {
            StartCoroutine(FlashCo());
            yield return new WaitForSeconds(knockTime);
            rb.velocity = Vector2.zero;
            currentState = PlayerState.Idle;
            rb.velocity = Vector2.zero;
        }
    }

    //TODO IFRAMES move the player flashing to its own script
    private IEnumerator FlashCo()
    {
        int temp = 0;
        triggerCollider.enabled = false;
        while (temp < numberOfFlashes)
        {
            mySprite.color = flashColor;
            yield return new WaitForSeconds(flashDuration);
            mySprite.color = regularColor;
            yield return new WaitForSeconds(flashDuration);
            temp++;
        }
        triggerCollider.enabled = true;
    }
    public IEnumerator AbilityCo(float abilityDurarion)
    {
        currentState = PlayerState.Ability;
        currentAbility.Ability(transform.position,
                facingDirection, animator, rb);
        yield return new WaitForSeconds(abilityDurarion);
        currentState = PlayerState.Idle;
    }
}
