using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public float health;
    public float[] position;

    //public PlayerData(PlayerHealth playerHealth) 
    //{ 
    //   health = playerHealth.GetCurrentHealth();
    //   position = new float[3];
    //    position[0] = playerHealth.transform.position.x;
    //    position[1] = playerHealth.transform.position.y;
    //    position[2] = playerHealth.transform.position.z;
    //}

}
