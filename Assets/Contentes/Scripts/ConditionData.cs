using UnityEngine;
using System.Collections;

public class ConditionData {

	[SerializeField] Define.Condition type;
	[SerializeField] int value;	// ex : poison damage
	[SerializeField] int turn;	// cure time
}
