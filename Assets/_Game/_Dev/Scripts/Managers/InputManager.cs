using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.Pizia.Tools;
using com.Pizia.Saver;
using com.Pizia.TouchJoypadSystem;

public class InputManager : CachedReferences
{
    public bool active = true;
    public Character character;
    Vector2 joyDirL, joyDirR;
    Vector2 joyDirPrevL, joyDirPrevR;

    //====================================================================================

    private void OnEnable()
    {
        TouchJoypadManager.Instance.GetJoypad(0).joypadEvent += JoyMoveL;
    }
    private void OnDisable()
    {
        if (TouchJoypadManager.Instance == null) return;
        TouchJoypadManager.Instance.GetJoypad(0).joypadEvent -= JoyMoveL;
    }

    //====================================================================================

    void JoyMoveL(TouchJoypad _joy, Vector2 _dir)
    {
        joyDirL = active ? _dir : Vector2.zero;

        if (joyDirL.magnitude > Mathf.Epsilon)
        {
            joyDirPrevL = joyDirL;
        }
    }

    //====================================================================================

    public Vector2 GetJoyL()
    {
        return joyDirL;
    }

    public void Toggle(bool _on)
    {
        if (_on) TouchJoypadManager.Instance.Enable();
        else TouchJoypadManager.Instance.Disable();
        active = _on;
        joyDirL *= 0;
    }
}
