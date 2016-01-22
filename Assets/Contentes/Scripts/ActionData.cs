using UnityEngine;
using System.Collections;

[System.Serializable]
public class ActionData {

	[SerializeField] Define.Method _method;
	public Define.Method Method { get { return _method; } }
	[SerializeField] Define.Ability _dependAbility;
	public Define.Ability DependAbility { get { return _dependAbility; } }
	[SerializeField] Define.Side _dependSide;
	public Define.Side DependSide { get { return _dependSide; } }
	[SerializeField] StatusData _action;
	public StatusData Action { get { return _action; } }
	[SerializeField] int _turn;
	public int Turn { get { return _turn; } }
}

public class ActionResultData {
	public ActionData action;
	public StatusData senderStatus;
	public StatusData receiverStatus;
}
