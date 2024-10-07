using UnityEngine;
using CapturedGame;

namespace _Game.Scripts.ScriptableObjects
{
    [CreateAssetMenu(fileName = "DayTasks", menuName = "ScriptableObjects/Day", order = 0)]
    public class DayTask : ScriptableObject
    {
        [System.Serializable]
        public struct TimeEvent
        {
            public Roger.ToDoTasks action;

            [Space]
            public int hour;
            public int minute; 
        }

        public TimeEvent[] tasks = null;
    }
}