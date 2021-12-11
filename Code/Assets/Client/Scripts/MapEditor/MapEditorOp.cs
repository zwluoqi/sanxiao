using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

#if UNITY_EDITOR
public class MapEditorOp :MonoBehaviour
{
    private static MapEditorOp instance;

    public static MapEditorOp Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<MapEditorOp>();
            }
            if (instance == null)
            {
                instance = new GameObject("MapEditorOp").AddComponent<MapEditorOp>();
                GameObject.DontDestroyOnLoad(instance.gameObject);
            }
            return instance;
        }

    }

	public bool loginToGame = false;
	// Update is called once per frame
	void OnGUI()
	{
		//Save and test
		if(GUI.Button(new Rect(Screen.width-200,120,160,40),"Stop"))
		{
			Application.LoadLevel("MapEditor");
			gameObject.SetActive(false);
			loginToGame = false;
		}

        if (GUI.Button(new Rect(Screen.width - 200, 60, 160, 40), "Save Ele Data"))
        {
            SaveEleData("g"+LevelData.currentLevel+".txt");
        }
		
	}


	void Update(){
		if(Application.loadedLevelName == "sanxiao" && !loginToGame){
			loginToGame = true;
			SceneManager.Instance.LoginToGame();
		}
	}


    public void SaveEleData(string fileName)
    {
        string saveString = "";

        //set map data
        for (int row = Map.maxRow - 1; row >= 0; row--)
        {
            for (int col = 0; col <= Map.maxCol - 1; col++)
            {
                //Get square at [row,col]
                UISquareView square = Map.Instance.GetSquare(row,col);
                //Save square type to save string
                if (square.item != null)
                {
                    saveString += square.item.tab_id;
                }
                else
                {
                    saveString += "-1";
                }
                //if this column not yet end of row, add space between them
                if (col < (Map.maxCol - 1)) saveString += " ";
            }
            //if this row is not yet end of row, add new line symbol between rows
            if (row >0) saveString += "\r\n";
        }
        if (Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.WindowsEditor)
        {
            //Write to file
            string activeDir = Application.dataPath + @"/Client/Resources/Maps/";
            string newPath = System.IO.Path.Combine(activeDir, fileName);
            StreamWriter sw = new StreamWriter(newPath);
            sw.Write(saveString);
            sw.Close();
            UnityEditor.AssetDatabase.Refresh();
        }
    }
}
#endif