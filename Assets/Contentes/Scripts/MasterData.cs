using UnityEngine;
using System.Collections;
using System.Linq;

public class MasterData : ScriptableObject {

	[SerializeField] UnitMasterData[] _unitMasterData;
	public UnitMasterData FindMasterData(string name) {
		return _unitMasterData.First (unit => unit.UnitName == name);
	}
}
