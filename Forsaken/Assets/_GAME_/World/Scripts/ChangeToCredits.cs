using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeToCredits : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collider)
    {

        GameManager.instance.ChangeScene("Credits");
    }
}
