using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class UserDataItem {
	string _name;
	int _count;
};
	
[System.Serializable]
public class UserData {

	[SerializeField] List<UserDataItem> _depotItems = new List<UserDataItem> ();

	[SerializeField] List<string> _holdItems = new List<string>();

	[SerializeField] List<string> _libraryItems = new List<string> ();

	public UserData() {
	}
}
