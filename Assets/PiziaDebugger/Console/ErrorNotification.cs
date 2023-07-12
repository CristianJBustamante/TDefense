using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Pizia
{
    public class ErrorNotification : MonoBehaviour
    {
        public PiziaDebugger piziaDebugger;
        public RectTransform container;
        public Text text;

        private void Start () 
        {
            container.anchoredPosition3D = new Vector2(0,-80);
        }

        public void ShowError (string errorTxt)
        {
            bool showError = true;
            if (piziaDebugger.showing && piziaDebugger.currentWindow == 0) 
            {
                showError = false;
            }

            if (showError) 
            {
                text.text = errorTxt;
                LeanTween.move(container,new Vector2(0,0),0.12f);
            }
        }

        public void ClickError ()
        {
            HideError();
            piziaDebugger.ShowDebugger();
            piziaDebugger.SetActiveWindow(0);
        }

        public void HideError ()
        {
            LeanTween.move(container,new Vector2(0,-container.rect.height),0.12f);
        }
    }
}