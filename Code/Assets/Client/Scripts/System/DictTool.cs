using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class DictTool
{
    private static Dictionary<string, string> dict = new Dictionary<string, string>();
    private static bool isInit = false;
    public static bool IsInit
    {
        get
        {
            return isInit;
        }
    }

    public static void Initialize()
    {
        if(isInit)
        {
            return;
        }
        dict.Clear();
#if DYNAMIC_UPDATE
        FileStream fs = File.OpenRead(Application.temporaryCachePath+"/config.txt");
        StreamReader sr = new StreamReader(fs, System.Text.Encoding.UTF8);
#else
        TextAsset ta = Resources.Load("config") as TextAsset;
        StringReader sr = new StringReader(System.Text.Encoding.UTF8.GetString(ta.bytes));
#endif
        char[] sp = { ',' };
        while (true)
        {
            string line = sr.ReadLine();
            if (line == null)
            {
                break;
            }
            line = line.Trim();
            if (string.IsNullOrEmpty(line))
            {
                continue;
            }
            string[] data = line.Split(sp, System.StringSplitOptions.RemoveEmptyEntries);
            if (data == null || data.Length != 2)
            {
                Debug.LogError("Language error. " + line);
                continue;
            }

            dict[data[0]] = data[1];
        }
        isInit = true;
        sr.Close();
#if DYNAMIC_UPDATE
        fs.Close();
#else
        Resources.UnloadAsset(ta);
#endif
    }

    public static void Reset()
    {
        isInit = false;
        Initialize();
    }
    public static string GetText(string key)
    {
        if (!dict.ContainsKey(key))
        {
            Debug.LogError("Language dont has this key:" + key);
            return "";
        }
        return dict[key];
    }

    private static Dictionary<int, string> dictNoUpdate = null;
    public static string GetTextNoUpdate(int key)
    {
        if (dictNoUpdate == null)
        {
            InitNoUpdate();
        }
        if (!dictNoUpdate.ContainsKey(key))
        {
            Debug.LogError("Language noupdate dont has this key:" + key);
            return "";
        }
        return dictNoUpdate[key];
    }

    private static void InitNoUpdate()
    {
        TextAsset ta = Resources.Load("languageNoUpdate") as TextAsset;
        StringReader sr = new StringReader(System.Text.Encoding.UTF8.GetString(ta.bytes));
        dictNoUpdate = new Dictionary<int, string>();
        char[] sp = { ',' };
        while (true)
        {
            string line = sr.ReadLine();
            if (line == null)
            {
                break;
            }
            line = line.Trim();
            if (string.IsNullOrEmpty(line))
            {
                continue;
            }
            string[] data = line.Split(sp, System.StringSplitOptions.RemoveEmptyEntries);
            if (data == null || data.Length != 2)
            {
                Debug.LogError("Language error. " + line);
                continue;
            }

            dictNoUpdate[int.Parse(data[0])] = data[1];
        }
        isInit = true;
        sr.Close();
        Resources.UnloadAsset(ta);
    }

}
