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
    }

    private static Scene targetScene;


    public static void Load(Scene targetScene)
    {
        SceneLoader.targetScene = targetScene;

        SceneManager.LoadScene(targetScene.ToString());

    }

    public static void LoadFromString(string targetScene)
    {
        SceneManager.LoadScene(targetScene);
    }

    public static void LoaderCallback()
    {
        SceneManager.LoadScene(targetScene.ToString());
    }
}
