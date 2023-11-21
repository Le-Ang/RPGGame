using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Knockback : MonoBehaviour
{
    public float thrust;
    public float knockTime;
    public float damage;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Ontrigger with pot
        if (collision.CompareTag("broken") && this.gameObject.CompareTag("Player"))
        {
            collision.GetComponent<Pot>().Broken();
            StartCoroutine(Destroy());
            Destroy(collision);
        }
        //Ontrigger with enemy
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Player"))
        {
            Rigidbody2D hit = collision.GetComponent<Rigidbody2D>();
            if (hit != null)
            {
                Vector2 difference = hit.transform.position - transform.position;
                difference = difference.normalized * thrust;
                hit.AddForce(difference, ForceMode2D.Impulse);
                if (collision.gameObject.CompareTag("Enemy") && collision.isTrigger)
                {
                    hit.GetComponent<Enemy>().state = EnemyState.stagger;
                    collision.GetComponent<Enemy>().Knock(hit, knockTime, damage);
                }
                if (collision.gameObject.CompareTag("Player"))
                {
                    if(collision.GetComponent<PlayerMovement>().currentState != PlayerState.Stagger)
                    hit.GetComponent<PlayerMovement>().currentState = PlayerState.Stagger;
                    collision.GetComponent<PlayerMovement>().Knock(knockTime, damage);
                }

                Debug.Log("coroutine");
            }
        }
    }
    private IEnumerator Destroy()
    {
        yield return new WaitForSeconds(0.4f);
    }
}
