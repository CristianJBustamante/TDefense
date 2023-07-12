using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
#if !UNITY_EDITOR
    void Awake()
    {
        SceneManager.LoadScene(1, LoadSceneMode.Additive);
    }
#endif
}
