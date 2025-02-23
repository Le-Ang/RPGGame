using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class GenericProjectile : MonoBehaviour
{
    [SerializeField] private Rigidbody2D myRb;
    [SerializeField] private float mySpeed;
    // Start is called before the first frame update
    void Start()
    {
        myRb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    public void Setup(Vector2 moveDirection)
    {
        myRb.velocity = moveDirection.normalized * mySpeed;
    }
}
