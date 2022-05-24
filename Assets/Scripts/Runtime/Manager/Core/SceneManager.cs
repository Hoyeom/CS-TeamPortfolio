using System;
using Runtime.Scenes;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

public class SceneManagerEx
{
    public BaseScene CurrentScene =>
        Object.FindObjectOfType<BaseScene>();
        
    public void LoadScene(Define.Scene type)
    {
        Managers.Clear();
        Scene scene = SceneManager.GetSceneByName(GetSceneName(type));
        SceneManager.LoadSceneAsync(GetSceneName(type));
    }

    private string GetSceneName(Define.Scene type)
    {
        string name = Enum.GetName(typeof(Define.Scene), type);
        return name;
    }

    public void Clear()
    {
        CurrentScene.Clear();
    }
}