using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Knockback : MonoBehaviour
{
    [SerializeField] private float thrust;
    [SerializeField] private float knockTime;
    [SerializeField] private string collisionTag;
    //public float damage;
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag(collisionTag) && collision.isTrigger)
        {
                Rigidbody2D temp = collision.GetComponentInParent<Rigidbody2D>();
                if (temp)
                {
                    Vector2 direction = collision.transform.position - transform.position;
                    temp.DOMove((Vector2)collision.transform.position +
                        (direction.normalized * thrust), knockTime);
                }
            //private IEnumerator Destroy()
            //{
            //    yield return new WaitForSeconds(0.4f);
            //}
        }
    }
}
