using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class StageManager : Utility.Singleton<StageManager> {

	[SerializeField] RectTransform _backGround;
	[SerializeField] RectTransform _foreGround;

	[SerializeField] List<Define.Chip> _chipPriority;

	Vector3[,] _positions = new Vector3[Define.StageWidth, Define.StageDepth];
	ChipController[,] _chips = new ChipController[Define.StageWidth, Define.StageDepth];
	UnitController[,] _units = new UnitController[Define.StageWidth, Define.StageDepth];

	public UnitController FindUnit(ChipController chip) {
		if(chip != null)
			return _units [chip.x, chip.z];
		return null;
	}

	public void RemoveUnit(UnitController unit) {
		if (unit == _units [unit.x, unit.z])
			_units [unit.x, unit.z] = null;
	}

	public void Clear() {
		for(int x = 0; x < Define.StageWidth; ++x) {
			for(int z = 0; z < Define.StageDepth; ++z) {
				if(_chips[x,z] != null) 
					_chips [x, z].Remove();
				_chips [x, z] = null;
				_units [x, z] = null;
			}
		}
	}

	Dictionary<UnitController, ChipController> _unitsTo = new Dictionary<UnitController, ChipController>();

	public void SetChip(ChipController chip) {
		int x = chip.x;
		int z = chip.z;
		Vector3 position = _positions [x, z];
		chip.ChipView.transform.SetParent (_backGround, false);
		chip.ChipView.transform.localPosition = position;
		for (int i = 0; i < _backGround.childCount; ++i) {
			ChipController childChip = _backGround.GetChild (i).GetComponent<ChipController> ();
			if (childChip != null && _chipPriority.IndexOf(chip.ChipType) > _chipPriority.IndexOf(childChip.ChipType)) {
				chip.ChipView.transform.SetSiblingIndex (i);
				break;
			}
		}								
		_chips[x, z] = chip;
	}

	public void SetUnit(UnitController unit) {
		int x = unit.x;
		int z = unit.z;
		Vector3 position = _positions [x, z];
		unit.UnitView.Stop ();
		unit.UnitView.transform.SetParent (_foreGround, false);
		unit.UnitView.transform.localPosition = position;
		for (int i = 0; i < _foreGround.childCount; ++i) {
			if (position.y > _foreGround.GetChild (i).transform.localPosition.y) {
				unit.UnitView.transform.SetSiblingIndex (i);
				break;
			}
		}								
		_units[x, z] = unit;
	}

	public ChipController GetChip(int x, int z) {
		if (x >= 0 && x < Define.StageWidth && z >= 0 && z < Define.StageDepth)
			return _chips [x, z];
		return null;
	}

	public UnitController GetUnit(int x, int z) {
		if (x >= 0 && x < Define.StageWidth && z >= 0 && z < Define.StageDepth)
			return _units [x, z];
		return null;
	}

	public void UnitTo(UnitController unit, ChipController chip) {
		_unitsTo.Add (unit, chip);
	}

	public void CalcTo() {
		UnitController[] orderedUnits = _unitsTo.Keys.OrderBy (elem => -elem.UnitMasterData.Status.Agi).ToArray();
		foreach(UnitController unit in orderedUnits) {
			if (unit.UnitActiveData.Status.IsDead)
				continue;
			
			_units [unit.x, unit.z] = null;
			ChipController chip = _unitsTo [unit];
			UnitController rideUnit = _units [chip.x, chip.z];
			int x = chip.x;
			int z = chip.z;
			// action to unit
			if (rideUnit != null) {
				x = unit.x;
				z = unit.z;

				if (rideUnit.IsRecievable && unit.Side != rideUnit.Side) {
					GameManager.Instance.Attack (unit, rideUnit);
				}
			} 
			// move to chip
			else {
			}
			if (unit.x != x || unit.z != z) {
				unit.x = x;
				unit.z = z;
				// todo on ExecTo
				unit.MoveTo (chip);
			}
			unit.Action ();
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

	// Update is called once per frame
	void Update () {
	}
}
