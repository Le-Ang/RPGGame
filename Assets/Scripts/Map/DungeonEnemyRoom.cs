using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonEnemyRoom : DungeonRoom
{
    public Door[] door;

    private void Start()
    {
        OpenDoors();
    }
    public void CheckEnemies()
    {
        for(int i = 0; i < door.Length; i++)
        {
            if (enemies[i].gameObject.activeInHierarchy && i < enemies.Length -1)
            {
                return;
            }
        }
        OpenDoors();
    }

    public override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !collision.isTrigger)
        {
            //Activate all enemies and pots
            for (int i = 0; i < enemies.Length; i++)
            {
                ChangeActivation(enemies[i], true);
            }
            for (int i = 0; i < pots.Length; i++)
            {
                ChangeActivation(pots[i], true);
            }
            Invoke("CloseDoors", 0.5f);
            virtualCamera.SetActive(true);
        }
    }
    public override void OnTriggerExit2D(Collider2D collision)
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
            virtualCamera.SetActive(false);
        }
    }

    public void CloseDoors()
    {
        for(int i = 0; i < door.Length; i++)
        {
            door[i].Close();
        }
    }

    public void OpenDoors()
    {
        for(int i = 0; i < door.Length;i++)
        {
            door[i].Open();
        }
    }
}
