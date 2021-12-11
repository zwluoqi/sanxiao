using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using GCGame.Table;

#if UNITY_EDITOR	
public class MapEditor : MonoBehaviour 
{
	
	public UIGrid grid;
	public UIScrollView scroll;
	public Transform editorPanel;
	private List<GameObject> editors ;
	

	public static MapEditor Instance				= null;
	
	//Chekc
	public bool isEnabled 							= false;	

	//Map is grid, so we will have one list of squares
	[HideInInspector]
	public List<UISquareView> squares 					= new List<UISquareView>();
	
	//List of object will be inactive when run map editor
	private List<GameObject> hiddenObjects 			= new List<GameObject>();
	
	//Array of map values
	private int[] map 								= new int[Map.maxCol*Map.maxRow];
	
	//Current tool user seclected
	private int currentTool 					= -1;
	
	//Check if current game mode is limit move or limit time
	//private bool isLimitMoveMode 					= true;
	//private bool isProcessGold						= false;
	
	private string fileName							="1.txt";//保存文件名;

	public static string tmpFileName = "";

	public static bool save = false;

	
	// Use this for initialization
	void Awake () 
	{
		Instance = this;
        LocalDataBase.InitCopyDatas();
		//If in editor and map editor enable, disable all game parts
		if(Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.WindowsEditor)
		{
			if(isEnabled)
			{
				
				if(string.IsNullOrEmpty(tmpFileName)){
					fileName = "1.txt";
					Reset();
				}else{
					fileName = tmpFileName;
					int level = int.Parse(fileName.Replace(".txt",""));
					LoadMap(level);
					
					//Write to file
					//如果不是保存的话,我们不用还原bak到正式txt中;
					if(!save){
						string activeDir = Application.dataPath + @"/Client/Resources/Maps/";
						string newPath = System.IO.Path.Combine(activeDir, fileName);
						string desPath = System.IO.Path.Combine(activeDir, level+".bak");
						if(File.Exists(desPath)){
							File.Copy(desPath,newPath,true);
						}				
						UnityEditor.AssetDatabase.Refresh();
					}
				}	
				///GenSquares();
			}
		}

	}
	
	void OnEnable(){
		Hashtable table_square = TableManager.GetSquare();
		editors = new List<GameObject>();
		foreach(DictionaryEntry dic in table_square){
			Tab_Square tab_square = (Tab_Square)dic.Value;
			GameObject obj = WidgetBufferManager.Instance.loadWidget("MapEditorView",grid.transform);
			obj.transform.Find("square").GetComponent<UISprite>().spriteName = tab_square.SpriteName;
			obj.name =  ((int)dic.Key).ToString();
			editors.Add(obj);
			UIEventListener.Get(obj).onClick += OnSelectedEditor;
		}
		scroll.horizontalScrollBar.value = 0;
		grid.repositionNow = true;
		
	}
	
	
	void OnSelectedEditor(GameObject go){
		int typeID = int.Parse(go.name);
		currentTool = typeID;
		SystemConfig.Log("selected:"+currentTool);
	}
	
	
	void OnDestroy(){
		Instance = null;
	}
	
	void ResetMapData(){
		//Set all map to 1 (mean all square not empty)
		for(int i=0; i<map.Length; i++)
		{
			map[i] = 1;
		}	
	}
	
	void GenSquares()
	{
		//Gen square by row and column
        bool bFlg = false;
		for(int row = 0; row< Map.maxRow; row++)
		{
            bool start = bFlg;
			for(int col=0; col<Map.maxCol; col++)
			{
                bFlg = !bFlg;
				GenSquare(row,col,bFlg);
			}
			bFlg = !start;
		}
		
		SystemConfig.Log("editorPanel childCount:"+editorPanel.childCount);
	}

	
	/// <summary>
	/// Generate the square at [row,column]
	/// </summary>
	/// <param name='row'>
	/// Row.
	/// </param>
	/// <param name='col'>
	/// Col.
	/// </param>
	void GenSquare(int row, int col, bool bFlg)
	{
		//Create new square at that row,column
		UISquareView square = null;
		//If at that point, map data is 1, gen non-empty item
		//otherwise, gen empty item
		int mapValue = map[row*Map.maxCol + col];
		
		int sType = mapValue;

		square = UISquareView.LoadSquare(row,col,sType,bFlg,
			editorPanel,true);
		//add square to squares list
		squares.Add(square);
	}	
	
