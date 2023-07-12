using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.Pizia.Tools;
using com.Pizia.Saver;

public class Utilities
{
    public static IEnumerator WaitAndDoStuff(float time, Action action)
    {
        yield return new WaitForSeconds(time);
        action.Invoke();
    }
    public static IEnumerator WaitForFramesAndDoStuff(int frames, Action action)
    {
        while (frames > 0)
        {
            yield return null;
            frames--;
        }
        action.Invoke();
    }
}
