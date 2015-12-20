using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// todo : serialize for save
[System.Serializable]
public class UnitMasterData {

	[SerializeField] string _unitName;
	public string UnitName { get { return _unitName; } }

	[SerializeField] string _viewName;
	public string ViewName { get { return _viewName; } }

	[SerializeField] StatusData _status;
	public StatusData Status { get { return _status; } }
}
