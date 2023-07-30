using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndOfDemoUI : MonoBehaviour
{
    [SerializeField] private Button confirmButton;

    private void Awake()
    {
        confirmButton.onClick.AddListener(() =>
        {
            gameObject.SetActive(false);
        });
    }
}
