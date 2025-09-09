using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class QuestHapticHandler : AbstractHapticHandler
{
    private InputDevice leftDevice;
    private InputDevice rightDevice;

    public override void Init()
    {
        // Get Left & Right XR devices
        var leftDevices = new List<InputDevice>();
        var rightDevices = new List<InputDevice>();

        InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.HeldInHand | InputDeviceCharacteristics.Left, leftDevices);
        InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.HeldInHand | InputDeviceCharacteristics.Right, rightDevices);

        if (leftDevices.Count > 0) leftDevice = leftDevices[0];
        if (rightDevices.Count > 0) rightDevice = rightDevices[0];

        if (!leftDevice.isValid || !rightDevice.isValid)
        {
            Debug.LogWarning("QuestHapticHandler: Could not find valid XR controller devices.");
        }
    }

    public override void Play(HapticLibrarySO.HapticData data)
    {
        if (data == null) return;

        if (data.bothHands)
        {
            SendHaptic(leftDevice, data);
            SendHaptic(rightDevice, data);
        }
        else
        {
            SendHaptic(rightDevice, data);
        }
    }

    private void SendHaptic(InputDevice device, HapticLibrarySO.HapticData data)
    {
        if (device.isValid && device.TryGetHapticCapabilities(out HapticCapabilities capabilities) && capabilities.supportsImpulse)
        {
            device.SendHapticImpulse(0, data.amplitude, data.duration);
        }
    }

    public override void StopAll()
    {
        if (leftDevice.isValid)
            leftDevice.StopHaptics();

        if (rightDevice.isValid)
            rightDevice.StopHaptics();
    }
}
