using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class StatusData {

	[SerializeField] int _hp;
	public int Hp { get { return _hp; } }
	[SerializeField] int _mp;
	[SerializeField] int _str;
	[SerializeField] int _def;
	[SerializeField] int _agi;
	public int Agi { get { return _agi; } }
	[SerializeField] int _lck;

	[SerializeField] List<ConditionData> _conditions;
}
