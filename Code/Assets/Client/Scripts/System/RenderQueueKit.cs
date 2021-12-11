using UnityEngine;
using System.Collections;

public class RenderQueueKit : MonoBehaviour
{

    public int queue = 2850;
    private int lastQueue;
    // Use this for initialization
    void OnEnable()
    {
        lastQueue = queue;
        foreach (Material material in gameObject.GetComponent<Renderer>().sharedMaterials)
        {
			if(material != null)
			{
           	 	material.renderQueue = queue;
			}
        }
    }

    void Update()
    {

        if (lastQueue != queue)
        {
            OnEnable();
        }
    }

}
