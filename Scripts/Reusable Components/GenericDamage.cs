using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class GenericDamage : MonoBehaviour
{
    [SerializeField] private float damage;
    [SerializeField] private string collisionTag;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(collisionTag) && collision.isTrigger)
        {
            GenericHealth temp = collision.GetComponent<GenericHealth>();
            if (temp)
            {
                temp.Damage(damage);
            }
        }
    }
}
