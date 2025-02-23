using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("Movement Stuff")]
    public float moveSpeed;
    public Vector2 directionToMove;
    public Rigidbody2D myRigidbody;

    // Start is called before the first frame update
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    
    public void Launch(Vector2 initialVelocity)
    {
        myRigidbody.velocity = initialVelocity * moveSpeed;
    }
}
