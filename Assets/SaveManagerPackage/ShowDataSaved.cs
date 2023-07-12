using UnityEngine;
using UnityEngine.SceneManagement;

namespace com.Pizia.Saver
{
    public class ShowDataSaved : MonoBehaviour
    {
#if UNITY_EDITOR        
        [SerializeField] bool clear;
#endif
        
        [SerializeField] bool enableInsantSave, timeSaving, saveOnSceneChange;
        [SerializeField] float timeToSave = 10;

        float saveTime;

        void Awake()
        {
            Application.quitting += SaveManager.SaveAll;
            if (saveOnSceneChange)
            {
                SceneManager.sceneLoaded += (_, __) => SaveManager.SaveAll();
                SceneManager.sceneUnloaded += _ => SaveManager.SaveAll();
            }

            SaveManager.instantSave = enableInsantSave;
            saveTime = Time.time + timeToSave;
        }

        void Update()
        {
            if (!timeSaving || Time.time < saveTime) return;
            SaveManager.SaveAll();
            saveTime = Time.time + timeToSave;
        }

        void OnValidate()
        {
#if UNITY_EDITOR
            if (!clear) return;

            Saver.Clear();
            clear = false;
#endif

            SaveManager.instantSave = enableInsantSave;
        }

        void OnApplicationQuit() => SaveManager.SaveAll();

#if !UNITY_EDITOR
        void OnApplicationFocus(bool focus) => SaveManager.SaveAll();
        void OnApplicationPause(bool pause) => SaveManager.SaveAll();
#endif
    }
}