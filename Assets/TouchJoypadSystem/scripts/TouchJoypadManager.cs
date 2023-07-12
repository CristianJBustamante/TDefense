using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace com.Pizia.TouchJoypadSystem
{
    [RequireComponent(typeof(Canvas))]
    [RequireComponent(typeof(CanvasScaler))]
    public partial class TouchJoypadManager : MonoBehaviour
    {
        public class TouchData
        {
            public bool touching = false;
            public Vector2 pointInitial;
            public Vector2 pointCurrent;
            public Vector2 Offset
            {
                get
                {
                    return ((pointCurrent - pointInitial) / Screen.width) * manager.CanvasScaler.referenceResolution.x;
                }
            }
            public Vector2 Direction
            {
                get
                {
                    return Vector2.ClampMagnitude((Offset / joypadSocketRadius) * 2f, 1f);
                }
            }

            [HideInInspector]
            public TouchJoypadManager manager;
            [HideInInspector]
            internal TouchJoypad joypad;
        }
        private static readonly int maxTouches = 5;
        private static readonly float joypadSocketRadius = 150f;
        private static TouchJoypadManager instance;
        public static TouchJoypadManager Instance
        {
            get
            {
                if (instance == null) instance = FindObjectOfType<TouchJoypadManager>();
                return instance;
            }
        }

        [HideInInspector]
        public TouchData[] touchData = new TouchData[maxTouches];
        [HideInInspector]
        public TouchData mouseData;
        [HideInInspector]
        public TouchData controllerData;
        [SerializeField]
        private TouchJoypad[] touchJoypads;
        [SerializeField]
        private TouchJoypadButton[] touchJoypadButtons;
        private Canvas canvas;

        public GameObject visualSocket;
        public Canvas Canvas
        {
            get
            {
                if (canvas == null) canvas = GetComponent<Canvas>();
                return canvas;
            }
        }
        private CanvasScaler canvasScaler;
        public CanvasScaler CanvasScaler
        {
            get
            {
                if (canvasScaler == null) canvasScaler = GetComponent<CanvasScaler>();
                return canvasScaler;
            }
        }

        public bool releaseOverUI;

        void Awake()
        {
            if (instance == null || instance == this)
            {
                DontDestroyOnLoad(gameObject);
                instance = this;
                Initialize();
                visualSocket.SetActive(true);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        void Initialize()
        {
            Input.simulateMouseWithTouches = false;
            for (int i = 0; i < touchData.Length; i++)
            {
                touchData[i] = new TouchData()
                {
                    touching = false,
                    pointInitial = Vector2.zero,
                    pointCurrent = Vector2.zero,
                    manager = this
                };
            }
            mouseData = new TouchData()
            {
                touching = false,
                pointInitial = Vector2.zero,
                pointCurrent = Vector2.zero,
                manager = this
            };
            controllerData = new TouchData()
            {
                touching = false,
                pointInitial = Vector2.zero,
                pointCurrent = Vector2.zero,
                manager = this
            };
            foreach (var joypad in touchJoypads)
            {
                joypad.Visible = joypad.visibleWhenInactive;
                joypad.initialPosition = joypad.socket.anchoredPosition;
                joypad.manager = this;
            }
        }
        void Update()
        {
            //UPDATE touchData positions
            for (int i = 0; i < Input.touchCount; i++)
            {
                var touch = Input.GetTouch(i);
                if (touch.fingerId < maxTouches)
                {
                    var data = touchData[touch.fingerId];
                    bool uiRelease = false;
                    bool isOverUI = UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject(touch.fingerId); ;
                    if (releaseOverUI)
                    {
                        uiRelease = isOverUI;
                    }

                    if (touch.phase == TouchPhase.Began)
                    {
                        if (!isOverUI)
                        {
                            data.pointInitial = touch.position;

                            //it's starting, check to see if any joypads available
                            foreach (var joypad in touchJoypads)
                            {
                                if (!joypad.active)
                                {
                                    if (RectTransformUtility.RectangleContainsScreenPoint(joypad.RectArea, touch.position))
                                    {
                                        joypad.active = true;
                                        joypad.Visible = joypad.visibleWhenActive;
                                        joypad.socket.position = data.pointInitial;
                                        joypad.JoypadStartedEvent();
                                        data.joypad = joypad;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    if (touch.phase != TouchPhase.Ended && touch.phase != TouchPhase.Canceled && !uiRelease)
                    {
                        data.pointCurrent = touch.position;
                        data.touching = true;

                        //it's acting, check to see if it has a joypad
                        if (data.joypad != null)
                        {
                            data.joypad.knob.position = Vector2.MoveTowards(data.pointInitial, data.pointCurrent, (joypadSocketRadius * Screen.width) / (CanvasScaler.referenceResolution.x * 2f));
                            if (data.joypad.restrictHorizontal)
                            {
                                data.joypad.knob.localPosition = new Vector2(0f, data.joypad.knob.localPosition.y);
                            }
                            if (data.joypad.restrictVertical)
                            {
                                data.joypad.knob.localPosition = new Vector2(data.joypad.knob.localPosition.x, 0f);
                            }

                        }
                    }
                    else
                    {
                        data.touching = false;
                        data.pointInitial = Vector2.zero;
                        data.pointCurrent = Vector2.zero;

                        //stopped acting, disable joypad
                        if (data.joypad != null)
                        {
                            data.joypad.JoypadReleasedEvent();
                            data.joypad.active = false;
                            data.joypad.socket.anchoredPosition = data.joypad.initialPosition;
                            data.joypad.knob.localPosition = Vector3.zero;
                            data.joypad.Visible = data.joypad.visibleWhenInactive;
                        }
                        data.joypad = null;
                    }
                }
            }

            //UPDATE mouse data
            bool uiMouseRelease = false;
            bool isMouseOverUI = UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject(-1);
            if (releaseOverUI)
            {
                uiMouseRelease = isMouseOverUI;
            }

            if (Input.GetMouseButtonDown(0))
            {
                if (!isMouseOverUI)
                {
                    mouseData.pointInitial = Input.mousePosition;

                    //it's starting, check to see if any joypads available
                    foreach (var joypad in touchJoypads)
                    {
                        if (!joypad.active)
                        {
                            if (RectTransformUtility.RectangleContainsScreenPoint(joypad.RectArea, Input.mousePosition))
                            {
                                joypad.active = true;
                                joypad.Visible = joypad.visibleWhenActive;
                                mouseData.joypad = joypad;
                                joypad.socket.position = mouseData.pointInitial;
                                joypad.JoypadStartedEvent();
                                break;
                            }
                        }
                    }
                }
            }
            if (Input.GetMouseButton(0) && !uiMouseRelease)
            {
                mouseData.pointCurrent = Input.mousePosition;
                mouseData.touching = true;

                //it's acting, check to see if it has a joypad
                if (mouseData.joypad != null)
                {
                    mouseData.joypad.knob.position = Vector2.MoveTowards(mouseData.pointInitial, mouseData.pointCurrent, (joypadSocketRadius * Screen.width) / (CanvasScaler.referenceResolution.x * 2f));
                    if (mouseData.joypad.restrictHorizontal)
                    {
                        mouseData.joypad.knob.localPosition = new Vector2(0f, mouseData.joypad.knob.localPosition.y);
                    }
                    if (mouseData.joypad.restrictVertical)
                    {
                        mouseData.joypad.knob.localPosition = new Vector2(mouseData.joypad.knob.localPosition.x, 0f);
                    }
                }

            }
            if (Input.GetMouseButtonUp(0) || uiMouseRelease)
            {
                mouseData.touching = false;
                mouseData.pointInitial = Vector2.zero;
                mouseData.pointCurrent = Vector2.zero;

                //stopped acting, disable joypad
                if (mouseData.joypad != null)
                {
                    mouseData.joypad.JoypadReleasedEvent();
                    mouseData.joypad.active = false;
                    mouseData.joypad.socket.anchoredPosition = mouseData.joypad.initialPosition;
                    mouseData.joypad.knob.localPosition = Vector3.zero;
                    mouseData.joypad.Visible = mouseData.joypad.visibleWhenInactive;
                }
                mouseData.joypad = null;
            }

            var axisX = Input.GetAxisRaw("Horizontal");
            var axisY = Input.GetAxisRaw("Vertical");
            var controllerDir = new Vector2(axisX, axisY);
            controllerDir = controllerDir.normalized;
            if (controllerDir != Vector2.zero)
            {
                controllerData.touching = true;
                foreach (var n in touchData)
                {
                    if (n.joypad != null && n.joypad == touchJoypads[0])
                    {
                        n.touching = false;
                        n.pointInitial = Vector2.zero;
                        n.pointCurrent = Vector2.zero;

                        //stopped acting, disable joypad
                        n.joypad = null;
                    }
                }
                if (mouseData.joypad != null && mouseData.joypad == touchJoypads[0])
                {
                    mouseData.touching = false;
                    mouseData.pointInitial = Vector2.zero;
                    mouseData.pointCurrent = Vector2.zero;

                    //stopped acting, disable joypad
                    mouseData.joypad = null;
                }

                controllerData.joypad = touchJoypads[0];
                controllerData.joypad.active = true;
                controllerData.pointInitial = Vector2.zero;
                controllerData.pointCurrent = (controllerDir * Screen.width) / CanvasScaler.referenceResolution.x;
                controllerData.pointCurrent = controllerData.pointCurrent * joypadSocketRadius;
                controllerData.joypad.Visible = false;
            }
            else
            {
                controllerData.touching = false;
                controllerData.pointInitial = Vector2.zero;
                controllerData.pointCurrent = Vector2.zero;

                //stopped acting, disable joypad
                if (controllerData.joypad != null)
                {
                    controllerData.joypad.active = false;
                    controllerData.joypad.socket.anchoredPosition = controllerData.joypad.initialPosition;
                    controllerData.joypad.knob.localPosition = Vector3.zero;
                    controllerData.joypad.Visible = controllerData.joypad.visibleWhenInactive;
                }
                controllerData.joypad = null;
            }

            foreach (var joypad in touchJoypads)
            {
                joypad.JoypadEventUpdate();
            }
        }

        internal void Disable()
        {
            foreach (var n in touchData)
            {
                n.touching = false;
                n.pointInitial = Vector2.zero;
                n.pointCurrent = Vector2.zero;

                //stopped acting, disable joypad
                if (n.joypad != null)
                {
                    n.joypad.active = false;
                    n.joypad.socket.anchoredPosition = n.joypad.initialPosition;
                    n.joypad.knob.localPosition = Vector3.zero;
                    n.joypad.Visible = n.joypad.visibleWhenInactive;
                }
                n.joypad = null;
            }

            mouseData.touching = false;
            mouseData.pointInitial = Vector2.zero;
            mouseData.pointCurrent = Vector2.zero;

            //stopped acting, disable joypad
            if (mouseData.joypad != null)
            {
                mouseData.joypad.active = false;
                mouseData.joypad.socket.anchoredPosition = mouseData.joypad.initialPosition;
                mouseData.joypad.knob.localPosition = Vector3.zero;
                mouseData.joypad.Visible = mouseData.joypad.visibleWhenInactive;
            }
            mouseData.joypad = null;

            gameObject.SetActive(false);
        }



        // Here is where the Enable() Method should be...
        // If only Santiago did it...

        //tomapavó
        internal void Enable()
        {
            gameObject.SetActive(true);
        }

        public TouchJoypad GetJoypad(int joypadIndex)
        {
            if (joypadIndex < touchJoypads.Length && touchJoypads[joypadIndex] != null)
            {
                return touchJoypads[joypadIndex];
            }
            return null;
        }
        public TouchJoypad GetJoypad(string joypadName)
        {
            if (string.IsNullOrEmpty(joypadName))
            {
                return null;
            }
            TouchJoypad joypad = null;
            foreach (var n in touchJoypads)
            {
                if (n != null && n.joypadName == joypadName)
                {
                    joypad = n;
                    break;
                }
            }
            if (joypad != null)
            {
                return joypad;
            }
            return null;
        }

        public TouchJoypadButton GetJoypadButton(int buttonIndex)
        {
            if (buttonIndex < touchJoypadButtons.Length && touchJoypadButtons[buttonIndex] != null)
            {
                return touchJoypadButtons[buttonIndex];
            }
            return null;
        }
        public TouchJoypadButton GetJoypadButton(string buttonName)
        {
            if (string.IsNullOrEmpty(buttonName))
            {
                return null;
            }
            TouchJoypadButton button = null;
            foreach (var n in touchJoypadButtons)
            {
                if (n != null && n.buttonName == buttonName)
                {
                    button = n;
                    break;
                }
            }
            if (button != null)
            {
                return button;
            }
            return null;
        }
    }
}