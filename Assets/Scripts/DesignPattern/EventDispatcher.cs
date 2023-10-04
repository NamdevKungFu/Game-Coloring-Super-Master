using UnityEngine;
using System.Collections.Generic;
using System;

public class EventDispatcher : MonoBehaviour
{
    private static EventDispatcher s_instance;
    public static EventDispatcher Instance
    {
        get
        {
            if (s_instance == null)
            {
                GameObject singletonObject = new GameObject();
                s_instance = singletonObject.AddComponent<EventDispatcher>();
                singletonObject.name = "EventDispatcher";
            }
            return s_instance;
        }
        private set { }
    }

    public static bool HasInstance()
    {
        return s_instance != null;
    }

    private void Awake()
    {
        if (s_instance != null && s_instance.GetInstanceID() != GetInstanceID())
        {
            Destroy(gameObject);
        }
        else
        {
            s_instance = this;
        }

        DontDestroyOnLoad(gameObject);
    }


    private void OnDestroy()
    {
        if (s_instance == this)
        {
            ClearAllListener();
            s_instance = null;
        }
    }

    private Dictionary<EventID, Action<object>> _listeners = new Dictionary<EventID, Action<object>>();

    public void RegisterListener(EventID eventID, Action<object> callback)
    {
        if (_listeners.ContainsKey(eventID))
        {
            _listeners[eventID] += callback;
        }
        else
        {
            _listeners.Add(eventID, null);
            _listeners[eventID] += callback;
        }
    }

    public void PostEvent(EventID eventID, object param = null)
    {
        if (!_listeners.ContainsKey(eventID))
        {
            return;
        }

        var callbacks = _listeners[eventID];
        if (callbacks != null)
        {
            callbacks(param);
        }
        else
        {
            _listeners.Remove(eventID);
        }
    }

    public void RemoveListener(EventID eventID, Action<object> callback)
    {
        if (_listeners.ContainsKey(eventID))
        {
            _listeners[eventID] -= callback;
        }
    }

    public void ClearAllListener()
    {
        _listeners.Clear();
    }
}

public static class EventDispatcherExtension
{
    public static void RegisterListener(this MonoBehaviour listener, EventID eventID, Action<object> callback)
    {
        EventDispatcher.Instance.RegisterListener(eventID, callback);
    }

    public static void PostEvent(this MonoBehaviour listener, EventID eventID, object param)
    {
        EventDispatcher.Instance.PostEvent(eventID, param);
    }

    public static void PostEvent(this MonoBehaviour sender, EventID eventID)
    {
        EventDispatcher.Instance.PostEvent(eventID, null);
    }
}
