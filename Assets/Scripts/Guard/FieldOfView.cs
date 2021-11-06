using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    [Header("FOW Settings")]
    public float ViewRadius;
    [Range(0,360)]
    public float ViewAngle;

    public LayerMask targetMask;
    public LayerMask obstacleMask;

    // Proprietes for caching useful informations for Guard
    [HideInInspector]
    public bool PlayerInRange;
    [HideInInspector]
    public Vector3 PlayerPosition;

    private void Awake()
    {
        PlayerInRange = false;
        PlayerPosition = Vector3.zero;
    }

    private void Start()
    {
        StartCoroutine(FindTargetsWithDelay(0.2f));
    }

    private IEnumerator FindTargetsWithDelay(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            FindVisibleTargets();
        }
    }

    private void FindVisibleTargets()
    {
        Collider[] targetInViewRadius = Physics.OverlapSphere(transform.position, ViewRadius, targetMask);

        for(int i=0; i< targetInViewRadius.Length; i++)
        {
            Transform target = targetInViewRadius[i].transform;
            Vector3 dirToTarget = (target.position - transform.position).normalized;
            if(Vector3.Angle(transform.forward, dirToTarget)< ViewAngle / 2)
            {
                // Target is in the view angle
                float dstToTarget = Vector3.Distance(transform.position, target.position);
                if(!Physics.Raycast(transform.position, dirToTarget, dstToTarget, obstacleMask))
                {
                    // No obstacle between this object (guard) and the target (player) - the target in in sight
                    PlayerInRange = true;
                    PlayerPosition = target.position;
                }
                else
                {
                    PlayerInRange = false;
                    PlayerPosition = Vector3.zero;
                }
            }
        }
    }

    // Direction of lines of the view angle of fow - angleIsGlobal = false -> angle rotate with character
    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
  
}
