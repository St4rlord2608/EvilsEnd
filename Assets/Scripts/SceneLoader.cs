using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneLoader
{
    public enum Scene
    {
        MainMenu,
        GameScene,
        LoadingScene,
        Demo,
        SafeHouse,
        CompleteMap
    }

    private static Scene targetScene;
    private static string targetSceneString;

    private static bool loadByString = false;


    public static void Load(Scene targetScene)
    {
        loadByString = false;
        SceneLoader.targetScene = targetScene;

        SceneManager.LoadScene(SceneLoader.Scene.LoadingScene.ToString());

    }

    public static void LoadFromString(string targetScene)
    {
        loadByString = true;
        SceneLoader.targetSceneString = targetScene;
        SceneManager.LoadScene(SceneLoader.Scene.LoadingScene.ToString());
    }

    public static void LoaderCallback()
    {
        if(loadByString)
        {
            SceneManager.LoadScene(targetSceneString);
        }
        else
        {
            SceneManager.LoadScene(targetScene.ToString());
        }
        
    }
}
