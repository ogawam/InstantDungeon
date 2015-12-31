using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(UserManager))]
public class UserManagerInspector : Editor {

	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI ();
		UserManager instance = target as UserManager;
		if (GUILayout.Button ("Save")) {
			instance.Save ();
		}

		if(GUILayout.Button ("Load")) {
			instance.Load ();
		}

		if (GUILayout.Button ("Clear")) {
			instance.Clear ();
		}
	}
}
