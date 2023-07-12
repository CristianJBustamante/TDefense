using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.Pizia.Tools;
using com.Pizia.Saver;
using Cinemachine;

public class GameManager : CachedReferences
{
    public static GameManager instance;

    public bool skipOnboarding;
    [Range(0, 10)] public float timeScale = 1;

    [Header("Components")]
    public Player playerCharacter;
    public InputManager playerInput;

    //=====================================================================================

    private void Awake()
    {
        instance = this;

        Application.targetFrameRate = 60;
    }

    private void Start()
    {

    }

#if UNITY_EDITOR
    private void Update()
    {
        Time.timeScale = timeScale;

        if (Input.GetKey(KeyCode.X))
        {
            Time.timeScale = 4f;
        }
    }
#endif

    //=====================================================================================

    public void TogglePlayerControl(bool _on)
    {
        playerInput.Toggle(_on);
    }
}

public static class GameTags
{
    public const string playerTag = "Player";
}