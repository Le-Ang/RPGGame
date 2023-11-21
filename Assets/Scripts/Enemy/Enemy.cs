using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyState { idle, move, attack, stagger}
public class Enemy : MonoBehaviour
{
    public EnemyState state;
    public FloatValue enemyMaxHealth;
    public float enemyHealth;
    public string enemyName;
    public int enemyDamage;
    public float moveSpeed;

    private void Awake()
    {
        enemyHealth = enemyMaxHealth.initialValue;
    }
    private void TakeDamage(float damage)
    {
        enemyHealth -= damage;
        if(enemyHealth <= 0)
        {
            //Destroy(gameObject);
            Debug.Log("death");
        }
    }
    public void Knock(Rigidbody2D rb, float knockTime, float damage)
    {
        StartCoroutine(KnockCo(rb, knockTime));
        TakeDamage(damage);
    }
    private IEnumerator KnockCo(Rigidbody2D rb, float knockTime)
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
