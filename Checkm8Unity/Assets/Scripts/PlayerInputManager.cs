using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputManager : MonoBehaviour
{
    
    public bool lockedAbilities;
    public bool lockedMovement;

    public bool lastMovedRight;

    private Vector2 _moveValue;
    [HideInInspector]
    public Vector2 MoveValue
    {
        get 
        {
            if (lockedMovement)
            {
                return Vector2.zero;
            }
            return _moveValue; 
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


        // bischop

    }

    private void Move(InputAction.CallbackContext input)
    {
        _moveValue = input.ReadValue<Vector2>();
        if (_moveValue.x > 0 && !lastMovedRight)
        {
            lastMovedRight = true;
        }
        else if (_moveValue.x < 0 && lastMovedRight)
        {
            lastMovedRight = false;
        }
    }
    void ShowJumpIndicator(InputAction.CallbackContext input)
    {
        if (lockedAbilities)
        {
            return;
        }
        GameManager.s_player.GetComponent<PlayerBase>().Horse();
        Debug.Log("showing jump indicator");
    }

    void Jump(InputAction.CallbackContext input)
    {
        if (lockedAbilities)
        {
            return;
        }
        StartCoroutine(GameManager.s_player.GetComponent<PlayerHorse>().Jump());
    }
}
