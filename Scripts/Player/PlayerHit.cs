using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHit : MonoBehaviour
{
    public GameObject pot;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("broken"))
        {
            collision.GetComponent<Pot>().Broken();
            StartCoroutine(Destroy());
        }
    }

    private IEnumerator Destroy()
    {
        yield return new WaitForSeconds(0.4f);
        Destroy(pot);
    }
}
