using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    [SerializeField] private GameObject pauseScreen;
    [SerializeField] private GameObject endScreen;
    [SerializeField] private TextMeshProUGUI endScreenText;

    public void DisplayEndScreen(string gameOverText)
    {
        endScreen.SetActive(true);
        endScreenText.text = gameOverText;
    }

    public void TogglePauseScreen()
    {
        if (pauseScreen.activeSelf) pauseScreen.SetActive(false);
        else pauseScreen.SetActive(true);
    }
    public void Resume()
    {
        GameInput.OnPauseGame?.Invoke();
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene("Level 1");
    }
}
