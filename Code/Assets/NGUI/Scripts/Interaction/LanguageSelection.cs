//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2015 Tasharen Entertainment
//----------------------------------------------

using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Turns the popup list it's attached to into a language selection list.
/// </summary>

[RequireComponent(typeof(UIPopupList))]
[AddComponentMenu("NGUI/Interaction/Language Selection")]
public class LanguageSelection : MonoBehaviour
{
	UIPopupList mList;

    public string[] languse;
    public string defalutLan;
	void Start ()
	{
		mList = GetComponent<UIPopupList>();


        mList.items.Clear();

        for (int i = 0, imax = languse.Length; i < imax; ++i)
            mList.items.Add(languse[i]);

        

		EventDelegate.Add(mList.onChange, OnChange);

		mList.value = Localization.language;
	}

	void OnChange ()
	{
		Localization.language = UIPopupList.current.value;
	}
}
