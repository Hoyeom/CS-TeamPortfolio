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
            toggle.onValueChanged.AddListener(delegate(bool b) { Select(b, j); });
        }
    }

    public void Select(bool value,int id)
    {
        if (value)
        {
            Managers.Game.CharacterId = id;
            Debug.Log(id);
        }
    }
}