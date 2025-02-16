using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HearActionGZ (A)", menuName = "ScriptableObjects/Actions/HearActionGZ")]
public class HearActionGreenZombie : Action
{
    public float radius = 10f;

    public override bool Check(GameObject owner)
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(owner.transform.position, radius);

        foreach (Collider2D hit in hits)
        {
            if (hit.GetComponent<Player_Controller>())
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
