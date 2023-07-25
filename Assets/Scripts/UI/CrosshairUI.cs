using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrosshairUI : MonoBehaviour
{
    public static CrosshairUI Instance;

    [SerializeField] private Image aimsAtEnemyImage;

    private void Awake()
    {
        Instance = this;
        HideEnemyAim();
    }

    public void ShowEnemyAim()
    {
        aimsAtEnemyImage.gameObject.SetActive(true);
    }

    public void HideEnemyAim()
    {
        aimsAtEnemyImage.gameObject.SetActive(false);
    }
}
