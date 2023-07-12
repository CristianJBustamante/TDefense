using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Pizia
{
    public class TraceLog : MonoBehaviour
    {
        public Text text;
        public ScrollRect scrollRect;
        public GameObject main;
        public CanvasGroup background;
        public void ShowTraceLog (string trace)
        {
            gameObject.SetActive(true);
            if (trace == "" || trace == string.Empty) {
                trace = "Can't reach stack trace, please enable development build under Build Settings. (File > Build Settings)";
            }
            text.text = trace;
            LeanTween.alphaCanvas(background,0.6f,0.2f);
            LeanTween.scale(main,Vector3.one,0.2f).setEaseOutBack().setOnComplete(()=>{
                scrollRect.normalizedPosition = new Vector2(0,1);
            });
        }

        public void HideTraceLog ()
        {
            LeanTween.alphaCanvas(background,0,0.08f);
            LeanTween.scale(main,Vector3.zero,0.08f).setOnComplete(()=>{
                gameObject.SetActive(false);
            });
        }
    }
}
