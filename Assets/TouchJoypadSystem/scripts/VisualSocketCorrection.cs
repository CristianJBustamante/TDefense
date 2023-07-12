using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace com.Pizia.TouchJoypadSystem
{
    public class VisualSocketCorrection : MonoBehaviour
    {
        public Transform socket;
        Transform myTf;
        void Awake()
        {
            myTf = transform;
        }
        void Update()
        {
            myTf.position = socket.position;
        }
    }
}
