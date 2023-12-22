using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DoorType
{
    key, enemy, button
}
public class Door : Interactable
{
    [Header("Door variables")]
    public DoorType doorType;
    public bool open = false;
    public Inventory playerInventory;
    public SpriteRenderer doorSprite;
    public BoxCollider2D physicsCollider;

    // Update is called once per frame
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            if (playerInRange && doorType == DoorType.key)
            {
                //Does the player have a key?
                if(playerInventory.numberOfKeys >0)
                {
                    //Remove a player key
                    playerInventory.numberOfKeys--;
                    //If so, call the open method
                    Open();
                }
            }
        }
    }

    public void Open()
    {
        //Turn off the door's sprite renderer
        doorSprite.enabled = false;
        //Set open to true
        open = true;
        //turn off the door's box collider
        physicsCollider.enabled = false;
    }
}
