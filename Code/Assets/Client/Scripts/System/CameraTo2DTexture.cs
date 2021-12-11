using UnityEngine;
using System.Collections;

public class CameraTo2DTexture : MonoBehaviour {
    public UITexture utsource;
    private UITexture ut;

    /// <summary>
    /// 摄像机成像到贴图上;
    /// </summary>
    /// <param name="utv"></param>
    /// <returns></returns>
    public void Init()
    {
        utsource.mainTexture = CaptureCamera(GetComponent<Camera>(), new Rect(0, 0, Screen.width, Screen.height));
    }


    void OnEnable()
    {
        Init();
    }

    void OnDisable()
    {
        GetComponent<Camera>().targetTexture.Release();
        GetComponent<Camera>().targetTexture = null;
        utsource.mainTexture = null;
    }

    /*
    /// <summary>  
    /// Captures the screenshot2.  
    /// </summary>  
    /// <returns>The screenshot2.</returns>  
    /// <param name="rect">Rect.截图的区域，左下角为o点</param>  
    Texture2D CaptureScreenshot2(Rect rect, bool save = false)
    {
        // 先创建一个的空纹理，大小可根据实现需要来设置  
        Texture2D screenShot = new Texture2D((int)rect.width, (int)rect.height, TextureFormat.RGB24, false);

        // 读取屏幕像素信息并存储为纹理数据，  
        screenShot.ReadPixels(rect, 0, 0);
        screenShot.Apply();

        if (save)
        {
            // 然后将这些纹理数据，成一个png图片文件  
            byte[] bytes = screenShot.EncodeToPNG();
            string filename = Application.dataPath + "/Screenshot.png";
            System.IO.File.WriteAllBytes(filename, bytes);
            Debug.Log(string.Format("captire on png: {0}", filename));
        }

        // 最后，我返回这个Texture2d对象，这样我们直接，所这个截图图示在游戏中，当然这个根据自己的需求的。  
        return screenShot;
    }
    */

    /// <summary>  
    /// 直接使用target渲染,效率低.
    /// </summary>  
    /// <returns>The screenshot2.</returns>  
    /// <param name="camera">Camera.要被截屏的相机</param>  
    /// <param name="rect">Rect.截屏的区域</param>  
	RenderTexture CaptureCamera(Camera camera, Rect rect,bool save = false)
    {
        // 创建一个RenderTexture对象  
        RenderTexture rt = new RenderTexture((int)rect.width, (int)rect.height, 0);

        // 临时设置相关相机的targetTexture为rt, 并手动渲染相关相机  
        camera.targetTexture = rt;

		return rt;
    }  

}
