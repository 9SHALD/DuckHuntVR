using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

public class InputReader : MonoBehaviour {
    public delegate void Void(int index);
    public static event Void TriggerDownEvents;
    public static event Void TriggerUpEvents;
    public static event Void PadPressDownEvents;
    public static event Void PadPressUpEvents;
    private List<Vector3> previousPositions = new List<Vector3>();
    private int amountToRemember = 15;
    public float controllerSpeed;
    private Vector3 lastFramePosition;
    OVRInput.Controller m_controller;
    SteamVR_Controller.Device device;

    void Update() {
        if (trackedObj == null)
            trackedObj = transform.parent.GetComponent<SteamVR_TrackedObject>();
        device = SteamVR_Controller.Input((int)trackedObj.index);
        ReadTriggerInput();
        ReadPadPressInput();
        DetermineSpeed();
    }

    private void ReadTriggerInput() {
        if (device.GetTouchDown(SteamVR_Controller.ButtonMask.Trigger) && TriggerDownEvents != null)
            TriggerDownEvents((int)trackedObj.index);
        if (device.GetTouchUp(SteamVR_Controller.ButtonMask.Trigger) && TriggerUpEvents != null)
            TriggerUpEvents((int)trackedObj.index);
    }

    private void ReadPadPressInput() {
        if (device.GetPressDown(SteamVR_Controller.ButtonMask.Touchpad) && PadPressDownEvents != null)
            PadPressDownEvents((int)trackedObj.index);
        if (device.GetPressUp(SteamVR_Controller.ButtonMask.Touchpad) && PadPressUpEvents != null)
            PadPressUpEvents((int)trackedObj.index);
    }

    private void DetermineSpeed() {
        controllerSpeed = Mathf.Abs(Vector3.Distance(transform.position, lastFramePosition)) * Time.deltaTime;
        if (controllerSpeed < 0.00001f)
            controllerSpeed = 0;
        lastFramePosition = transform.position;
    }


    private int GetLocation() {
        bool up = device.GetAxis(Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad)[0] > 0;
        bool right = device.GetAxis(Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad)[1] > 0;
        int one = !up ? 0 : 1;
        int location = !right ? one + 2 : one;

        return location;
    }

    public float GetTriggerAxis() {
        if (device != null)
            return device.GetAxis(Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger).x;
        else
            return 0;
    }

    public void Vibrate(int time = 3999) {
        ushort ttime = (ushort)time;
        if (ttime > 3999) {
            ttime = 3999;
            StartCoroutine(VibrateExtender(time - 3999));
        }
        device.TriggerHapticPulse(ttime);
    }

    private IEnumerator VibrateExtender(int timeLeft) {
        yield return new WaitForSeconds(0.004f);
        Vibrate(timeLeft);
    }
}
