using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.Pizia.Tools;
using com.Pizia.Saver;

public class LockRotation : CachedReferences
{
    public bool findCamera;
    Camera cam;
    Canvas canvas;
    Quaternion rot;

    void Awake()
    {
        rot = transform.rotation;
        if (findCamera)
        {
            cam = Camera.main;
            canvas = GetComponent<Canvas>();
            canvas.worldCamera = cam;
        }
    }

    void Update()
    {
        transform.rotation = rot;
    }
}
