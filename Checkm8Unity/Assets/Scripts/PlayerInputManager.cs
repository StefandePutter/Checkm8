using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    
    private bool lockedAbilities;
    private bool lockedMovement;
    private bool lastMovedRight;

    private Vector2 moveValue;
    [HideInInspector]
    public Vector2 MoveValue
    {
        get 
        {
            return moveValue; 
        }
    }

    private void Start()
    {
        Controller controller = new Controller();

        // movement
        controller.Player.Move.Enable();
        controller.Player.Move.performed += Move;
        controller.Player.Move.canceled += Move; // so it goes to 0


        // horse jump
        controller.Player.Horse.Enable();
        controller.Player.Horse.started += ShowJumpIndicator; // show indicator
        controller.Player.Horse.canceled += Jump; // show indicator
        controller.Player.Horse.canceled += HideJumpIndicator; // hide indicator


        // bischop

    }

    private void Move(InputAction.CallbackContext input)
    {
        if (lockedMovement)
        {
            return;
        }
        moveValue = input.ReadValue<Vector2>();
        if (moveValue.x == 1)
        {
            Debug.Log("Right");
            lastMovedRight = true;
        } 
        else if (moveValue.x == -1)
        {
            Debug.Log("left");
            lastMovedRight = false;
        }
    }

    void Jump(InputAction.CallbackContext input)
    {
        if (lockedAbilities)
        {
            return;
        }
        Debug.Log("jumping " + (lastMovedRight ? "right" : "left") + " up");
    }

    void ShowJumpIndicator(InputAction.CallbackContext input)
    {
        if (lockedAbilities)
        {
            return;
        }
        Debug.Log("showing jump indicator");
    }
    void HideJumpIndicator(InputAction.CallbackContext input)
    {
        if (lockedAbilities)
        {
            return;
        }
        Debug.Log("hiding jump indicator");
    }
}
