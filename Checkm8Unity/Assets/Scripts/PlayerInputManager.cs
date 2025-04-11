using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputManager : MonoBehaviour
{
    
    private Controller _controller;
    private GameManager _gameManager;

    public bool LockedAbilities;
    public bool LockedMovement;

    public bool LastMovedRight;

    private Vector2 _moveValue;
    [HideInInspector]
    public Vector2 MoveValue
    {
        get
        {
            if (LockedMovement)
            {
                return Vector2.zero;
            }
            return _moveValue; 
        }
    }

    private void OnEnable()
    {
        _controller = new Controller();

        // movement
        _controller.Player.Move.Enable();
        _controller.Player.Move.performed += Move;
        _controller.Player.Move.canceled += Move; // so it goes to 0

        // horse jump
        _controller.Player.Horse.Enable();
        _controller.Player.Horse.started += ShowJumpIndicator; // show indicator
        _controller.Player.Horse.canceled += Jump; // show indicator


        // bischop
        _controller.Player.Bischop.Enable();
        _controller.Player.Bischop.performed += Bischop;

        // Rook
        _controller.Player.Rook.Enable();
        _controller.Player.Rook.performed += Rook;

        // Queen
        _controller.Player.Queen.Enable();
        _controller.Player.Queen.performed += Queen;
    }

    private void Start()
    {
        _gameManager = GetComponent<GameManager>();
    }

    private void OnDisable()
    {
        _controller.Player.Move.Disable();
        _controller.Player.Horse.Disable();
        _controller.Player.Bischop.Disable();
        _controller.Player.Rook.Disable();
        _controller.Player.Queen.Disable();
    }

    private void Move(InputAction.CallbackContext input)
    {
        _moveValue = input.ReadValue<Vector2>();
        if (_moveValue.x > 0 && !LastMovedRight)
        {
            LastMovedRight = true;
        }
        else if (_moveValue.x < 0 && LastMovedRight)
        {
            LastMovedRight = false;
        }
    }
    void ShowJumpIndicator(InputAction.CallbackContext input)
    {
        if (LockedAbilities)
        {
            return;
        }
        GameManager.s_Player.GetComponent<PlayerBase>().Horse();
    }

    void Jump(InputAction.CallbackContext input)
    {
        if (LockedAbilities)
        {
            return;
        }
        if (_gameManager.GameDone)
        {
            _gameManager.ResetScene();
            return;
        }
        if (GameManager.s_Player.TryGetComponent<PlayerHorse>(out PlayerHorse script))
        {
            StartCoroutine(script.Jump());
        }
    }

    void Bischop(InputAction.CallbackContext input)
    {
        if (LockedAbilities)
        {
            return;
        }
        GameManager.s_Player.GetComponent<PlayerBase>().Bischop();
    }

    void Rook(InputAction.CallbackContext input)
    {
        if (LockedAbilities)
        {
            return;
        }
        GameManager.s_Player.GetComponent<PlayerBase>().Rook();
    }

    void Queen(InputAction.CallbackContext input)
    {
        if (LockedAbilities)
        {
            return;
        }
        GameManager.s_Player.GetComponent<PlayerBase>().Queen();
    }
}
