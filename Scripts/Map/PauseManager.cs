using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    private bool isPaused;
    public GameObject pausePanel;
    public GameObject inventoryPanel;
    public bool usingPausePanel;
    public string mainmenu;
    // Start is called before the first frame update
    void Start()
    {
        isPaused = false;
        pausePanel.SetActive(false);
        inventoryPanel.SetActive(false);
        usingPausePanel = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("pause"))
        {
            ChangePause();
        }
        if (Input.GetButtonDown("Open Inventory"))
        {
            ShowInventory();
        }
    }
    public void ChangePause()
    {
        isPaused = !isPaused;
        if (isPaused)
        {
            pausePanel.SetActive(true);
            Time.timeScale = 0f;
            usingPausePanel = true;
        }
        else
        {
            inventoryPanel.SetActive(false);
            pausePanel.SetActive(false);
            Time.timeScale = 1f;
        }
    }

    public void ShowInventory()
    {
        usingPausePanel = !usingPausePanel;
        if (usingPausePanel && Input.GetButtonDown("pause"))
        {
            inventoryPanel.SetActive(false);
            isPaused = false;
            Time.timeScale = 1f;
        }
        else
        {
            inventoryPanel.SetActive(true);
            isPaused = true;
            Time.timeScale = 0f;
        }
    }
    public void QuitToMain()
    {
        SceneManager.LoadScene(mainmenu);
        Time.timeScale = 1f;
    }
}
