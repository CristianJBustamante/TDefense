using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System;

public class PiziaDebugger : MonoBehaviour
{
    public static PiziaDebugger instance;

    private Canvas canvas;
    private CanvasScaler canvasScaler;
    public RectTransform container;
    public GameObject[] windows;
    public Image[] windowsButtons;

    public Pizia.Console console;
    public Pizia.Actions action;
    public Pizia.ErrorNotification errorNotification;
    public Pizia.DebuggingVariables debuggingVariables;
    
    [HideInInspector]
    public int currentWindow = 0;

    [HideInInspector]
    public bool showing = false;

    private int swipeState = 0; //0 = idle, 1 = dragging 2 = completed
    private float startSwipeTime = 0;
    private Vector2 input1StartPos;
    private Vector2 input2StartPos;

    private const int SWIPE_LENGTH_PERCENTAGE = 15;
    private const float MAX_TIME_SWIPE = 0.35f;

    void Awake()
    {
        if (instance == null) 
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            console.Initialize();
            canvas = GetComponent<Canvas>();
            canvasScaler = GetComponent <CanvasScaler>(); 
            SetActiveWindow(1);
            SceneManager.sceneLoaded += OnSceneChanged;

            EventSystem eventSystem = FindObjectOfType<EventSystem>();
            if (eventSystem == null) {
                GameObject _eventSystem = new GameObject("Event System");
                _eventSystem.AddComponent<EventSystem>();
                _eventSystem.AddComponent<StandaloneInputModule>();
            }
        } 
        else 
        {
            Destroy(gameObject);
        }
    }

    void Start () 
    {
        container.anchoredPosition3D = new Vector3(0,container.rect.height);
        canvas.enabled = true;
        debuggingVariables.LoadData();
    }

    void OnSceneChanged(Scene scene, LoadSceneMode mode)
    {
        Start();
    }

    void Update ()
    {
        if (Input.touches.Length == 2)
        {
            if (swipeState == 0) 
            {
                input1StartPos = Input.touches[0].position;
                input2StartPos = Input.touches[1].position;
                swipeState = 1;
                startSwipeTime = Time.time;
            }

            if (swipeState == 1)
            {
                float input1Offset = input1StartPos.y - Input.touches[0].position.y;
                float input2Offset = input2StartPos.y - Input.touches[1].position.y;

                float length1Percentage = input1Offset * 100 /Screen.height;
                float length2Percentage = input2Offset * 100 /Screen.height;

                if (length1Percentage > SWIPE_LENGTH_PERCENTAGE && length2Percentage > SWIPE_LENGTH_PERCENTAGE && !showing) 
                {
                    if (Time.time - startSwipeTime < MAX_TIME_SWIPE) {
                        swipeState = 2;
                        ShowDebugger();
                    }
                } 
                else if (length1Percentage < -SWIPE_LENGTH_PERCENTAGE && length2Percentage < -SWIPE_LENGTH_PERCENTAGE && showing)
                {
                    if (Time.time - startSwipeTime < MAX_TIME_SWIPE) {
                        swipeState = 2;
                        HideDebugger();
                    }
                }
            }
        } 
        else 
        {
            swipeState = 0;
        }
    }

    public void RefreshDebuggableVariables ()
    {
        debuggingVariables.LoadData();
    }

    public void ShowDebugger ()
    {
        showing = true;
        if (Screen.orientation == ScreenOrientation.Portrait || Screen.orientation == ScreenOrientation.PortraitUpsideDown) {
            canvasScaler.referenceResolution = new Vector2(800,600);
        } else {
            canvasScaler.referenceResolution = new Vector2(2000,600);
        }
        LeanTween.move(container,Vector3.zero,0.3f);
        SetActiveWindow(currentWindow);
    }

    public void HideDebugger ()
    {
        showing = false;
        LeanTween.move(container,new Vector3(0,container.rect.height),0.3f);
    }
    
    public void SetActiveWindow (int windowIndex)
    {
        for (int i=0; i<windows.Length; i++) 
        {
            windows[i].SetActive(false);
            windowsButtons[i].color = new Color(0.5f,0.5f,0.5f,1);
            if (i == windowIndex) 
            {
                windows[i].SetActive(true);
                windowsButtons[i].color = new Color(1,0.7f,0.35f,1);
            }
        }
        currentWindow = windowIndex;

        if (currentWindow == 0) {
            errorNotification.HideError();
        }
    }
}

[AttributeUsage(AttributeTargets.Field)]
public class ConsoleField : Attribute {}

[AttributeUsage(AttributeTargets.Method)]
public class ConsoleFunction : Attribute {}

[AttributeUsage(AttributeTargets.Field)]
public class DebugField : Attribute {}