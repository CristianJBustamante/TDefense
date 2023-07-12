using UnityEngine;

namespace com.Pizia.TouchJoypadSystem
{
    public class TouchJoypad : MonoBehaviour
    {
        public string joypadName;
        [HideInInspector]
        public bool active = false;
        public RectTransform RectArea
        {
            get
            {
                if (rect == null)
                    rect = GetComponent<RectTransform>();
                return rect;
            }
        }
        private RectTransform rect;
        public RectTransform socket;
        public RectTransform knob;
        public bool visibleWhenActive;
        public bool visibleWhenInactive;
        internal Vector2 initialPosition;
        [SerializeField]
        private UnityEngine.UI.Text debugText;
        [HideInInspector]
        public TouchJoypadManager manager;
        [Range(0f, 1f)]
        public float deadzone = 0.1f;
        public bool resetOnRelease = true;
        [Header("Restrictions")]
        public bool restrictHorizontal = false;
        public bool restrictVertical = false;
        internal bool isOn = false;

        public delegate void EventHandler(TouchJoypad sender, Vector2 direction);
        public event EventHandler joypadEvent;
        public delegate void SimpleEventHandler(TouchJoypad sender);
        public event SimpleEventHandler joypadStarted, joypadReleased;
        internal void JoypadEventUpdate()
        {
            var dir = Direction;
            if ((Mathf.Abs(dir.x) > deadzone || Mathf.Abs(dir.y) > deadzone) && dir != Vector2.zero)
            {
                //DebugText = dir.ToString ();
                joypadEvent?.Invoke(this, dir);
            }
        }
        internal void JoypadStartedEvent()
        {
            joypadStarted?.Invoke(this);
            isOn = true;
        }
        internal void JoypadReleasedEvent()
        {
            if (resetOnRelease)
            {
                joypadEvent?.Invoke(this, Vector2.zero);
            }
            joypadReleased?.Invoke(this);
            isOn = false;
        }
        public string DebugText
        {
            get
            {
                if (debugText != null && debugText.isActiveAndEnabled)
                {
                    return debugText.text;
                }
                return "";
            }
            internal set
            {
                if (debugText != null && debugText.isActiveAndEnabled)
                {
                    debugText.text = value;
                }
            }
        }
        public bool Visible
        {
            get { return socket.gameObject.activeSelf; }
            set { socket.gameObject.SetActive(value); }
        }
        public Vector2 Direction
        {
            get
            {
                if (active)
                {
                    foreach (var data in manager.touchData)
                    {
                        if (data.joypad != null && data.joypad == this)
                        {
                            var dir = data.Direction;
                            if (Mathf.Abs(dir.x) < deadzone || restrictHorizontal)
                            {
                                dir.x = 0f;
                            }
                            if (Mathf.Abs(dir.y) < deadzone || restrictVertical)
                            {
                                dir.y = 0f;
                            }
                            return dir;
                        }
                    }
                    if (manager.mouseData.joypad != null && manager.mouseData.joypad == this)
                    {
                        var dir = manager.mouseData.Direction;
                        if (Mathf.Abs(dir.x) < deadzone || restrictHorizontal)
                        {
                            dir.x = 0f;
                        }
                        if (Mathf.Abs(dir.y) < deadzone || restrictVertical)
                        {
                            dir.y = 0f;
                        }
                        return dir;
                    }
                    if (
                        manager.controllerData.joypad != null
                        && manager.controllerData.joypad == this
                    )
                    {
                        var dir = manager.controllerData.Direction;
                        if (Mathf.Abs(dir.x) < deadzone || restrictHorizontal)
                        {
                            dir.x = 0f;
                        }
                        if (Mathf.Abs(dir.y) < deadzone || restrictVertical)
                        {
                            dir.y = 0f;
                        }
                        return dir;
                    }
                }
                return Vector2.zero;
            }
        }
    }
}
