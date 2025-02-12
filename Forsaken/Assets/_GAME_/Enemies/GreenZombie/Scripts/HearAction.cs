using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.PackageManager;
using UnityEngine;

[CreateAssetMenu(fileName = "HearAction (A)", menuName = "ScriptableObjects/Actions/HearAction")]

public class HearAction : Action
{
    public float radius = 3f;


    public override bool Check(GameObject owner)
    {
     

        Collider2D[] hits = Physics2D.OverlapCircleAll(owner.transform.position, radius);

        foreach (Collider2D hit in hits)
        {
            if (hit.GetComponent<PlayerMovement>())
            {

                return true;
            }
        }

        return false; 


    }


    public override void DrawGizmos(GameObject owner)
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(owner.transform.position, radius);
    }


}
