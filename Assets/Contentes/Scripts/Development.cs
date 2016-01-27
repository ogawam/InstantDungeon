using UnityEngine;
using System.Collections;
using System.Collections.Generic;

static public class Development {

	public enum LogType {
		Action,
		View,
	};

	static public void Log(LogType type, string log) { 
#if UNITY_EDITOR		
		if (logs.ContainsKey (type)) {
			logs [type].Add (log);
		}
#endif
	}

	static public void LogAction(string log) {
#if UNITY_EDITOR		
		Log (LogType.Action, log);
#endif
	}

#if UNITY_EDITOR		
	static public Dictionary<LogType, List<string>> logs = new Dictionary<LogType, List<string>>(){
		{ LogType.Action, new List<string>() },
		{ LogType.View, new List<string>() }
	};
#endif
}
