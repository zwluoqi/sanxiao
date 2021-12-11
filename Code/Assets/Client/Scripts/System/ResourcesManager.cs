using UnityEngine;
using System.Collections;


public class ResourcesManager :MonoBehaviour  {

    private static ResourcesManager instance;

    public static ResourcesManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<ResourcesManager>();
            }
            if (instance == null)
            {
                instance = new GameObject("ResourcesManager").AddComponent<ResourcesManager>();
            }
            return instance;
        }

    }



	private Camera mainCamera;
	
	private GameObject CreateGameObj(string objName){
		mainCamera = Camera.main;
		if(mainCamera == null){
			mainCamera = GameObject.FindObjectOfType(typeof(Camera)) as Camera;
		}
		if(mainCamera == null){
			return  CreateGameObj(objName,null);
		}else{
			return  CreateGameObj(objName,mainCamera.transform);
		}
		

	}
	
	private GameObject CreateGameObj(string objName,Transform parentGameObject){
		Object quitapp = Resources.Load(objName);
		GameObject topWindow = GameObject.Instantiate(quitapp) as GameObject;
		topWindow.transform.parent = parentGameObject;
		topWindow.transform.localScale = Vector3.one;
		topWindow.transform.localPosition = Vector3.zero;
		topWindow.transform.localRotation = Quaternion.identity;
		
		//Resources.UnloadUnusedAssets();
		return topWindow;
	}
	

	public GameObject loadWidget(string objName,Transform parentGameObject){
		return CreateGameObj("Widget/"+objName,parentGameObject);
	}
	
	public GameObject loadWidget(string objName){
		return CreateGameObj("Widget/"+objName);
	}

	public GameObject LoadWinGameObject(string objName,Transform parentGameObject){
		GameObject obj = CreateGameObj("Windows/"+objName,parentGameObject);
		if(obj.GetComponent<UIPanel>() != null){
			obj.GetComponent<UIPanel>().depth = 10 + PageManager.Instance.memoryPages.Count;
		}	
		return obj;
	}
	
	public GameObject LoadWinGameObject(string objName){
		GameObject obj = CreateGameObj("Windows/"+objName);
		if(obj.GetComponent<UIPanel>() != null){
            obj.GetComponent<UIPanel>().depth = 10 + PageManager.Instance.memoryPages.Count;
		}		
		return obj;
	}	
	
	public GameObject LoadParticleObject(string objName,Transform parentGameObject){
		return CreateGameObj("Particles/"+objName,parentGameObject);
	}
	
	public GameObject LoadParticleObject(string objName){
		return CreateGameObj("Particles/"+objName);
	}
	
	
	
	public GameObject LoadBoxGameObject(string objName){
		GameObject obj = CreateGameObj("Windows/PopUp/"+objName);
		obj.transform.localPosition = new Vector3(2000,0,0);
		if(obj.GetComponent<UIPanel>() != null){
			obj.GetComponent<UIPanel>().depth = 3000;
		}
		return obj;
	}	
	
	public GameObject LoadJinengGameObject(string objName,Transform parentGameObject){
		return CreateGameObj("jineng/"+objName,parentGameObject);
	}
}
