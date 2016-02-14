using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class StageManager : Utility.Singleton<StageManager> {

	[SerializeField] RectTransform _backGround;
	[SerializeField] RectTransform _mainGround;

	[SerializeField] List<Define.Chip> _chipPriority;

	Vector3[,] _positions = new Vector3[Define.StageWidth, Define.StageDepth];
	ChipController[,] _chips = new ChipController[Define.StageWidth, Define.StageDepth];
	UnitController[,] _units = new UnitController[Define.StageWidth, Define.StageDepth];
	List<PopController>[,] _pops = new List<PopController>[Define.StageWidth, Define.StageDepth];

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
				foreach (PopController pop in _pops[x, z])
					pop.Remove ();
				_pops [x, z].Clear();
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
		unit.UnitView.transform.SetParent (_mainGround, false);
		unit.UnitView.transform.localPosition = position;
		for (int i = 0; i < _mainGround.childCount; ++i) {
			if (position.y > _mainGround.GetChild (i).transform.localPosition.y) {
				unit.UnitView.transform.SetSiblingIndex (i);
				break;
			}
		}								
		_units[x, z] = unit;
	}

	public void SetPop(PopController pop) {
		int x = pop.x;
		int z = pop.z;
		Vector3 position = _positions [x, z];
		pop.PopView.transform.SetParent (_mainGround, false);
		pop.PopView.transform.localPosition = position;
		for (int i = 0; i < _mainGround.childCount; ++i) {
			if (position.y > _mainGround.GetChild (i).transform.localPosition.y) {
				pop.PopView.transform.SetSiblingIndex (i);
				Debug.Log ("index "+ i);
				break;
			}
		}								
		_pops[x, z].Add(pop);
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
		bool isSkip = false;
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
						GameManager.Instance.Open (unit, rideUnit);
						int removeBegin = _orderedUnits.IndexOf (unit) + 1;
						int removeCount = _orderedUnits.Count - removeBegin;
						_orderedUnits.RemoveRange(removeBegin, removeCount);
						isSkip = true;
					}
					else unit.CalcCommandResult (GetChip(x, z), null);
					break;
				case Define.Unit.Hero:
				case Define.Unit.Monster:
					if (rideUnit.IsRecievable && unit.Side != rideUnit.Side) {
						List<UnitController> receivers = new List<UnitController> (){ rideUnit };
						GameManager.Instance.Action (unit, receivers);
					}
					else unit.CalcCommandResult (GetChip(x, z), null);	// create move action ? 
					break;
				}
			} 
			else {
				unit.CalcCommandResult (chip, null);	// create move action ? 
			}
			_units [x, z] = unit;
			if (isSkip)
				break;
		}
	}

	public IEnumerator ExecTo() {
		int prevAgi = 0;
		List<Coroutine> coroutines = new List<Coroutine>(); 
		foreach (UnitController unit in _orderedUnits) {
			// move action
			CommandResultData commandResult = unit.CommandResult;
			if (commandResult != null) {
				if (commandResult.chip != null) {
					if(unit.x != commandResult.chip.x || unit.z != commandResult.chip.z)
						coroutines.Add(StartCoroutine(unit.DoMove (commandResult.chip)));
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
								if (receiver.dropPop != null) {
									StartCoroutine (receiver.dropPop.Appear ());
									receiver.dropPop = null;
								}
								if (receiver.dropUnit != null) {
									StartCoroutine(receiver.dropUnit.Appear ());
									receiver.dropUnit = null;
								}
								StageManager.Instance.RemoveUnit (receiver);
							}

							// Effect
							yield return new WaitForSeconds(0.1f);
						}
					}
				}
			}
			if (unit.UnitActiveData.CalcStatus.Agi != prevAgi) {
				prevAgi = unit.UnitActiveData.CalcStatus.Agi;
				yield return new WaitForSeconds(0.005f);
			}
		}
		_unitsTo.Clear ();
		foreach(Coroutine coroutine in coroutines)
			yield return coroutine;

		Development.LogAction ("clear units to");
	}

	public void Sort() {
		List<Transform> views = new List<Transform>(_mainGround.GetComponentsInChildren<Transform> ());
		views.Sort ((Transform l, Transform r) => (int)(r.localPosition.y - l.localPosition.y));
		for (int i = 0; i < views.Count; ++i)
			views [i].SetSiblingIndex (i);
	}

	void Awake() {
		Vector3 firstPosition = new Vector3 (
			-Define.ChipWidth * (Define.StageWidth - 1) / 2, 
			-Define.ChipDepth * (Define.StageDepth - 1) / 2, 0
		);
		for (int x = 0; x < Define.StageWidth; ++x) {
			for (int z = 0; z < Define.StageDepth; ++z) {
				_positions [x, z] = firstPosition + new Vector3 (Define.ChipWidth * x, Define.ChipDepth * z, 0);
				_pops [x, z] = new List<PopController> ();
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
