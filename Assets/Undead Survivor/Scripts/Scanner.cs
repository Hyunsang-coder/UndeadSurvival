using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scanner : MonoBehaviour
{
    [SerializeField] float scanRange;
    [SerializeField] LayerMask targetLayer;
    [SerializeField] RaycastHit2D[] targets;
    public Transform nearesttarget;

    private void FixedUpdate()
    {
        targets = Physics2D.CircleCastAll(transform.position, scanRange, Vector2.zero, 0, targetLayer);
        nearesttarget = GetNearest();
    }

    Transform GetNearest()
    {
        Transform nearest = null;

        float dist = 100;
        foreach (RaycastHit2D target in targets)
        {
            Vector3 myPos = transform.position;
            Vector3 targetPos= target.transform.position;

            float currentDist = Vector3.Distance(myPos, targetPos);

            if (currentDist < dist) {
                dist = currentDist;
                nearest = target.transform;
            }

        }

        return nearest;
    }

}
