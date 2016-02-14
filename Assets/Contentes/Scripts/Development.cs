using UnityEngine;
using System.Collections;
using System.Collections.Generic;

static public class Development {

	public enum LogType {
		Action,
		View,
	};

	static public void Log(LogType type, string log) { 
		if(Define.isEditor) {
			if (logs.ContainsKey (type)) {
				logs [type].Add (log);
			}
		}
	}

	static public void LogAction(string log) {
		if(Define.isEditor) {
			Log (LogType.Action, log);
		}
	}

	static public Dictionary<LogType, List<string>> logs = new Dictionary<LogType, List<string>>(){
		{ LogType.Action, new List<string>() },
		{ LogType.View, new List<string>() }
	};
}
