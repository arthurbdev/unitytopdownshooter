using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameUI : MonoBehaviour
{
    private int enemyTargetCount;
    private int enemyKillCount = 0;
    [SerializeField] private TextMeshProUGUI missionObjectiveText;

    private void OnEnable()
    {
        Enemy.OnEnemyDeath += UpdateMissionObjective;
    }

    private void OnDisable()
    {
        Enemy.OnEnemyDeath -= UpdateMissionObjective;
    }

    private void Start()
    {
        enemyTargetCount = GameController.Instance.enemyTargetCount;
        enemyKillCount = 0;
        missionObjectiveText.text = $"Mission target: kill {enemyTargetCount} enemies\n{enemyKillCount}/{enemyTargetCount}";
    }

    private void UpdateMissionObjective()
    {
        enemyKillCount++;
        missionObjectiveText.text = $"Mission target: kill {enemyTargetCount} enemies\n{enemyKillCount}/{enemyTargetCount}";
    }
}
