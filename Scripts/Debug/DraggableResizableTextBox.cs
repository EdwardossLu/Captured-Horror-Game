using UnityEngine;
using System;

namespace CapturedGame
{
    public class DraggableResizableTextBox
    {
        private Rect windowRect;
        private bool isResizing = false;
        private Vector2 lastMousePosition;

        public DraggableResizableTextBox(Rect initialRect)
        {
            windowRect = initialRect;
        }

        public void DrawWindow(string boxName, int windowID, Action content)
        {
            windowRect = GUI.Window(windowID, windowRect, id =>
            {
                content();
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
    }
}