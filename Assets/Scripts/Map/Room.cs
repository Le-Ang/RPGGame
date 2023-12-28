using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public Enemy[] enemies;
    public Pot[] pots;

    public virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") && !collision.isTrigger)
        {
            //Activate all enemies and pots
            for(int i = 0; i < enemies.Length; i++)
            {
                ChangeActivation(enemies[i], true);
            }
            for(int i = 0;i < pots.Length; i++)
            {
                ChangeActivation(pots[i], true);
            }
        }
    }
    public virtual void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !collision.isTrigger)
        {
            //Deactivate all enemies and pots
            for (int i = 0; i < enemies.Length; i++)
            {
                ChangeActivation(enemies[i], false);
            }
            for (int i = 0; i < pots.Length; i++)
            {
                ChangeActivation(pots[i], false);
            }
        }
    }

    void ChangeActivation(Component component, bool activation)
    {
        component.gameObject.SetActive(activation);
    }
}
