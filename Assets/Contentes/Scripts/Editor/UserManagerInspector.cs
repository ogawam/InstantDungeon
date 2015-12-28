using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(UserManager))]
public class UserManagerInspector : Editor {

	public override void OnInspectorGUI()
	{
		UserManager instance = target as UserManager;
		if (GUILayout.Button ("Save")) {
			instance.Save ();
		}

		if(GUILayout.Button ("Load")) {
		}

		if (GUILayout.Button ("Clear")) {
		}
	}
}
