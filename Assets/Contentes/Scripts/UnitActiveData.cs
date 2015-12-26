using UnityEngine;
using System.Collections;

public class UnitActiveData {

	StatusData _status;
	public StatusData Status { get { return _status; } } 
	public void Setup(StatusData status) {
		_status.Copy(status);
	}
	public UnitActiveData(UnitMasterData masterData) {
		_status = new StatusData ();
		_status.Copy(masterData.Status);
	}

	// todo calc chip buff
	public int GetAttackPoint() {
		return _status.Str;
	}

	// todo calc chip buff
	public int GetDefencePoint() {
		return _status.Def;
	}

	public void RecieveDamage(int damage) {
		_status.Damage (damage);
	}

	public int ActionPoint { 
		get { 
			if (_status.Agi < GameManager.Instance.Master.AgiOnceToTwice)
				return 5;
			else if (_status.Agi < GameManager.Instance.Master.AgiTwiceAtTime)
				return 10;
			return 20;
		}
	}
}
