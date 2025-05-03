using System;
using static Constants.Constants;

public static class InputEvents
{

    /*public delegate void ActionButtonEvent(InputDevice inputDevice, InputFeatureUsage<bool> inputFeatureUsage, bool buttonValue, LR_Device lr_Device, ControlType controlType);*/
    public delegate void ActionTrigger(LR_Device lr_Device, ControlType controlType, ControlState controlState);
    public delegate void ActionGrip( LR_Device lr_Device, ControlType controlType, ControlState controlStat, float gripValue);
    /*public delegate void ActionTouchpadEvent(InputDevice inputDevice, InputFeatureUsage<Vector2> inputFeatureUsage, Vector2 vector2, LR_Device deviceType, ControlType controlType);*/

    /*public static event ActionButtonEvent ButtonInputs;*/ 
    public static event ActionTrigger TriggerActionInputs; 
    public static event ActionGrip GripActionInputs;
    /*    public static event ActionTouchpadEvent PrimaryTouchpadInput;*/

    public static event Action<LR_Device, float> SetJetpackInput;

    public static void TriggerInput(LR_Device lr_Device, ControlType controlType, ControlState controlState)
    {
        TriggerActionInputs?.Invoke(lr_Device, controlType, controlState);
    } 

    public static void GripInput(LR_Device lr_Device, ControlType controlType, ControlState controlState, float gripValue)
    {
        GripActionInputs?.Invoke(lr_Device, controlType, controlState, gripValue);
    }

    public static void RaiseSetJetpackInput(LR_Device device, float value)
    {
        SetJetpackInput?.Invoke(device, value);
    }

    /* public static void TriggerPrimaryTouchpadInput(InputDevice inputDevice, InputFeatureUsage<Vector2> inputFeatureUsage, Vector2 vectorValue, LR_Device lr_Device, ControlType controlType)
     {
         PrimaryTouchpadInput?.Invoke(inputDevice, inputFeatureUsage, vectorValue, lr_Device, controlType);
     }*/
}
   