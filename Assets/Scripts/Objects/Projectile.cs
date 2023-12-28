using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("Movement Stuff")]
    public float moveSpeed;
    public Vector2 directionToMove;

    [Header("Lifetime")]
    public float lifeTime;
    private float lifeTimeSeconds;
    public Rigidbody2D myRigidbody;

    // Start is called before the first frame update
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        lifeTimeSeconds = lifeTime;
    }

    // Update is called once per frame
    void Update()
    {
        lifeTimeSeconds -= Time.deltaTime;
        if(lifeTimeSeconds <= 0)
        {
            Destroy(this.gameObject);
        }
    }
    public void Launch(Vector2 initialVelocity)
    {
        myRigidbody.velocity = initialVelocity * moveSpeed;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy (this.gameObject);
    }
}
