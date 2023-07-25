using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponReloading : MonoBehaviour
{
    [SerializeField] private Transform currentEquippedMagazine;

    private PlayerReloading playerReloading;

    private void Awake()
    {
        playerReloading = transform.root.GetComponent<PlayerReloading>();
    }

    private void Start()
    {
        playerReloading.OnMagazineAttached += PlayerReloading_OnMagazineAttached;
        playerReloading.OnMagazineRemoved += PlayerReloading_OnMagazineRemoved;
    }

    private void PlayerReloading_OnMagazineRemoved(object sender, System.EventArgs e)
    {
        currentEquippedMagazine.gameObject.SetActive(false);
    }

    private void PlayerReloading_OnMagazineAttached(object sender, System.EventArgs e)
    {
        currentEquippedMagazine.gameObject.SetActive(true);
    }
}
