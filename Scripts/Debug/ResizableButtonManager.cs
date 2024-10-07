using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CapturedGame
{
    public class ResizableButtonManager : MonoBehaviour, IDebug
    {
        private static ResizableButtonManager instance;
        private Dictionary<string, List<ButtonInfo>> buttonGroups;
        private Dictionary<string, float> buttonWidths;

        private string resizingGroup = null;
        private Rect windowRect;
        private bool isResizing = false;
        private Vector2 lastMousePosition;

        public string DebugName { get; } = "Debug Buttons";
        public bool IsDebugMode { get; private set; }

        void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
                buttonGroups = new Dictionary<string, List<ButtonInfo>>();
                buttonWidths = new Dictionary<string, float>();
                windowRect = new Rect(10, 10, 800, 300); // Default window size
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public static ResizableButtonManager Instance
        {
            get
            {
                if (instance == null)
                {
                    GameObject obj = new GameObject("ResizableButtonManager");
                    instance = obj.AddComponent<ResizableButtonManager>();
                }
                return instance;
            }
        }

        public void AddButton(string group, string buttonText, Action onClick)
        {
            if (!buttonGroups.ContainsKey(group))
            {
                buttonGroups[group] = new List<ButtonInfo>();
                buttonWidths[group] = 150f; // Default width
            }
            buttonGroups[group].Add(new ButtonInfo(buttonText, onClick));
        }
        
        public void DrawWindow(string boxName, int windowID)
        {
            windowRect = GUI.Window(windowID, windowRect, id =>
            {
                DrawButtons();
                GUI.DragWindow(new Rect(0, 0, windowRect.width, 20));
                GUI.Box(new Rect(windowRect.width - 20, windowRect.height - 20, 20, 20), "");
            }, boxName);

            ResizeWindow();
        }

        private void ResizeWindow()
        {
            Rect resizeHandleRect = new Rect(windowRect.x + windowRect.width - 20, windowRect.y + windowRect.height - 20, 20, 20);

            if (Event.current.type == EventType.MouseDown && resizeHandleRect.Contains(Event.current.mousePosition))
            {
                isResizing = true;
                lastMousePosition = Event.current.mousePosition;
                Event.current.Use();
            }

            if (isResizing)
            {
                Vector2 mouseDelta = Event.current.mousePosition - lastMousePosition;
                windowRect.width = Mathf.Max(100, windowRect.width + mouseDelta.x);
                windowRect.height = Mathf.Max(100, windowRect.height + mouseDelta.y);
                lastMousePosition = Event.current.mousePosition;

                if (Event.current.type == EventType.MouseUp)
                {
                    isResizing = false;
                }
            }
        }

        private void DrawButtons()
        {
            float yOffset = 40f;
            float xOffset = 10f;

            foreach (var group in buttonGroups)
            {
                GUI.Label(new Rect(xOffset, yOffset, buttonWidths[group.Key], 20), group.Key);
                yOffset += 20f;

                foreach (var button in group.Value)
                {
                    if (GUI.Button(new Rect(xOffset, yOffset, buttonWidths[group.Key], 30), button.Text))
                    {
                        button.OnClick.Invoke();
                    }
                    yOffset += 35f;
                }

                // Move to the next group horizontally
                xOffset += buttonWidths[group.Key] + 20f;
                yOffset = 40f; // Reset yOffset for the next group
            }
        }
        
        void OnGUI()
        {
            if (!IsDebugMode) return;
            
            DrawWindow(DebugName, 2);
        }

        public void OnDebugChange(bool enable)
        {
            IsDebugMode = enable;
        }
        
        private class ButtonInfo
        {
            public string Text { get; }
            public Action OnClick { get; }

            public ButtonInfo(string text, Action onClick)
            {
                Text = text;
                OnClick = onClick;
            }
        }
    }
}