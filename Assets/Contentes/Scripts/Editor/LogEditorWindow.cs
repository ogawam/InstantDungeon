
using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;

class LogEditorWindow : EditorWindow {
	[MenuItem ("Window/Log")]

	public static void  ShowWindow () {
		EditorWindow.GetWindow(typeof(LogEditorWindow));
	}

	Vector2 scrl = Vector2.zero;
	int logIndex = 0;
	void OnGUI () {
		scrl = GUILayout.BeginScrollView (scrl, "box");

		var to = typeof(Development.Log);
		Development.Log[] types = Enum.GetValues (to) as Development.Log[];
		logIndex = GUILayout.Toolbar(logIndex, Enum.GetNames (to));
		Development.Log logType = types [logIndex];
		List<string> logs = Development.logs[logType];
		GUILayout.BeginVertical ();
		foreach (string log in logs)
			GUILayout.TextArea (log);
		GUILayout.EndVertical();
		GUILayout.EndScrollView ();

		if (GUILayout.Button ("clear"))
			logs.Clear ();
	}
}