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
		if (x >= 0 && x < Define.StageWidth && 
			z >= 0 && z < Define.StageDepth)
			return _chips [x, z];
		return null;
	}

	public UnitController GetUnit(int x, int z) {
		if (x >= 0 && x < Define.StageWidth && 
			z >= 0 && z < Define.StageDepth)
			return _units [x, z];
		return null;
	}

	public void UnitTo(UnitController unit, ChipController chip) {
		Development.LogAction ("unit to "+ unit.name);
		_unitsTo.Add (unit, chip);
	}

	List<UnitController> _orderedUnits = new List<UnitController>();
	public void CalcTo() {
		_orderedUnits.Clear ();
		_orderedUnits.AddRange(_unitsTo.Keys.OrderBy (elem => -elem.UnitMasterData.Status.Agi).ToArray());
		foreach(UnitController unit in _orderedUnits) {
			if (unit.UnitActiveData.CalcStatus.IsDead)
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

				switch (rideUnit.UnitType) {
				case Define.Unit.Treasure:
					// TODO enable open enemy thief
					if (unit.UnitType == Define.Unit.Hero) {
						GameManager.Instance.Open (rideUnit, unit);
					}
					break;
				case Define.Unit.Hero:
				case Define.Unit.Monster:
					if (rideUnit.IsRecievable && unit.Side != rideUnit.Side)
						GameManager.Instance.Action (unit, new List<UnitController>(){ rideUnit });
					else unit.CalcCommandResult (GetChip(x, z), null);	// create move action ? 
					break;
				}
			} 
			else {
				unit.CalcCommandResult (chip, null);	// create move action ? 
			}
			_units [x, z] = unit;
		}
	}

	public IEnumerator ExecTo() {
		foreach (UnitController unit in _orderedUnits) {
			// move action
			CommandResultData commandResult = unit.CommandResult;
			if (commandResult != null) {
				if (commandResult.chip != null) {
					unit.MoveTo (commandResult.chip);
				} else {
					// todo Motion 

					foreach (UnitController receiver in commandResult.recievers.Keys) {
						List<ActionResultData> results = commandResult.recievers[receiver];
						foreach(ActionResultData result in results) {
							// Reaction
							StatusData prevStatus = receiver.UnitActiveData.CalcStatus;
							StatusData nextStatus = result.receiverStatus;
							int hpDiff = nextStatus.Hp - prevStatus.Hp;

							Development.LogAction ("hp diff "+ hpDiff);

							receiver.UnitActiveData.CalcStatus = nextStatus;

							// Damage
							if (hpDiff < 0) {
								receiver.Reaction ();
							}

							if (receiver.UnitActiveData.CalcStatus.IsDead) {
								StageManager.Instance.RemoveUnit (receiver);
/*
								if (Random.value < 0.1f) {
									UnitController unitController = CreateUnit ("Treasure", Define.Unit.Treasure, Define.Side.Party);
									unitController.transform.SetParent (_unitRoot.transform);
									unitController.x = defence.x;
									unitController.z = defence.z;
									_objectUnits.Add (unitController);
									StageManager.Instance.SetUnit (unitController);
								}
*/								
//								defence.UnitView.gameObject.SetActive(false);
							}

							// Effect
							yield return new WaitForSeconds(0.1f);
						}
					}
				}
			}
			yield return new WaitForSeconds(0.025f);
		}
		_unitsTo.Clear ();
		Development.LogAction ("clear units to");
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
