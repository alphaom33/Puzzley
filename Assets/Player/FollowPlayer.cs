using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        transform.position = GameManager.GetInstance().currentPlayer.transform.position + new Vector3(0, 0, -5); 
    }
}
