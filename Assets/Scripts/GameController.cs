using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private Player player;
    public int enemyTargetCount;

    public static GameController Instance { get; private set; }
    public UIController UIController { get; private set; }

    public static Action OnGameEnd;
    public static Action OnDisablePlayerInput;
    public static Action OnEnablePlayerInput;

    private int enemyKillCount;
    private bool isGamePaused = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;
        Time.timeScale = 1;
        UIController = GetComponentInChildren<UIController>();
    }

    private void OnEnable()
    {
        Enemy.OnEnemyDeath += OnEnemyDeathHandler;
        GameInput.OnPauseGame += OnPauseGameHandler;
        Player.OnPlayerDeath += OnPlayerDeathHandler;
        OnGameEnd += OnGameEndHandler;
    }

    private void OnDisable()
    {
        Enemy.OnEnemyDeath -= OnEnemyDeathHandler;
        GameInput.OnPauseGame -= OnPauseGameHandler;
        Player.OnPlayerDeath -= OnPlayerDeathHandler;
        OnGameEnd -= OnGameEndHandler;
    }

    private void OnGameEndHandler()
    {
        Time.timeScale = 0;
    }

    private void OnEnemyDeathHandler()
    {
        ++enemyKillCount;
        if (enemyKillCount >= enemyTargetCount)
        {
            OnGameEnd?.Invoke();
            UIController.DisplayEndScreen("You won");
        }
    }

    private void OnPauseGameHandler()
    {
        isGamePaused = !isGamePaused;
        UIController.TogglePauseScreen();
        if (isGamePaused)
        {
            Time.timeScale = 0;
            Debug.Log("Game Paused");
        }
        else
        {
            Time.timeScale = 1;
            Debug.Log("Game UnPaused");
        }
    }

    private void OnPlayerDeathHandler()
    {
        OnGameEnd?.Invoke();
        UIController.DisplayEndScreen("You Lost");
    }

}
