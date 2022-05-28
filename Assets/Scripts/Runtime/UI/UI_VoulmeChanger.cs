using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_VoulmeChanger : MonoBehaviour
{
    [SerializeField] private Define.Sound type;

    public void ChangeVolume(float volume)
    {
        Managers.Sound.SetAudioVolume(type, volume);
    }
}
