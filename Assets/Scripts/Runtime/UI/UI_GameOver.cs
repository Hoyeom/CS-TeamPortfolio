using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_GameOver : MonoBehaviour
{
    [SerializeField] private TMP_Text curScore;
    [SerializeField] private TMP_Text bestScore;
    [SerializeField] private TMP_Text curCoin;

    private void OnEnable()
    {
        curScore.text = $"{Managers.Game.Score}";
        bestScore.text = $"{Managers.Game.BestScore}";
        curCoin.text = $"{Managers.Game.Coin}";
        Managers.Data.SaveData();
    }

    public void Restart()
    {
        Managers.Scene.LoadScene(Define.Scene.Menu);
    }
}
