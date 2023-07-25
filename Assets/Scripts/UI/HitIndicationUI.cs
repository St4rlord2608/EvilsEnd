using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HitIndicationUI : MonoBehaviour
{
    public static HitIndicationUI Instance;

    [SerializeField] private Image hitImage;
    [SerializeField] private Image killedImage;
    [SerializeField] private float showIndicatorTime = 1.0f;

    private bool indicatorShown;
    private float currentIndicatorTime;

    private void Awake()
    {
        Instance = this;
        HideAll();
    }

    private void Update()
    {
        if (indicatorShown)
        {
            currentIndicatorTime += Time.deltaTime;
        }
        else
        {
            currentIndicatorTime = 0;
        }
        if(currentIndicatorTime > showIndicatorTime)
        {
            HideAll();
        }
    }

    public void ShowHit()
    {
        killedImage.gameObject.SetActive(false);
        hitImage.gameObject.SetActive(true);
        indicatorShown = true;
        currentIndicatorTime = 0;
    }

    public void ShowKill()
    {
        hitImage.gameObject.SetActive(false);
        killedImage.gameObject.SetActive(true);
        indicatorShown = true;
        currentIndicatorTime = 0;
    }

    private void HideAll()
    {
        hitImage.gameObject.SetActive(false);
        killedImage.gameObject.SetActive(false);
        indicatorShown = false;
    }
}
