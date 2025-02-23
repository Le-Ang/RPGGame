using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyState { idle, move, attack, stagger}
public class Enemy : MonoBehaviour
{
    [Header("State Machine")]
    public EnemyState state;

    [Header("Enemy Stats")]
    public FloatValue enemyMaxHealth;
    public float enemyHealth;
    public string enemyName;
    public int enemyDamage;
    public float moveSpeed;
    public Vector2 homePosition;

    [Header("Death Effects")]
    public GameObject deathEffect;
    private float deathEffectDelay = 1f;
    public LootTable thisLoot;

    [Header("Death Signals")]
    public SignalZelda roomSignal;
    private void Awake()
    {
        enemyHealth = enemyMaxHealth.initialValue;
    }

    private void OnEnable()
    {
        transform.position = homePosition;
        enemyHealth = enemyMaxHealth.initialValue;
        state = EnemyState.idle;
    }
    private void Update()
    {
        enemyHealth = gameObject.GetComponent<GenericHealth>().currentHealth;
        if (enemyHealth <= 0)
        {
            DeathEffect();
            MakeLoot();
            if (roomSignal != null)
            {
                roomSignal.Raise();
            }
            this.gameObject.SetActive(false);
        }
    }
    private void TakeDamage(float damage)
    {
        enemyHealth -= damage;
        if(enemyHealth <= 0)
        {
            DeathEffect();
            MakeLoot();
            if(roomSignal != null)
            {
                roomSignal.Raise();
            }
            this.gameObject.SetActive(false);
        }
    }

    private void MakeLoot()
    {
        if(thisLoot != null)
        {
            Powerup current = thisLoot.LootPowerup();
            if(current != null)
            {
                Instantiate(current.gameObject, transform.position, Quaternion.identity);
            }
        }
    }

    private void DeathEffect()
    {
        if(deathEffect != null)
        {
            GameObject effect = Instantiate(deathEffect, transform.position, Quaternion.identity);
            Destroy(effect, deathEffectDelay);
        }
    }
    public void Knock(Rigidbody2D rb, float knockTime)
    {
        StartCoroutine(KnockCo(rb, knockTime));
    }
    public IEnumerator KnockCo(Rigidbody2D rb, float knockTime)
    {
        if (rb != null)
        {
            yield return new WaitForSeconds(knockTime);
            rb.velocity = Vector2.zero;
            state = EnemyState.idle;
            rb.velocity = Vector2.zero;
        }
    }
}
