using UnityEngine;

public class InputController : MonoBehaviour
{
    private PlayerInputActions playerInputActions;

    public Vector2 Move { get; private set; }
    public bool EscapePressed { get; private set; }
    public bool ScreenshotPressed { get; private set; }
    public bool MainWeaponPressed { get; private set; }
    public bool LeftSideInnerWeaponPressed { get; private set; }
    public bool RightSideInnerWeaponPressed { get; private set; }
    public bool LeftSideOuterWeaponPressed { get; private set; }
    public bool RightSideOuterWeaponPressed { get; private set; }
    public bool MainWeaponHold { get; private set; }
    public bool LeftSideInnerWeaponHold { get; private set; }
    public bool RightSideInnerWeaponHold { get; private set; }
    public bool LeftSideOuterWeaponHold { get; private set; }
    public bool RightSideOuterWeaponHold { get; private set; }
    public bool reloadHold { get; private set; }
    public bool healPressed { get; private set; }
    public bool dodgePressed { get; private set; }

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

    // Process inputs for this frame
    private void Update()
    {
        Move = PlayerInputActions.Gameplay.Move.ReadValue<Vector2>();
        ScreenshotPressed = PlayerInputActions.Gameplay.Screenshot.WasPressedThisFrame();
        EscapePressed = PlayerInputActions.Gameplay.Escape.WasPressedThisFrame();

        MainWeaponPressed = PlayerInputActions.Gameplay.FireMainWeapon.WasPressedThisFrame();
        LeftSideInnerWeaponPressed = PlayerInputActions.Gameplay.FireLeftSideInnerWeapon.WasPressedThisFrame();
        RightSideInnerWeaponPressed = PlayerInputActions.Gameplay.FireRightSideInnerWeapon.WasPressedThisFrame();
        LeftSideOuterWeaponPressed = PlayerInputActions.Gameplay.FireLeftSideOuterWeapon.WasPressedThisFrame();
        RightSideOuterWeaponPressed = PlayerInputActions.Gameplay.FireRightSideOuterWeapon.WasPressedThisFrame();

        MainWeaponHold = PlayerInputActions.Gameplay.FireMainWeapon.IsPressed();
        LeftSideInnerWeaponHold = PlayerInputActions.Gameplay.FireLeftSideInnerWeapon.IsPressed();
        RightSideInnerWeaponHold = PlayerInputActions.Gameplay.FireRightSideInnerWeapon.IsPressed();
        LeftSideOuterWeaponHold = PlayerInputActions.Gameplay.FireLeftSideOuterWeapon.IsPressed();
        RightSideOuterWeaponHold = PlayerInputActions.Gameplay.FireRightSideOuterWeapon.IsPressed();

        reloadHold = PlayerInputActions.Gameplay.Reload.IsPressed();
        healPressed = PlayerInputActions.Gameplay.Heal.WasPressedThisFrame();
        dodgePressed = PlayerInputActions.Gameplay.Dodge.WasPressedThisFrame();
    }
}
