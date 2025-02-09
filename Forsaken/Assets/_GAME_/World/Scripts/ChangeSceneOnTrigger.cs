using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSceneOnTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collider)
    {
        GameManager.instance.ChangeScene("Level_1");
    }
}
