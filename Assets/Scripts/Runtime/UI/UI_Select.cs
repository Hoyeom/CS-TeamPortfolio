using System;
using UnityEngine;
using UnityEngine.UI;


public class UI_Select : MonoBehaviour
{
    private Toggle[] _toggles;

    private void Start()
    {
        _toggles = GetComponentsInChildren<Toggle>();
        for (var i = 0; i < _toggles.Length; i++)
        {
            Toggle toggle = _toggles[i];
            int j = i;
            if (i == Managers.Game.CharacterID.Value)
                toggle.isOn = true;
            toggle.onValueChanged.AddListener(delegate(bool isOn) { Select(isOn, j); });
        }
    }

    public void Select(bool isOn,int id)
    {
        if (isOn) Managers.Game.CharacterID.Value = (uint) id;
    }
}