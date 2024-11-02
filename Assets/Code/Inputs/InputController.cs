using UnityEngine;

public class InputController : MonoBehaviour
{
    private PlayerInputActions playerInputActions;

    public Vector2 Move { get; private set; }
    public bool EscapePressed { get; private set; }
    public bool ScreenshotPressed { get; private set; }
    public bool fire1Pressed { get; private set; }
    public bool fire2Pressed { get; private set; }
    public bool fire3Pressed { get; private set; }
    public bool fire4Pressed { get; private set; }
    public bool fire5Pressed { get; private set; }
    public bool fire1Hold { get; private set; }
    public bool fire2Hold { get; private set; }
    public bool fire3Hold { get; private set; }
    public bool fire4Hold { get; private set; }
    public bool fire5Hold { get; private set; }


    private PlayerInputActions PlayerInputActions
    {
        get
        {
            if (playerInputActions == null)
            {
                playerInputActions = new PlayerInputActions();
            }

            return playerInputActions;
        }
    }


    private void OnEnable()
    {
        PlayerInputActions.Enable();
    }


    private void OnDisable()
    {
        PlayerInputActions.Disable();
    }


    private void Update()
    {
        // Process inputs for this frame
        Move = PlayerInputActions.Gameplay.Move.ReadValue<Vector2>();
        ScreenshotPressed = PlayerInputActions.Gameplay.Screenshot.WasPressedThisFrame();
        EscapePressed = PlayerInputActions.Gameplay.Escape.WasPressedThisFrame();

        fire1Pressed = PlayerInputActions.Gameplay.Fire1.WasPressedThisFrame();
        fire2Pressed = PlayerInputActions.Gameplay.Fire2.WasPressedThisFrame();
        fire3Pressed = PlayerInputActions.Gameplay.Fire3.WasPressedThisFrame();
        fire4Pressed = PlayerInputActions.Gameplay.Fire4.WasPressedThisFrame();
        fire5Pressed = PlayerInputActions.Gameplay.Fire5.WasPressedThisFrame();

        fire1Hold = PlayerInputActions.Gameplay.Fire1.IsPressed();
        fire2Hold = PlayerInputActions.Gameplay.Fire2.IsPressed();
        fire3Hold = PlayerInputActions.Gameplay.Fire3.IsPressed();
        fire4Hold = PlayerInputActions.Gameplay.Fire4.IsPressed();
        fire5Hold = PlayerInputActions.Gameplay.Fire5.IsPressed();
    }
}
