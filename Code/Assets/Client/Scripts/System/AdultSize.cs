using UnityEngine;
using System.Collections;

public class AdultSize : MonoBehaviour {
    float currentWidth ;
    float currentHeigth;

    public enum UITYPE
    {
        ZHAI,
        UI9_16,
        UI2_3,
        UI3_4,
        KUAN,
    }

    private UITYPE uiType;
    private UIRoot root;
    void Awake()
    {
        currentWidth = Screen.width;
        currentHeigth = Screen.height;
        float dis = currentWidth / currentHeigth;
        if (dis < 0.54)
        {
            uiType = UITYPE.ZHAI;
        }
        else if (dis >= 0.54 && dis < 0.62f)
        {
            uiType = UITYPE.UI9_16;
        }
        else if(dis >= 0.62f && dis< 0.70f)
        {
            uiType = UITYPE.UI2_3;
        }
        else if (dis >= 0.70f && dis < 0.76f)
        {
            uiType = UITYPE.UI3_4;
        }
        else
        {
            uiType = UITYPE.KUAN;
        }
        root = GetComponent<UIRoot>();
    }
    public void AdustUI()
    {
        //StartCoroutine(_AdustUI());
        //root.manualHeight = (int)SystemConfig.standard_height;

        switch (uiType)
        {
            case UITYPE.ZHAI:
                root.manualHeight = 1460;
                break;
            case UITYPE.UI9_16:
                root.manualHeight = (int)SystemConfig.standard_height;
                break;
            case UITYPE.UI2_3:
                root.manualHeight = 1175;
                break;
            case UITYPE.UI3_4:
                root.manualHeight = 1175;
                break;
            case UITYPE.KUAN:
                root.manualHeight = 1175;
                break;
        }

    }

    public void AdustGameAndLogin()
    {
        //StartCoroutine(_AdustGameAndLogin());
        switch (uiType)
        {
            case UITYPE.ZHAI:
                root.manualHeight = 1460;
                break;
            case UITYPE.UI9_16:
                root.manualHeight = (int)SystemConfig.standard_height;
                break;
            case UITYPE.UI2_3:
                root.manualHeight = 1175;
                break;
            case UITYPE.UI3_4:
                root.manualHeight = 1175;
                break;
            case UITYPE.KUAN:
                root.manualHeight = 1175;
                break;
        }
    }

}
