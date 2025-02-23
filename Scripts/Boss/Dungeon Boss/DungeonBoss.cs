using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DungeonBossState {Sleep, Idle, Move, Interact}
public class DungeonBoss : MonoBehaviour
{
    public DungeonBossState dBossState;
    public GameObject player;
    public Animator anim;
    private bool facingRight = true;
    public float distanceView;
    public float speed;
    public float distanceAttack;
    public float bossHealth;
    public float currentHealth;
    public DBossHealthBar dbHealthBar;
    public float dBossAmor;
    public Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = bossHealth;
        dbHealthBar.SetMaxHealth(bossHealth);
        anim = gameObject.GetComponent<Animator>();
        dBossState = DungeonBossState.Sleep;
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    //to control the boss movement
    private void Movement()
    {
        float distance = Vector3.Distance(gameObject.transform.position, player.transform.position);
        if (distance <= distanceView)
        {
            if (dBossState == DungeonBossState.Sleep)
            {
                anim.SetTrigger("Movement");
                Invoke("Delay",1.5f);
            }
            else if(dBossState == DungeonBossState.Move)
            {
                gameObject.transform.position = Vector3.MoveTowards(transform.position,
                    player.transform.position, speed * Time.deltaTime);
                rb.MovePosition(gameObject.transform.position);
                if (transform.position.x > player.transform.position.x && facingRight)
                {
                    Flip();
                }
                else if (transform.position.x < player.transform.position.x && !facingRight)
                {
                    Flip();
                }
            }            
        }
    }

    //to flip the boss face attach to the player
    private void Flip()
    {
        facingRight = !facingRight;
        transform.Rotate(0f, 180f, 0f);
    }

    //to delay boss movement when anim action
    public void Delay()
    {
        dBossState = DungeonBossState.Move;
    }

    //to reduce boss health when it take damage
    public void TakeDamage(int damage)
    {
        currentHealth = damage - dBossAmor;

        dbHealthBar.SetHealth(currentHealth);
    }
}
