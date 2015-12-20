using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class StageManager : Utility.Singleton<StageManager> {

	[SerializeField] RectTransform _stageRoot;

	Vector3[,] _positions = new Vector3[Define.StageWidth, Define.StageDepth];
	ChipController[,] _chips = new ChipController[Define.StageWidth, Define.StageDepth];
	UnitController[,] _units = new UnitController[Define.StageWidth, Define.StageDepth];

	public UnitController FindUnit(ChipController chip) {
		if(chip != null)
			return _units [chip.x, chip.z];
		return null;
	}

	Dictionary<UnitController, ChipController> _unitsTo = new Dictionary<UnitController, ChipController>();

	public void SetChip(int x, int z, ChipController chip) {
		chip.CreateView (_stageRoot, _positions [x,z]);
		chip.x = x;
		chip.z = z;
		_chips[x, z] = chip;
	}

	public void SetUnit(int x, int z, UnitController unit) {
		unit.CreateView (_stageRoot, _positions[x,z]);
		unit.x = x;
		unit.z = z;
		_units[x, z] = unit;
	}

	public ChipController GetChip(int x, int z) {
		if (x >= 0 && x < Define.StageWidth && z >= 0 && z < Define.StageDepth)
			return _chips [x, z];
		return null;
	}

	public void UnitTo(UnitController unit, ChipController chip) {
		_unitsTo.Add (unit, chip);
	}

	public void CalcTo() {
		UnitController[] orderedUnits = _unitsTo.Keys.OrderBy (elem => -elem.UnitMasterData.Status.Agi).ToArray();
		foreach(UnitController unit in orderedUnits) {
			_units [unit.x, unit.z] = null;
			ChipController chip = _unitsTo [unit];
			UnitController rideUnit = _units [chip.x, chip.z];
			int x = chip.x;
			int z = chip.z;
			// action to unit
			if (rideUnit != null) {
				x = unit.x;
				z = unit.z;
			} 
			// move to chip
			else {
			}
			if (unit.x != x || unit.z != z) {
				unit.x = x;
				unit.z = z;
				unit.MoveTo (chip);
			}
			_units [x, z] = unit;
		}
		_unitsTo.Clear ();
	}

	public void ExecTo() {
		
	}

	void Awake() {
		Vector3 firstPosition = new Vector3 (
			-Define.ChipWidth * (Define.StageWidth - 1) / 2, 
			-Define.ChipDepth * (Define.StageDepth - 1) / 2, 0
		);
		for (int x = 0; x < Define.StageWidth; ++x) {
			for (int z = 0; z < Define.StageDepth; ++z) {
				_positions [x, z] = firstPosition + new Vector3 (Define.ChipWidth * x, Define.ChipDepth * z, 0);
			}
		}
	}

	// Use this for initialization
	void Start () {
	}

	public bool sort = true;
	// Update is called once per frame
	void Update () {
		if (sort) {
			RectTransform[] children = _stageRoot.GetComponentsInChildren<RectTransform> ();
			RectTransform[] ordered = children.OrderBy (elem => -elem.localPosition.y).ToArray ();
			for (int i = 0; i < ordered.Length; ++i) {
				ordered [i].SetSiblingIndex (i);
			}
		}
	}
}
