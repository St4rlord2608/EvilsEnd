using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTrail : MonoBehaviour
{
    [SerializeField] private float travelSpeed;

    private Vector3 target = Vector3.zero;

    private void Update()
    {
        if(target != Vector3.zero)
        {
            transform.position = Vector3.Lerp(transform.position, target, Time.deltaTime * travelSpeed);
        }
    }

    public void SetTarget(Vector3 target)
    {
        this.target = target;
    }
}
