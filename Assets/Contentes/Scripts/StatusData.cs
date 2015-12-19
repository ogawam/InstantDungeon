using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class StatusData {

	[SerializeField] int hp;
	[SerializeField] int mp;
	[SerializeField] int str;
	[SerializeField] int def;
	[SerializeField] int agi;
	[SerializeField] int lck;

	[SerializeField] List<ConditionData> conditions;
}
