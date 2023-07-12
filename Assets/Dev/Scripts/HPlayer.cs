using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.Pizia.Tools;
using com.Pizia.Saver;

public class HPlayer : CachedReferences
{
    public static HPlayer instance;
    public SimpleMovement simpleMovement;

    void Awake(){
        if(instance == null) instance = this;
        else Destroy(this);
    }
}
