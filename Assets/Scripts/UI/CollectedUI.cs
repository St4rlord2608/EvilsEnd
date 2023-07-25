using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CollectedUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private float maxTextShownTime = 3f;

    private string material;
    private int materialAmount;
    private float currentTextShownTime = 0f;

    private bool textIsShown = false;

    private void Awake()
    {
        gameObject.SetActive(false);
    }

    private void Update()
    {
        if (textIsShown)
        {
            currentTextShownTime += Time.deltaTime;
            if(currentTextShownTime >= maxTextShownTime)
            {
                gameObject.SetActive(false);
            }
        }
        else
        {
            currentTextShownTime = 0;
        }
    }
    public void SetMaterialAndAmount(string material, int materialAmount)
    {
        this.material = material;
        this.materialAmount = materialAmount;
        text.text = "+ " + materialAmount + " " + material;
        gameObject.SetActive(true);
        textIsShown = true;
    }
}
