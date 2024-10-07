using Sirenix.OdinInspector;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Required, SerializeField] private Transform head = null;
    [Required, SerializeField] private PlayerCrouch crouch = null;
    
    [Header("Movement")]
    [SerializeField] private float speed = 12f;
    [Range(1f, 5f), SerializeField] private float sprintMultiplier = 2f;
    [Range(0f, 1f), SerializeField] private float slowMultiplier = 0.5f;

    [Space] 
    [ReadOnly] public Vector2 input = Vector2.zero;
    [ReadOnly] public bool isSprinting = false;
    
    private float gravity = -9.81f;
    private bool isGrounded = false;
    private Vector3 velocity = Vector3.zero;
    
    private CharacterController controller = null;
    
    private void Awake()
    {
        gravity = Physics.gravity.y;
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        isGrounded = controller.isGrounded;
        
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        
        Vector3 move = head.right * input.x + head.forward * input.y;
        move.y = 0;
        
        if (move.sqrMagnitude > 1f) 
            move.Normalize();
        
        controller.Move(move * (Speed() * Time.deltaTime));

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    private float Speed()
    {
        float s = speed;
        
        if (crouch.IsCrouching)
        {
            return s * slowMultiplier;
        }
        
        if (isSprinting)
        {
            return s * sprintMultiplier;
        }
        
        return s;
    }
}
