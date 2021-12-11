using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public delegate void EventCallback();
public delegate void EventCallbackEnd();


public class EventDelayManger 
	{

    public bool enabled;
    private static EventDelayManger instance;

    public static EventDelayManger Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new EventDelayManger();
                }
                return instance;
            }
        }

	    public enum EventLifeCircle
	    {
	        CREATE,
	        DOING,
	        PAUSE,
	        DEATH,
	    }

	    public enum EventType
	    {
	        JUST_ONCE,
	        COUNT_LOOP,
	        UNLIMIT_LOOP, 
	    }
	    
	    public class EventDelay
	    {
	        internal EventLifeCircle state = EventLifeCircle.CREATE;
	        public EventLifeCircle State
	        {
	            get { return state; }
	        }

	        internal EventType type;
	        public EventType Type
	        {
	            get { return type; }
	        }

	        internal float triggerTime;
	        public float TriggerTime
	        {
	            get { return triggerTime; }
	        }

	        internal float remainderTime;

	        internal int count;
	        public int Count
	        {
	            get { return count; }
	        }

	        internal float spaceTime;
	        public float SpaceTime
	        {
	            get { return spaceTime; }
	        }


	        internal EventCallback callback;
	        internal EventCallbackEnd callbackEnd;

	        internal EventDelay(EventCallback callback, float triggerTime)
	        {
	            this.triggerTime = triggerTime;
	            this.callback = callback;

	            this.type = EventType.JUST_ONCE;
	        }

	        internal EventDelay(EventCallback callback,EventCallbackEnd callbackEnd, float triggerTime,float spaceTime, int count)
	        {
	            this.triggerTime = triggerTime;
	            this.callback = callback;
	            this.callbackEnd = callbackEnd;
	            this.count = count;
	            this.spaceTime = spaceTime;

	            this.type = EventType.COUNT_LOOP;
	        }

	        internal EventDelay(EventCallback callback, float triggerTime, float spaceTime)
	        {
	           
	            this.triggerTime = triggerTime;
	            this.callback = callback;
	            this.spaceTime = spaceTime;

	            this.type = EventType.UNLIMIT_LOOP;
	        }

	    }

	    private List<EventDelay> eventList = new List<EventDelay>();
	    private List<EventDelay> pauseList = new List<EventDelay>();
	    private List<EventDelay> cacheList = new List<EventDelay>();
	    private List<EventDelay> deathList = new List<EventDelay>();


	    private float timeLine = 0.0f;

	    public EventDelay CreateEvent(EventCallback cb, float delay)
	    {
	        EventDelay es = new EventDelay(cb, delay + timeLine);
	        cacheList.Add(es);
	        return es;
	    }

	    public EventDelay CreateEvent(EventCallback cb, float delay,float space)
	    {
	        EventDelay es = new EventDelay(cb, delay + timeLine, space);
	        cacheList.Add(es);
	        return es;
	    }


	    public EventDelay CreateEvent(EventCallback cb, EventCallbackEnd cbe, float delay ,float space, int count)
	    {
	        EventDelay es = new EventDelay(cb,cbe,delay + timeLine,space, count);
	        cacheList.Add(es);
	        return es;
	    }



	    public void Delete(EventDelay ed)
	    {
	        ed.state = EventLifeCircle.DEATH;
	    }


	    public void Pause(EventDelay ed)
	    {
	        if (ed.state != EventLifeCircle.DEATH)
	        {
	            ed.state = EventLifeCircle.PAUSE;
	            ed.remainderTime = ed.triggerTime - timeLine;
	        }
	    }

	    public void Continue(EventDelay ed)
	    {
	        if (ed.state == EventLifeCircle.PAUSE)
	        {
	            ed.triggerTime = timeLine + ed.remainderTime;
	            ed.state = EventLifeCircle.DOING;
	        }
	    }

		public float GetEventRemainTime(EventDelay ed)
		{
			if(ed == null || ed.state == EventLifeCircle.DEATH)
			{
				return float.MaxValue;
			}
			if(ed.state == EventLifeCircle.PAUSE)
			{
				return ed.remainderTime;
			}
			else
			{
				return ed.triggerTime - timeLine;
			}
		}

		public void OrderedUpdate (float deltaTime) 
	    {
			if(!enabled)
			{
				return;
			}
	        // 更新时间线
			UpdateTimeline(deltaTime);

	        UpdateCacheList();

	        UpdatePauseList();

	        UpdateLifeCircle();
			
	        UpdateDoingList();

		}

		private void UpdateTimeline(float deltaTime)
	    {
			timeLine += deltaTime;
	    }

	    private void UpdateCacheList()
	    {
	        // 缓冲表不为空
	        if (cacheList.Count != 0)
	        {
	            foreach (EventDelay e in cacheList)
	            {
	                if (e.state == EventLifeCircle.CREATE || e.state == EventLifeCircle.DOING)
	                {
	                    AddEvent(e);
	                }
	                else if (e.state == EventLifeCircle.PAUSE)
	                {
	                    pauseList.Add(e);
	                }
	            }
	            cacheList.Clear();
	        }
	    }


	    private void UpdatePauseList()
	    {
	        // 暂停表不为空
	        if (pauseList.Count != 0)
	        {
	            foreach (EventDelay e in pauseList)
	            {
	                if (e.state == EventLifeCircle.DOING)
	                {
	                    AddEvent(e);
	                    deathList.Add(e);
	                }
	            }

	            foreach (EventDelay e in deathList)
	            {
	                pauseList.Remove(e);
	            }
	            deathList.Clear();

	        }
	    }

	    private void UpdateDoingList()
	    {
	        foreach (EventDelay e in eventList)
	        {
	            if (timeLine >= e.triggerTime)
	            {
	                if (e.state != EventLifeCircle.DOING)
	                {
	                    continue;
	                }

	                switch (e.type)
	                {
	                    case EventType.JUST_ONCE: //一次性事件
	                        {
	                            e.state = EventLifeCircle.DEATH;
	                            e.callback();
	                        }
	                        break;
	                    case EventType.COUNT_LOOP://有限循环事件
	                        {
	                            e.count--;
                                e.triggerTime += e.spaceTime;
	                            if (e.count == 0)
	                            {
	                                e.callback();
	                                e.callbackEnd();
	                                e.state = EventLifeCircle.DEATH;
	                            }
	                            else
	                            {
	                                e.callback();
	                            }
	                        }
	                        break;
	                    case EventType.UNLIMIT_LOOP://无限循环事件
	                        {
	                            e.triggerTime += e.spaceTime;
	                            e.callback();
	                        }
	                        break;
	                    default:
	                        e.state = EventLifeCircle.DEATH;
	                        break;
	                }

	            }
	            else //后面的事件不会触发
	            {
	                break;
	            }

	        }
	    }

	    //处理Event的生命周期
	    private void UpdateLifeCircle()
	    {
	        foreach (EventDelay e in eventList)
	        {
	            switch (e.state)
	            {
	                case EventLifeCircle.CREATE:
	                    cacheList.Add(e);
	                    break;
	                case EventLifeCircle.DOING:

	                    break;
	                case EventLifeCircle.PAUSE:
	                    pauseList.Add(e);
	                    break;
	                case EventLifeCircle.DEATH:
	                    deathList.Add(e);
	                    break;
	            }
	        }

	        //转移到Cache表
	        if (cacheList.Count != 0)
	        {
	            foreach (EventDelay e in cacheList)
	            {
	                eventList.Remove(e);
	            }
	        }


	        //转移到暂停表
	        if (pauseList.Count != 0)
	        {
	            foreach (EventDelay e in pauseList)
	            {
	                eventList.Remove(e);
	            }

	        }

	        // 转移到死亡表
	        if (deathList.Count != 0)
	        {
	            foreach (EventDelay e in deathList)
	            {
	                eventList.Remove(e);
	            }

	            deathList.Clear();
	        }
	    }

	    private void AddEvent(EventDelay e)
	    {
	        e.state = EventLifeCircle.DOING;
	        int i=0;
	        for (i = 0; i < eventList.Count; i++)
	        {
	            if (eventList[i].triggerTime > e.triggerTime)
	            {
	                eventList.Insert(i, e);
	                return;
	            }
	        }
	        eventList.Add(e);  
	    }

		public void Init()
		{
			timeLine = 0f;
			enabled = true;
			ClearAllEvent();
		}

		public void Close()
		{
			enabled = false;
			ClearAllEvent();
		}
		
	    public static EventDelayManger GetInstance()
	    {
            return Instance;
	    }

	    public void ClearAllEvent()
	    { 
	        eventList.Clear();
	        pauseList.Clear();
	        cacheList.Clear();
	        deathList.Clear();
	    }
	}