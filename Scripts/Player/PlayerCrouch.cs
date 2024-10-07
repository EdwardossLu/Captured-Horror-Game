using Sirenix.OdinInspector;
using UnityEngine;

public class PlayerCrouch : MonoBehaviour
{
    [Required, SerializeField] private Transform head;
    [SerializeField] private float transitionSpeed = 5.0f;
    
    [Header("Crouch")]
    [SerializeField] private float crouchHeight = 1.0f;
    [SerializeField] private float crouchCenterY = 0.5f;
    [SerializeField] private float crouchCameraY = 0.5f;

    [Header("Crouch Detection")]
    [SerializeField] private float headOffsetCheck = 1f;
    [SerializeField] private float checkSize = 1f;
    [SerializeField] private LayerMask ignoreLayer = 0;
    
    // Default Standing
    private float standHeight = 2.0f;
    private float standCameraY = 1.0f;
    private const float STAND_CENTER_Y = 1.0f;
    
    // Current
    private float currentHeight = 0f;
    private float currentCenterY = 0f;
    private float currentCameraY = 0f;
    
    private bool isCrouching = false;

    private CharacterController characterController;

    public bool IsCrouching => isCrouching;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();

        standCameraY = characterController.height;
        standHeight = head.localPosition.y;
    }

    private void Start()
    {
        currentHeight = standHeight;
        currentCenterY = STAND_CENTER_Y;
        currentCameraY = standCameraY;
    }

    private void Update()
    {
        CanStand();
        
        HandleCrouch();
        UpdateCharacterController();
        UpdateCameraPosition();
    }

    public void Use()
    {
        if (!isCrouching)
        {
            isCrouching = true;
        }
        else if (isCrouching && CanStand())
        {
            isCrouching = false;
        }
    }

    private void HandleCrouch()
    {
        if (isCrouching)
        {
            currentHeight = Mathf.Lerp(characterController.height, crouchHeight, Time.deltaTime * transitionSpeed);
            currentCenterY = Mathf.Lerp(characterController.center.y, crouchCenterY, Time.deltaTime * transitionSpeed);
            currentCameraY = Mathf.Lerp(head.localPosition.y, crouchCameraY, Time.deltaTime * transitionSpeed);
        }
        else
        {
            currentHeight = Mathf.Lerp(characterController.height, standHeight, Time.deltaTime * transitionSpeed);
            currentCenterY = Mathf.Lerp(characterController.center.y, STAND_CENTER_Y, Time.deltaTime * transitionSpeed);
            currentCameraY = Mathf.Lerp(head.localPosition.y, standCameraY, Time.deltaTime * transitionSpeed);
        }
    }

    private void UpdateCharacterController()
    {
        characterController.height = currentHeight;
        characterController.center = new Vector3(characterController.center.x, currentCenterY, characterController.center.z);
    }

    private void UpdateCameraPosition()
    {
        head.localPosition = new Vector3(head.localPosition.x, currentCameraY, head.transform.localPosition.z);
    }
    
    private bool CanStand()
    {
        // Add an offset to the head
        Vector3 check = head.position + (Vector3.up * headOffsetCheck);
        return !Physics.CheckSphere(check, checkSize);
    }
}
