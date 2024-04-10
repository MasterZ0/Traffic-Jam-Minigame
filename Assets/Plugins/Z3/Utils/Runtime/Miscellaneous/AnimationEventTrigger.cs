using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Z3.Utils
{
    [Serializable]
    public struct EventReference
    {
        public string eventName;
        public UnityEvent unityEvent;
    }

    public class AnimationEventTrigger : MonoBehaviour
    {
        [Header("Event Trigger")]
        [SerializeField] private EventReference[] eventReferences;

        public event Action<string> OnEventTrigger;

        public void OnEvent(string eventName)
        {
            EventReference reference = eventReferences.First(e => e.eventName == eventName);
            reference.unityEvent.Invoke();
            OnEventTrigger?.Invoke(eventName);
        }
    }
}
