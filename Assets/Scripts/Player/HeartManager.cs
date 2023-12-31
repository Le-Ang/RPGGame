using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartManager : MonoBehaviour
{
    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite halfFullHeart;
    public Sprite emptyHeart;
    public FloatValue heartContainers;
    public FloatValue playerCurrentHealth;
    // Start is called before the first frame update
    void Start()
    {
        InitHearts();
    }

    public void InitHearts()
    {
        for (int i = 0; i < heartContainers.RuntimeValue; i++){
            hearts[i].gameObject.SetActive(true);
            hearts[i].sprite = fullHeart;
        }
    }

    public void UpdateHearts()
    {
        InitHearts();
        float tempHealth = playerCurrentHealth.RuntimeValue / 2;
        for(int i = 0; i< heartContainers.RuntimeValue; i++)
        {
            if(i <= tempHealth-1)
            {
                Debug.Log("Full Heart");
                hearts[i].sprite = fullHeart;
            }else if(i >= tempHealth)
            {
                Debug.Log("Empty Heart");
                hearts[i].sprite = emptyHeart;
            }
            else
            {
                Debug.Log("half full heart");
                hearts[i].sprite = halfFullHeart;
            }
        }
    }
}
