using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadLevelUI : MonoBehaviour
{
    [SerializeField] private string levelName;
    [SerializeField] private Button loadLevelButton;

    private void Awake()
    {
        loadLevelButton.onClick.AddListener(() =>
        {
            SceneLoader.LoadFromString(levelName);
        });
    }

    private void Start()
    {
        if (LevelDB.CheckIfLevelIsInDictionary(levelName))
        {
            if (LevelDB.GetLevel(levelName).unlocked == true)
            {
                gameObject.SetActive(true);
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
        else
        {
            gameObject.SetActive(false) ;
        }
        
    }

    public string GetLevelName()
    {
        return levelName;
    }
}
