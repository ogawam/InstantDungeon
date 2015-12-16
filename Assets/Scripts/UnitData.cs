using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// todo : serialize for save
[System.Serializable]
public class UnitData {

	int hp;
	int mp;
	int str;
	int def;
	int agi;
	int lck;

	Define.Side side;
	List<ConditionData> conditions;
}
