using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : MonoBehaviour
{
    [SerializeField] OnTriggerBase[] triggeredScripts;

    private void OnTriggerEnter(Collider collider)
    {
        if(collider.TryGetComponent<PlayerHealth>(out PlayerHealth playerHealth))
        {
            foreach(OnTriggerBase triggered in triggeredScripts)
            {
                triggered.Trigger();
            }
            Destroy(gameObject);
        }
    }

}
