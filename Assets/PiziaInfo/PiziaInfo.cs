using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PiziaInfo : MonoBehaviour
{
    [SerializeField] private Text fpsText, versionText, resolutionText;

    private void Awake()
    {
        versionText.text = "v. " + Application.version;
        resolutionText.text = Screen.currentResolution.width + "x" + Screen.currentResolution.height;
        StartCoroutine(UpdateFpsCounter());
    }

    private IEnumerator UpdateFpsCounter()
    {
        while (true)
        {            
            float fps = 1 / Time.unscaledDeltaTime;
            fpsText.text = "FPS: " + fps.ToString("F0");

#if UNITY_EDITOR
            resolutionText.text = Screen.currentResolution.width + "x" + Screen.currentResolution.height;
#endif

            yield return new WaitForSecondsRealtime(0.5f);
        }
    }
}