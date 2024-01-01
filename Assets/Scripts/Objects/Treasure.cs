using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Treasure : Interactable
{
    [Header("Contents")]
    public Item contents;
    public Inventory playerInventory;
    public bool isOpen;
    public BoolValue storedOpen;

    [Header("Signals Dialog")]
    public SignalSender raiseItem;
    public GameObject dialogBox;
    public Text dialogText;

    [Header("Animation")]
    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        isOpen = storedOpen.RuntimeValue;
        if (isOpen)
        {
            anim.SetBool("opened", true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Attack") && playerInRange)
        {
            if(!isOpen)
            {
                //Open Chest
                OpenChets();
            }
            else
            {
                //Chest is already open
                ChetsAlreadyOpen();
            }
        }
    }

    public void OpenChets()
    {
        //Dialog window on
        dialogBox.SetActive(true);
        //dialog text = contents text
        dialogText.text = contents.itemDescription;
        //add contents to the inventory
        playerInventory.AddItem(contents);
        playerInventory.currentItem = contents;
        //Raise the signal to the player to animate
        raiseItem.Raise();
        //raise the context clue
        context.Raise();
        //Set the chest to opened
        isOpen = true;
        anim.SetBool("opened", true);
        storedOpen.RuntimeValue = isOpen;
    }

    public void ChetsAlreadyOpen()
    {
            //Dialog off
            dialogBox.SetActive(false);
            //raise the signal to the player to stop animating
            raiseItem.Raise();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !collision.isTrigger && !isOpen)
        {
            context.Raise();
            playerInRange = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !collision.isTrigger && !isOpen)
        {
            context.Raise();
            playerInRange = false;
        }
    }
}
