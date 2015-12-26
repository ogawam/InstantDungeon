using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class StatusData {

	[SerializeField] int _hp;
	public int Hp { get { return _hp; } }
	[SerializeField] int _mp;
	[SerializeField] int _str;
	public int Str { get { return _str; } }
	[SerializeField] int _def;
	public int Def { get { return _def; } }
	[SerializeField] int _agi;
	public int Agi { get { return _agi; } }
	[SerializeField] int _lck;

	public void Damage(int damage) {
		_hp -= damage;
		if (_hp <= 0)
			_hp = 0;
	}

	public bool IsLive { get { return _hp > 0; } }
	public bool IsDead { get { return !IsLive; } }

	[SerializeField] List<ConditionData> _conditions;
	public void Copy(StatusData status) {
		_hp = status._hp;
		_mp = status._mp;
		_str = status._str;
		_def = status._def;
		_agi = status._agi;
		_lck = status._lck;
	}
}