	public void RemoveMap()
	{
		//Remove all squares
		for(int i=0; i<squares.Count; i++)
		{
			squares[i].RemoveAllObjects();
			squares[i].Destroy(true);
		}
		squares.Clear();
	}	
	
	// Update is called once per frame
	void OnGUI()
	{
		GUI.Label(new Rect(Screen.width-200,80,80,40),"File  Name");
		fileName = GUI.TextField(new Rect(Screen.width-100,80,80,30),fileName);		
		
		//Save and test
		if(GUI.Button(new Rect(Screen.width-200,120,160,40),"Save&Test"))
		{
			int level = int.Parse(fileName.Replace(".txt",""));

			if(!fileName.Contains(".txt")) fileName += ".txt";
			SaveMap(fileName);

			save = true;
			tmpFileName = fileName;
			Test(level);
		}
		
		//测试关卡;
		if(GUI.Button(new Rect(Screen.width-200,180,160,40),"Test")){	
			int level = int.Parse(fileName.Replace(".txt",""));
			if(!fileName.Contains(".txt")) fileName += ".txt";
			
			//Write to file
			string activeDir = Application.dataPath + @"/Client/Resources/Maps/";
			string newPath = System.IO.Path.Combine(activeDir, fileName);
			string desPath = System.IO.Path.Combine(activeDir, level+".bak");
			if(File.Exists(newPath)){
				File.Copy(newPath,desPath,true);
			}
			SaveMap(fileName);//待加载数据;

			save = false;
			tmpFileName = fileName;
			Test(level);
		}
		
		
		
		//载入关卡;
		if(GUI.Button(new Rect(Screen.width-200,240,160,40),"LoadMap")){
			int level = int.Parse(fileName.Replace(".txt",""));
			LoadMap(level);
		}
		
		//重置;
		if(GUI.Button(new Rect(Screen.width-200,300,160,40),"Reset")){
			Reset();
		}
		
		
	}
	//This function will be called in SquareObject when user click on
	//square in edit mode
	public void ClickOnSquare(UISquareView square)
	{
		if(currentTool == -1){
			return ;
		}

		//If current tool is erase, set square to empty
		//And remove all objects in square
		square.ChangeToOtherType(currentTool);


	}
	public void SaveMap(string fileName)
	{
		string saveString="";

		//set map data
		for(int row=0; row<Map.maxRow; row++)
		{
			for(int col=0; col<Map.maxCol; col++)
			{
				//Get square at [row,col]
				UISquareView square = squares[row*Map.maxCol+col];	
				//Save square type to save string
				saveString += square.tab_id;
				//if this column not yet end of row, add space between them
				if(col < (Map.maxCol-1)) saveString += " ";					
			}
			//if this row is not yet end of row, add new line symbol between rows
			if(row <(Map.maxRow-1)) saveString += "\r\n";
		}
		if(Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.WindowsEditor)
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
	
	public void Test(int  level){
		
		//Remove all squares
		RemoveMap();
		LocalDataBase.Instance().SetSelectCopyLevel(level);
		MapEditorOp.Instance.gameObject.SetActive(true);
		//StartCoroutine( AsynLoadLevel());
		Application.LoadLevel("sanxiao");
	}

	IEnumerator AsynLoadLevel(){
		AsyncOperation asynco = Application.LoadLevelAsync("sanxiao");
		yield return asynco;
	}
	
	public void LoadMap(int level){
		//Remove all squares
		RemoveMap();
		

		currentTool = -1;	
		LevelData.LoadDataFromLocal(level);
		map = LevelData.mapData;	
		GenSquares();
	}
	
	public void Reset(){
		//Remove all squares
		RemoveMap();

		currentTool = -1;	
		
		ResetMapData();
		GenSquares();
	}
	

}
#endif	