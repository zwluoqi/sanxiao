using UnityEngine;
using System.Collections;

using System.Collections.Generic;

public class WidgetBufferManager  {

    private static WidgetBufferManager instance;

    public static WidgetBufferManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new WidgetBufferManager();
            }
            return instance;
        }
    }




	private Dictionary<string,ObjectPool> widgetBuffers = new Dictionary<string, ObjectPool>();


    public void PreLoadWidget(string objName, int captily, Transform parent)
    {
        if (!widgetBuffers.ContainsKey(objName))
        {
            GameObject uObj = Resources.Load("Widget/" + objName) as GameObject;
            ObjectPool poolObj = new ObjectPool(uObj, captily, null);
            widgetBuffers.Add(objName, poolObj);
        }
        else
        {
            widgetBuffers[objName].PrePopulate(captily);
        }
    }

	public GameObject loadWidget(string objName,Transform parent){
		if(!widgetBuffers.ContainsKey(objName)){
			GameObject uObj = Resources.Load("Widget/"+objName) as GameObject;
			ObjectPool poolObj = new ObjectPool(uObj,0,null);
			widgetBuffers.Add(objName,poolObj);
		}
		
		GameObject gObj = widgetBuffers[objName].Create(Vector3.zero,Vector3.one,parent);

		return gObj;
	}
		
	public GameObject loadWidget(string objName){
		return loadWidget(objName,null);
	}
	
	public void DestroyWidgetObj(string objName,GameObject obj){		
		if(!widgetBuffers.ContainsKey(objName)){
			Debug.LogError("obj destroy error");
		}else{
			widgetBuffers[objName].Destroy(obj);
		}
	}
	
	public void DestroyWidgetObjs(string objName,List<GameObject> objs){

        foreach (GameObject go in objs)
        {
            DestroyWidgetObj(objName, go);
        }
	}

    public void ClearObjs()
    {
        foreach (KeyValuePair<string, ObjectPool> buffer in widgetBuffers)
        {
            buffer.Value.DestroyAllFromMem();
        }
    }
	
}
