using System;
using UnityEngine;

public class FOVSensor : MonoBehaviour
{
    [SerializeField] private float viewRadius = 5f;
    [Range(0, 360)]
    [SerializeField] private float viewAngle = 90f;
    [SerializeField] private string targetTag = "Player";           // Tag to detect players
    [SerializeField] private LayerMask obstacleMask;
    [SerializeField] private int numRays = 100;                     // Number of rays to cast for the FOV
    [SerializeField] private Transform headTransform;               // Reference to the AI's head transform
    [SerializeField] private int heightChecks = 3;                  // Number of height checks

    [Header("Debug")]
    [SerializeField] private Color detected = Color.red;
    [SerializeField] private Color idle = Color.blue;
    [SerializeField] private Color obstructed = new Color(209, 113, 17);
    
    public bool PlayerDetected { get; private set; } = false;
    public bool PlayerDetectedBehindObstacle { get; private set; } = false;
    

    private void Update()
    {
        DetectPlayer();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = PlayerDetectedBehindObstacle ? obstructed : (PlayerDetected ? detected : idle);
        DrawFOV(transform.position); 
    }

    /// <summary>
    /// Draw gizmo at the base (floor) position
    /// </summary>
    /// <param name="origin"></param>
    private void DrawFOV(Vector3 origin)
    {
        Vector3 startAngle = DirFromAngle(-viewAngle / 2, false, origin);
        Vector3 endAngle = DirFromAngle(viewAngle / 2, false, origin);

        Gizmos.DrawLine(origin, origin + startAngle * viewRadius);
        Gizmos.DrawLine(origin, origin + endAngle * viewRadius);

        float angleStep = viewAngle / numRays;

        // Fan the lines in a cone shape
        for (int i = 0; i <= numRays; i++)
        {
            float angle = -viewAngle / 2 + angleStep * i;
            Vector3 dir = DirFromAngle(angle, false, origin);
            RaycastHit hit;

            if (Physics.Raycast(origin, dir, out hit, viewRadius, obstacleMask))
            {
                Gizmos.DrawLine(origin, hit.point);
            }
            else
            {
                Gizmos.DrawLine(origin, origin + dir * viewRadius);
            }
        }
    }

    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal, Vector3 origin)
    {
        if (!angleIsGlobal)
            angleInDegrees += headTransform.eulerAngles.y;

        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }

    /// <summary>
    /// Detect when the player is within viewing range of the AI
    /// </summary>
    private void DetectPlayer()
    {
        bool playerInFOV = false;
        bool playerInFOVBehindObstacle = false;

        Collider[] targetsInViewRadius = Physics.OverlapSphere(headTransform.position, viewRadius);

        foreach (Collider target in targetsInViewRadius)
        {
            if (target.CompareTag(targetTag))
            {
                Transform targetTransform = target.transform;
                CharacterController controller = target.GetComponent<CharacterController>();
                Vector3 dirToTarget = (targetTransform.position - headTransform.position).normalized;

                // Check from the AI head height
                if (Vector3.Angle(headTransform.forward, dirToTarget) < viewAngle / 2)
                {
                    float dstToTarget = Vector3.Distance(headTransform.position, targetTransform.position);
                    bool canSeePlayer = false;
                    bool behindObstacle = false;

                    for (int i = 0; i <= heightChecks; i++)
                    {
                        float heightFactor = i / (float)heightChecks;
                        Vector3 checkOrigin = headTransform.position + Vector3.up * heightFactor * headTransform.localScale.y;
                        Vector3 checkTarget = targetTransform.position + Vector3.up * heightFactor * (controller ? controller.height : targetTransform.localScale.y);

                        if (!Physics.Raycast(checkOrigin, (checkTarget - checkOrigin).normalized, dstToTarget, obstacleMask))
                        {
                            canSeePlayer = true;
                            break;
                        }
                        else
                        {
                            behindObstacle = true;
                        }
                    }

                    if (canSeePlayer)
                    {
                        playerInFOV = true;

                        if (behindObstacle)
                        {
                            playerInFOVBehindObstacle = true;
                        }

                        if (!PlayerDetected)
                        {
                            Debug.Log("Player detected:");
                            PlayerDetected = true;
                        }
                    }
                }
            }
        }

        PlayerDetectedBehindObstacle = playerInFOVBehindObstacle;

        if (playerInFOV || !PlayerDetected) return;

        PlayerDetected = false;
        Debug.Log("Player lost");
    }
}
