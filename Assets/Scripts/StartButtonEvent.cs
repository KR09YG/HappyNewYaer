using System;
using UnityEngine;

[CreateAssetMenu(fileName = "StartButtonEvent", menuName = "Scriptable Objects/StartButtonEvent")]
public class StartButtonEvent : ScriptableObject
{
    private Action _onStartButtonClick;

    public void RegisterListener(Action listener)
    {
        _onStartButtonClick += listener;
    }

    public void UnregisterListener(Action listener)
    {
        _onStartButtonClick -= listener;
    }

    public void NotifyListeners()
    {
        _onStartButtonClick?.Invoke();
    }
}
