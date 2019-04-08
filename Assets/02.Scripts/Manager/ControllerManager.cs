using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerManager : MonoBehaviour
{
    public static bool Pad_Press = false;
    public static bool Pad_Click = false;
    public static bool Pad_PressDown = false;
    public static bool Pad_PressUp = false;

    public static bool Grip_Press = false;
    public static bool Grip_Click = false;
    public static bool Grip_PressDown = false;
    public static bool Grip_PressUp = false;

    public static bool Trigger_Press = false;
    public static bool Trigger_Click = false;
    public static bool Trigger_PressDown = false;
    public static bool Trigger_PressUp = false;

    //패드
    public void PadPress()
    {
        Pad_Press = true;
        Pad_PressUp = false;
        Debug.Log("PadPress");
    }

    public void PadPressDown()
    {
        Pad_PressDown = true;
        Debug.Log("PadPressDown");
    }

    public void PadPressUp()
    {
        Pad_Press = false;
        Pad_PressUp = true;
        Debug.Log("PadPressUP");
    }
    
    //그립
    public void GripPress()
    {
        Grip_Press = true;
        Grip_PressUp = false;
        Debug.Log("GripPress");
    }

    public void GripPressUp()
    {
        Grip_Press = false;
        Grip_PressUp = true;
        Debug.Log("GripPressUP");
    }

    //트리거
    public void TriggerPressUp()
    {
        Trigger_Press = false;
        Trigger_PressUp = true;
        Debug.Log("TriggerPressUP");
    }
}
