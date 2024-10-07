using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField] private Transform head = null;
    [SerializeField] private float checkDistance = 1f;

    [Header("Drop Settings")] 
    [SerializeField] private float upTolerance = 0.5f;
    [SerializeField] private LayerMask dropLayer = 0;
    
    public void Use()
    {
        if (!Physics.Raycast(head.position, head.forward, out RaycastHit hit, checkDistance)) return;

        if (hit.collider.TryGetComponent(out IInteract interact))
        {
            interact.Use();
        }
    }

    public void Drop()
    {
        if (!Physics.Raycast(head.position, head.forward, out RaycastHit hit, checkDistance, dropLayer)) return;
        
        Vector3 normal = hit.normal;
        if (IsNormalFacingUp(normal))
        {
            InventoryManager.Instance.Remove(hit.point);
        }
    }
    
    private bool IsNormalFacingUp(Vector3 normal)
    {
        // Consider a normal to be facing upwards if the Y component is close to 1
        return Vector3.Dot(normal, Vector3.up) > 0.5f;
    }
}
