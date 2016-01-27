
using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;

class LogWindow : EditorWindow {
	[MenuItem ("Window/Log")]

	public static void  ShowWindow () {
		EditorWindow.GetWindow(typeof(LogWindow));
	}

	Vector2 scrl = Vector2.zero;
	int logIndex = 0;
	void OnGUI () {
		scrl = GUILayout.BeginScrollView (scrl, "box");

		var to = typeof(Development.LogType);
		Development.LogType[] types = Enum.GetValues (to) as Development.LogType[];
		logIndex = GUILayout.Toolbar(logIndex, Enum.GetNames (to));
		Development.LogType logType = types [logIndex];
		List<string> logs = Development.logs[logType];
		GUILayout.BeginVertical ();
		foreach (string log in logs) {
			GUILayout.BeginHorizontal ();
			if (GUILayout.Button ("c", GUILayout.MaxWidth(16))) {
			}
			GUILayout.Label (log);
			GUILayout.EndHorizontal ();
		}
		GUILayout.EndVertical();
		GUILayout.EndScrollView ();

		if (GUILayout.Button ("copy")) {
			EditorGUIUtility.systemCopyBuffer = "";
			foreach(string log in logs) {
				EditorGUIUtility.systemCopyBuffer += log;
			}
		}
		if (GUILayout.Button ("clear"))
			logs.Clear ();
	}
}