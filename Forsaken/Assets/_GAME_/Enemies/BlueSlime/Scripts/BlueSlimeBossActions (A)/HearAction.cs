using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HearAction (A)", menuName = "ScriptableObjects/Actions/HearAction")]

public class HearAction : Action
{
    public float radius = 10f;

    public override bool Check(GameObject owner)
    {
        //Collider[] hits = Physics.OverlapSphere(owner.transform.position, radius);
        Collider2D[] hits = Physics2D.OverlapCircleAll(owner.transform.position, radius);

        foreach (Collider2D hit in hits)
        {
            if(hit.GetComponent<Player_Controller>())
            {
                // estoy escuchando al jugador
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
