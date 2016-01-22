using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class CommandData {

	[SerializeField] string _name;
	public string Name { get { return _name; } }
	[SerializeField] int _ready;
	public int Ready { get { return _ready; } }
	[SerializeField] int _range;
	public int Range { get { return _range; } }
	[SerializeField] int _scope;
	public int Scope { get { return _scope; } }
	[SerializeField] Define.Nature _nature;
	public Define.Nature Nature { get { return _nature; } }
	[SerializeField] Define.Element _element;
	public Define.Element Element { get { return _element; } }
	[SerializeField] ActionData[] _actions;
	public ActionData[] Actions { get { return _actions; } }
}

public class CommandResultData {
	public UnitController sender = null;
	public ChipController chip = null;
	public StatusData moveStatus = null;
	public Dictionary<UnitController, List<ActionResultData>> recievers = new Dictionary<UnitController, List<ActionResultData>>();
}