
using UnityEngine; 
using VRInputs;
using static Constants.Constants;

public class ControllerInputs : MonoBehaviour
{
    // Reference to the Input Action Asset
    private VRInputActions controls; 
        
    private void Awake()
    {
        // Initialize the input action asset
        controls = new VRInputActions();

        // Assign callbacks for Left Hand
        
        controls.LeftHandControls.LeftGrip.performed += ctx => OnLeftGripPressed(ctx.ReadValue<float>());
        controls.LeftHandControls.LeftGrip.canceled += ctx => OnLeftGripReleased(ctx.ReadValue<float>());

        controls.LeftHandControls.LeftTrigger.performed += ctx => OnLeftTriggerPressed();
        controls.LeftHandControls.LeftTrigger.canceled += ctx => OnLeftTriggerReleased();
/*
        controls.LeftHandControls.LeftPrimaryButton.performed += ctx => OnLeftPrimaryButtonPressed();
        controls.LeftHandControls.LeftPrimaryButton.canceled += ctx => OnLeftPrimaryButtonReleased();*/

        // Assign callbacks for Right Hand
        controls.RightHandControls.RightGrip.performed += ctx => OnRightGripPressed(ctx.ReadValue<float>());
        controls.RightHandControls.RightGrip.canceled += ctx => OnRightGripReleased(ctx.ReadValue<float>());

        controls.RightHandControls.RightTrigger.performed += ctx => OnRightTriggerPressed();
        controls.RightHandControls.RightTrigger.canceled += ctx => OnRightTriggerReleased();
/*
        controls.RightHandControls.RightPrimaryButton.performed += ctx => OnRightPrimaryButtonPressed();
        controls.RightHandControls.RightPrimaryButton.canceled += ctx => OnRightPrimaryButtonReleased();*/
    }

    private void OnEnable()
    {
        // Enable the action maps
        controls.LeftHandControls.Enable();
        controls.RightHandControls.Enable();
    }

    private void OnDisable()
    {
        // Disable the action maps
        controls.LeftHandControls.Disable();
        controls.RightHandControls.Disable();
    }

    // Left Hand Callbacks
    private void OnLeftGripPressed(float value) => InputEvents.GripInput(LR_Device.L_Device, ControlType.Grip, ControlState.Pressed, value);
    private void OnLeftGripReleased(float value) => InputEvents.GripInput(LR_Device.L_Device, ControlType.Grip, ControlState.NotPressed, value);

    private void OnLeftTriggerPressed() => InputEvents.TriggerInput(LR_Device.L_Device, ControlType.Trigger, ControlState.Pressed);
    private void OnLeftTriggerReleased() => InputEvents.TriggerInput(LR_Device.L_Device, ControlType.Trigger, ControlState.NotPressed);

   /* private void OnLeftPrimaryButtonPressed() => Debug.Log("Left Primary Button Pressed");
    private void OnLeftPrimaryButtonReleased() => Debug.Log("Left Primary Button Released"); */

    // Right Hand Callbacks
    private void OnRightGripPressed(float value) => InputEvents.GripInput(LR_Device.R_Device, ControlType.Grip, ControlState.Pressed, value);
    private void OnRightGripReleased(float value) => InputEvents.GripInput(LR_Device.R_Device, ControlType.Grip, ControlState.NotPressed, value); 

    private void OnRightTriggerPressed() => InputEvents.TriggerInput(LR_Device.R_Device, ControlType.Trigger, ControlState.Pressed);
    private void OnRightTriggerReleased() => InputEvents.TriggerInput(LR_Device.R_Device, ControlType.Trigger, ControlState.NotPressed);
/*
    private void OnRightPrimaryButtonPressed() => Debug.Log("Right Primary Button Pressed");
    private void OnRightPrimaryButtonReleased() => Debug.Log("Right Primary Button Released");*/
}
