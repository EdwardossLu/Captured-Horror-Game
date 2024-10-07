using System;
using _Game.Scripts.Interface;
using _Game.Scripts.ScriptableObjects;
using Sirenix.OdinInspector;
using UnityEngine;
using CapturedGame;

namespace _Game.Scripts.Manager.Actions
{
    public class ScheduleSystem : MonoBehaviour, ISwitchable, IDebugText
    {
        [System.Serializable]
        struct Days
        {
            public Roger.Days day;
            public DayTask todo;
        }
        
        [SerializeField] private PlayMakerFSM fsm = null;

        [Space] 
        [SerializeField] private Days[] days = null;
        
        [Space]
        [ReadOnly, SerializeField] private int currentPriority = -1;
        [ReadOnly, SerializeField] private string currentEvent = string.Empty;
        [ReadOnly, SerializeField] private DayTask currentRoutine;
        
        public string DebugName { get; } = "Schedule";
        public bool IsDebugMode { get; private set; }
        public bool IsActive { get; private set; } = false;
        
        public DraggableResizableTextBox TextBox { get; private set; }

        private void OnEnable()
        {
            TimeManager.onTimeUpdate += OnTimeUpdate;
        }
        
        private void OnDisable()
        {
            TimeManager.onTimeUpdate -= OnTimeUpdate;
        }
        
        private void Start()
        {
            TextBox = new DraggableResizableTextBox(new Rect(10, 10, 250, 200));

            currentRoutine = GetRoutine();
        }
        
        private void OnTimeUpdate(int hour, int minute)
        {
            for (int i = 0; i < currentRoutine.tasks.Length; i++)
            {
                if (!(currentRoutine.tasks[i].hour == hour && currentRoutine.tasks[i].minute == minute)) continue;
            
                // TODO: Add a Priority system based on the AI emotions and current actions
                
                currentEvent = currentRoutine.tasks[i].action.ToString();
                fsm.SendEvent(currentEvent);
            
                break;
            }
        }
        
        public void Activate()
        {
            IsActive = true;
            fsm.enabled = true;
        }

        public void Deactivate()
        {
            IsActive = false;
            fsm.enabled = false;
        }

        private DayTask GetRoutine()
        {
            for (int i = 0; i < days.Length; i++)
            {
                if (days[i].day == TimeManager.Instance.GetCurrentDay)
                    return days[i].todo;
            }

            Debug.LogError("No corresponding days where found. Make sure all days are loading in days!");
            return null;
        }
        
        private void OnGUI()
        {
            if (!IsDebugMode) return;

            string currentDay = TimeManager.Instance.GetCurrentDay.ToString();
            TextBox.DrawWindow($"{currentDay} {DebugName}", 0, DrawValuesContent);
        }

        public void OnDebugChange(bool enable)
        {
            IsDebugMode = enable;
        }

        public void DrawValuesContent()
        {
            GUI.Label(new Rect(10, 10, 230, 230), currentEvent);
        }

        private void OnValidate()
        {
            if (!Application.isEditor)
                return;
            
            Array v = Enum.GetValues(typeof(Roger.Days));
            for (int i = 0; i < Enum.GetValues(typeof(Roger.Days)).Length; i++)
            {
                if ((int)days[i].day != (int)v.GetValue(i))
                {
                    Debug.LogError("ScheduleSystem was changed. You have a missing day or the days not in order!");
                    return;
                }
            }
        }
    }
}