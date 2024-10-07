using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Required, SerializeField] private PlayerInteract interact = null;
    [Required, SerializeField] private PlayerCrouch crouch = null;
    [Required, SerializeField] private PlayerMovement movement = null;
    [Required, SerializeField] private PlayerInventory inventory = null;

    [Space] 
    [Required, SerializeField] private InputActionReference move = null;
    [Required, SerializeField] private InputActionReference scroll = null;
    [Required, SerializeField] private InputActionReference numKey = null;

    private void OnEnable()
    {
        numKey.action.performed += OnNumberKeyPressed;
    }

    private void OnDisable()
    {
        numKey.action.performed -= OnNumberKeyPressed;
        movement.input = Vector2.zero;
    }

    private void OnMove()
    {
        movement.input = move.action.ReadValue<Vector2>();
    }

    private void OnInteract()
    {
        interact.Use();
    }

    private void OnDrop()
    {
        interact.Drop();
    }

    private void OnSprint()
    {
        movement.isSprinting = !movement.isSprinting;
    }

    private void OnCrouch()
    {
        crouch.Use();
    }

    private void OnMouseScroll()
    {
        int yScroll = (int)scroll.action.ReadValue<Vector2>().y;
        if (yScroll > 0f)
            inventory.Up();
        else if (yScroll < 0f)
            inventory.Down();
    }

    private void OnNumberKeyPressed(InputAction.CallbackContext context)
    {
        int numKeyValue;
        int.TryParse(context.control.name, out numKeyValue);
        inventory.Select(numKeyValue - 1);
    }
}
