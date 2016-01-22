using UnityEngine;
using System.Collections;
using System.Collections.Generic;

static public class Development {

	public enum Log {
		Action,
		View,
	};

	static public Dictionary<Log, List<string>> logs = new Dictionary<Log, List<string>>(){
		{ Log.Action, new List<string>() },
		{ Log.View, new List<string>() }
	};
}
