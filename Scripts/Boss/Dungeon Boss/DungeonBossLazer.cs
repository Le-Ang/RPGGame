using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonBossLazer : MonoBehaviour
{
    private Animator lazerAnim;
    // The target marker.
    public Transform target;

    // Angular speed in radians per sec.
    public float speed = 10.0f;

    // Start is called before the first frame update
    void Start()
    {
        lazerAnim = GetComponent<Animator>();
        if (gameObject.activeSelf ==true)
        {
            gameObject.GetComponent<BoxCollider2D>().enabled = true;
            lazerAnim.SetTrigger("Fire");
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Get the target's position
        Vector3 targetPosition = target.transform.position;

        // Compute the direction vector from the current object to the target object
        Vector3 direction = targetPosition - transform.position;

        // Calculate the angle in radians and convert it to degrees
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Set the rotation of the object to face the target object
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    private void Visuals()
    {
        Vector3 dir = target.position - transform.position;
        Quaternion rot = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), speed * Time.deltaTime);
        rot.x = 0;
        rot.y = 0;
        transform.rotation = rot;
    }
}
