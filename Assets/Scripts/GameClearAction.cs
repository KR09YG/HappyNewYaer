using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameClearAction", menuName = "Scriptable Objects/GameClearAction")]
public class GameClearAction : ScriptableObject
{
    private Action<List<string>, List<int>> _onGameEnd;

    public void RegisterListener(Action<List<string>, List<int>> listener)
    {
        _onGameEnd += listener;
    }

    public void UnregisterListener(Action<List<string>, List<int>> listener)
    {
        _onGameEnd -= listener;
    }

    public void NotifyListeners(List<string> list, List<int> scores)
    {
        _onGameEnd?.Invoke(list, scores);
        foreach (var s in list)
        {
            Debug.Log(s);
        }
    }
}
