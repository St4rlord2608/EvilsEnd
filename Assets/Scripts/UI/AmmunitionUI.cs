using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AmmunitionUI : MonoBehaviour
{
    public static AmmunitionUI Instance;

    [SerializeField] private TextMeshProUGUI ammunitionText;

    private string ammunitionInCurrentMagazine = "";
    private string ammunitionInInventory = "";

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        ammunitionText.text = ammunitionInCurrentMagazine + " / " + 
            ammunitionInInventory;
    }

    public void SetAmmunitionInCurrentMagazine(float ammunition)
    {
        ammunitionInCurrentMagazine = ammunition.ToString();
    }

    public void SetAmmunitionInInventory(float ammunition)
    {
        ammunitionInInventory = ammunition.ToString();
    }
}
