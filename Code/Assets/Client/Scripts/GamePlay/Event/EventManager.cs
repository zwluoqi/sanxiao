//--------------------------------------------------
// Ellan Game Framework v1.0
// Copyright 2013 Jiang Yin. All rights reserved.
// Feedback: mailto:ellan@foxmail.com
// Modify by HeGY.
//--------------------------------------------------

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


public sealed class EventManager 
{

    private static EventManager instance;

    public static EventManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new EventManager();
            }
            return instance;
        }
    }

    public delegate void EventHandler(EventDefine type, System.Object[] args);

    public class Event
    {
        public EventDefine type;
        public System.Object[] args;

        public Event(EventDefine type, System.Object[] args)
        {
            this.type = type;
            this.args = args;
        }
    }



    private IDictionary<EventDefine, IList<EventHandler>> m_EventHandlerList = new Dictionary<EventDefine, IList<EventHandler>>();
    private IList<Event> m_EventList = new List<Event>();
    private static String ModuleName = "Event Module";

	

    public void Update(float deltaTime)
    {
        foreach (Event e in m_EventList)
        {
            ProcessEvent(e);
        }

        m_EventList.Clear();
    }

    public void ClearEvent()
    {
        m_EventList.Clear();
        m_EventHandlerList.Clear();
    }

    public Boolean RegisterEventHandler(EventDefine type, EventHandler handler)
    {
        if (handler == null)
        {
            Debug.LogWarning(ModuleName + "注册的EventHandler不能为空。");
            return false;
        }

        if (!m_EventHandlerList.ContainsKey(type))
        {
            m_EventHandlerList.Add(type, new List<EventHandler>());
        }

        IList<EventHandler> handlerList = m_EventHandlerList[type];
        if (handlerList.Contains(handler))
        {
            Debug.LogWarning(ModuleName + "注册的EventHandler已存在。");
            return false;
        }

        handlerList.Add(handler);

        return true;
    }

    public Boolean UnregisterEventHandler(EventDefine type, EventHandler handler)
    {
        if (handler == null)
        {
           Debug.LogWarning(ModuleName + "解除注册的EventHandler不能为空。");
            return false;
        }

        if (!m_EventHandlerList.ContainsKey(type))
        {
            Debug.LogWarning(ModuleName + "解除注册的EventHandler不存在。");
            return false;
        }

        IList<EventHandler> handlerList = m_EventHandlerList[type];
        if (!handlerList.Contains(handler))
        {
            Debug.LogWarning(ModuleName + "解除注册的EventHandler不存在。");
            return false;
        }

        handlerList.Remove(handler);

        return true;
    }

    public void Fire(EventDefine type)
    {
        Fire(type, new System.Object[] { });
    }

    public void Fire(EventDefine type, System.Object arg0)
    {
        Fire(type, new System.Object[] { arg0 });
    }

    public void Fire(EventDefine type, System.Object arg0, System.Object arg1)
    {
        Fire(type, new System.Object[] { arg0, arg1 });
    }

    public void Fire(EventDefine type, System.Object arg0, System.Object arg1, System.Object arg2)
    {
        Fire(type, new System.Object[] { arg0, arg1, arg2 });
    }

    public void Fire(EventDefine type, System.Object[] args)
    {
        m_EventList.Add(new Event(type, args));
    }

    private void ProcessEvent(Event e)
    {
        if (!m_EventHandlerList.ContainsKey(e.type))
        {
            return;
        }

        IList<EventHandler> handlerList = m_EventHandlerList[e.type];
        foreach (EventHandler handler in handlerList)
        {
            if (handler == null)
            {
                Debug.LogWarning(ModuleName + "已注册的EventHandler为空，不能调用。");
                continue;
            }

            handler(e.type, e.args);
        }
    }
}
