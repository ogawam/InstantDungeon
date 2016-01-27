using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class UnitActiveData {

	StatusData _baseStatus = new StatusData();
	public StatusData BaseStatus { get { return _baseStatus; } } 

	StatusData _nextStatus = new StatusData();
	public StatusData NextStatus { 
		get { return _nextStatus; } 
		set { _nextStatus.Copy (value); } 
	}

	[SerializeField] List<ItemMasterData> _equipItems = new List<ItemMasterData>();
	public StatusData EquipStatus { get {
			StatusData result = new StatusData (_baseStatus);
			foreach (ItemMasterData item in _equipItems)
				result += item.Status;
			return result;
		}
	}

	public void Update() {
		_baseStatus.Update (_nextStatus);
		CalcStatus = NextStatus = EquipStatus;
	}

	StatusData _calcStatus = new StatusData();
	public StatusData CalcStatus { 
		get { return _calcStatus; } 
		set { _calcStatus.Copy(value); } 
	}

	public void Setup(StatusData status) {
		_baseStatus.Copy(status);
		CalcStatus = NextStatus = EquipStatus;
	}

	public int ActionPointBonus { 
		get { return CalcStatus.Agi < GameManager.Instance.Master.AgiOnceToTwice ? 5 :
			CalcStatus.Agi < GameManager.Instance.Master.AgiTwiceAtTime ? 10 : 20; }
	}

	public List<ActionResultData> CalcCommandResult(UnitActiveData receiver, CommandData command) {
		List<ActionResultData> result = new List<ActionResultData>();
		switch (command.Nature) {
		case Define.Nature.Physical:
			// todo shield skill
			break;
		case Define.Nature.Magical:
			// todo barrier skill
			break;
		}

		foreach(ActionData action in command.Actions) {
			ActionResultData actionResult = StatusData.CalcActionResult (
				CalcStatus, receiver.CalcStatus, 
				action, command.Nature, command.Element);
			result.Add(actionResult);
			receiver.CalcStatus = actionResult.receiverStatus;
			CalcStatus = actionResult.senderStatus;
		}
		return result;
	}

	public UnitActiveData(UnitMasterData masterData) {
		Setup (masterData.Status);
	}
}
