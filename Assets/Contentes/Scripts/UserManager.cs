using UnityEngine;
using System.Collections;

public class UserManager : Utility.Singleton<UserManager> {

	UserData _userData;

	public void Save() {
		string json = JsonUtility.ToJson (_userData);
		Debug.Log (json);
	}

	// Use this for initialization
	void Start () {
		_userData = new UserData ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
