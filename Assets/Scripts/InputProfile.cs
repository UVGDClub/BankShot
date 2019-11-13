using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InputProfile
{
    public string name;
    public bool gamepad;

    [Header("Keyboard and Mouse")]
    public KeyCode up;
    public KeyCode down;
    public KeyCode left;
    public KeyCode right;
    public KeyCode fire;

    [Header("Gamepad")]
    public string horizontalMovementAxis;
    public string verticalMovementAxis;
    public string horizontalAimAxis;
    public string verticalAimAxis;
}
