using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_VoulmeChanger : MonoBehaviour
{
    [SerializeField] private Define.Sound type;
    [SerializeField] private Slider _slider;
    
    private void Start()
    {
        _slider = transform.GetComponentInChildren<Slider>();
        _slider.value = Managers.Sound.GetVolume(type);
    }

    public void ChangeVolume(float volume)
    {
        Managers.Sound.SetAudioVolume(type, volume);
    }
}
