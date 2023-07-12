using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Pizia
{
    public class SwitchLogVisibility : MonoBehaviour
    {
        public Console console;
        CanvasGroup canvasGroup;
        bool active = true;
        private Color disableColor = new Color(0.2f,0.2f,0.2f);
        private Color activeColor = new Color(0.27f,0.27f,0.27f);

        public void Switch (int index)
        {
            if (canvasGroup == null) 
            {
                canvasGroup = GetComponent<CanvasGroup>();
            }
            
            active = !active;

            if (active) 
            {
                canvasGroup.alpha = 1;
            }
            else
            {
                canvasGroup.alpha = 0.12f;
            }

            if (index == 0)
            {
                console.showingLogs = active;
            }
            else if (index == 1)
            {
                console.showingWarnings = active;
            }
            else if (index == 2)
            {
                console.showingErrors = active;
            }

            console.SwitchElementsVisibility(index,active);
        }
    }
}