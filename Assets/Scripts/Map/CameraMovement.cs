using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [Header("Position Variables")]
    public Transform target;
    public float smoothing = 0.6f;
    public Vector2 minPosition;
    public Vector2 maxPosition;

    [Header("Animator")]
    public Animator anim;

    [Header("Position Reset")]
    public VectorValue camMin;
    public VectorValue camMax;
    // Start is called before the first frame update
    void Start()
    {
        maxPosition = camMax.Initialvalue;
        minPosition = camMin.Initialvalue;
        anim = GetComponent<Animator>();
        transform.position = new Vector3(target.position.x, target.position.y,
            transform.position.z);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(transform.position != target.position)
        {
            Vector3 targetPosition = new Vector3(target.position.x, target.position.y, transform.position.z);

            targetPosition.x = Mathf.Clamp(targetPosition.x, minPosition.x, maxPosition.x);

            targetPosition.y = Mathf.Clamp (targetPosition.y, minPosition.y, maxPosition.y);

            transform.position = Vector3.Lerp(transform.position, targetPosition, smoothing);
        }
    }

    public void BeginKick()
    {
        anim.SetBool("kick_active", true);
        StartCoroutine(Kickco());
    }

    public IEnumerator Kickco()
    {
        yield return null;
        anim.SetBool("kick_active", false);
    }
}
