using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    private Transform target;
    public float speed;
    public float rotateSpeed = 200;
    public Rigidbody2D myRigidbody;
    public float lifetime;
    private float lifetimeCounter;

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        lifetimeCounter = lifetime;        
    }
    private void Update()
    {
        Vector3 targetPosition = target.transform.position;

        // Compute the direction vector from the current object to the target object
        Vector3 direction = targetPosition - transform.position;

        // Calculate the angle in radians and convert it to degrees
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Set the rotation of the object to face the target object
        transform.rotation = Quaternion.Euler(0, 0, angle);
        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        lifetimeCounter -= Time.deltaTime;
        if (lifetimeCounter <= 0)
        {
            Destroy(this.gameObject);
        }
    }
    public void Setup(Vector2 velocity, Vector3 direction)
    {
        myRigidbody.velocity = velocity.normalized * speed;
        transform.rotation = Quaternion.Euler(direction);
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Destroy(this.gameObject);
        }
    }
}
