using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    [Header("FOW Settings")]
    public float ViewRadius;
    [Range(0,360)]
    public float ViewAngle;

    [SerializeField] private Light spotLight;
    
    public float MeshResolution; // raycast per degree
    public MeshFilter ViewMeshFilter;

    public LayerMask targetMask;
    public LayerMask obstacleMask;

    // Proprietes for caching useful informations for Guard
    [HideInInspector]
    public bool PlayerInRange;
    [HideInInspector]
    public Vector3 PlayerPosition;

    private Mesh _viewMesh;

    private void Awake()
    {
        PlayerInRange = false;
        PlayerPosition = Vector3.zero;

        _viewMesh = new Mesh();
        _viewMesh.name = "View Mesh";
        ViewMeshFilter.mesh = _viewMesh;

        // Light settings
        spotLight.range = ViewRadius;
        spotLight.spotAngle = ViewAngle;
        spotLight.intensity = ViewRadius * 2.0f + 5.0f;
    }

    private void Start()
    {
        StartCoroutine(FindTargetsWithDelay(0.2f));
    }

    private void LateUpdate()
    {
        DrawFieldOfView();
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

    private void DrawFieldOfView()
    {
        int stepCount = Mathf.RoundToInt(ViewAngle * MeshResolution); // rays count
        float stepAngleSize = ViewAngle / stepCount;
        List<Vector3> viewPoints = new List<Vector3>();

        for(int i=0; i<=stepCount; ++i)
        {
            float angle = transform.eulerAngles.y - ViewAngle / 2 + stepAngleSize * i;
            ViewCastInfo newViewCast = ViewCast(angle);
            viewPoints.Add(newViewCast.Point);
        }

        int vertexCount = viewPoints.Count + 1;
        Vector3[] vertices = new Vector3[vertexCount];
        int[] triangles = new int[(vertexCount - 2) * 3];

        vertices[0] = Vector3.zero; // mesh is child of the GO - so the origin of the mesh's triangles is .zero and not transform.position
        for (int i = 0; i < vertexCount - 2; ++i)
        {
            vertices[i + 1] = transform.InverseTransformPoint(viewPoints[i]);

            if (i < vertexCount - 2)
            {
                triangles[i * 3] = 0;
                triangles[i * 3 + 1] = i + 1;
                triangles[i * 3 + 2] = i + 2;
            }
        }

        _viewMesh.Clear();
        _viewMesh.vertices = vertices;
        _viewMesh.triangles = triangles;
        _viewMesh.RecalculateNormals();

    }

    private ViewCastInfo ViewCast(float globalAngle)
    {
        Vector3 dir = DirFromAngle(globalAngle, true);
        RaycastHit hit;

        if(Physics.Raycast(transform.position, dir, out hit, ViewRadius, obstacleMask))
        {
            return new ViewCastInfo(true, hit.point, hit.distance, globalAngle);
        }
        else
        {
            return new ViewCastInfo(false, transform.position + dir * ViewRadius, ViewRadius, globalAngle);
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

    public struct ViewCastInfo
    {
        public bool Hit;
        public Vector3 Point;
        public float Dist;
        public float Angle;

        public ViewCastInfo(bool hit, Vector3 point, float dist, float angle)
        {
            Hit = hit;
            Point = point;
            Dist = dist;
            Angle = angle;
        }
    }

}
