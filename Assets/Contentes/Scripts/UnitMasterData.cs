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

	[SerializeField] List<DropMasterData> _dropDatas;
	public List<DropMasterData> DropDatas { get { return _dropDatas; } }
}
	
[System.Serializable]
public class DropMasterData {
	[SerializeField] Define.DropType _dropType;
	public Define.DropType DropType { get { return _dropType; } }

	[SerializeField] Define.PopType _popType;
	public Define.PopType PopType { get { return _popType; } }

	[SerializeField] string _itemName;
	public string ItemName { get { return _itemName; } }

	[SerializeField] int _value;
	public int Value { get { return _value; } }

	[SerializeField] int _rate;
	public int Rate { get { return _rate; } }
}