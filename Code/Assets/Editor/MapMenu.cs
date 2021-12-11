using UnityEngine;
using System.Collections;
using UnityEditor;
public static class MapMenu
{
	[MenuItem("UNICORN/Create Map Editor")]
	static public void AddMapEditor ()
	{
		GameObject go = new GameObject("MapEditor");
		MapEditor  editor = go.AddComponent<MapEditor>();
		editor.isEnabled = true;
	}
}
