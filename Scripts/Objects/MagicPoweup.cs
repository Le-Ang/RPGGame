using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicPoweup : Powerup
{
    public Inventory playerInventory;
    public float magicValue;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerInventory.currentMagic += magicValue;
            powerupSignal.Raise();
            Destroy(this.gameObject);
        }
    }
}
