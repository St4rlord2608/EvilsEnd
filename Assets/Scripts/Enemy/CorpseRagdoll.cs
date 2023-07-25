using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorpseRagdoll : MonoBehaviour
{
    [SerializeField] private Transform visualsParent;
    [SerializeField] private float maxLiveTime = 10f;

    private float currenLiveTime = 0f;

    private void Update()
    {
        currenLiveTime += Time.deltaTime;
        if(currenLiveTime > maxLiveTime)
        {
            Destroy(gameObject);
        }
    }

    public void ActivateVisual(int visualIndex)
    {
        Transform visual = visualsParent.GetChild(visualIndex);
        visual.gameObject.SetActive(true);
    }
}
