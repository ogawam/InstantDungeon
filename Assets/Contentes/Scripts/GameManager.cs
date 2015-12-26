using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GameManager : Utility.Singleton<GameManager> {

	GameObject _chipRoot = null;
	GameObject _unitRoot = null;

	MasterData _master = null;
	public MasterData Master { get { return _master; } }

	int _floorNo = 0;

	int _turn = 1;

	UnitController _heroUnit = null;
	List<UnitController> _monsterUnits = new List<UnitController>();
	List<UnitController> _objectUnits  = new List<UnitController>();

	public void Click(ChipController chip) {
/*
		StageManager sm = StageManager.Instance;
		UnitController unit = sm.FindUnit (chip);
		if (unit == null) {
			sm.UnitTo (_heroUnit, chip);
			sm.CalcTo ();
			sm.ExecTo ();
		}
*/		
	}

	Define.Direction _dragDirection = Define.Direction.Up;
	float _dragRate = 0;

	public void Drag(PointerEventData eventData) {
//		Debug.Log ("delta " + (eventData.position - eventData.pressPosition));
		Vector3 slide = eventData.position - eventData.pressPosition;
		InterfaceManager.Instance.ArrowView.Disp ();
		if (Mathf.Abs (slide.x) > Mathf.Abs (slide.y)) {
			_dragDirection = slide.x > 0 ? Define.Direction.Right : Define.Direction.Left;
			_dragRate = Mathf.Min (1, Mathf.Abs (slide.x) / Define.DragToMove);
		} else {
			_dragDirection = slide.y > 0 ? Define.Direction.Up : Define.Direction.Down;
			_dragRate = Mathf.Min (1, Mathf.Abs (slide.y) / Define.DragToMove);
		}
		InterfaceManager.Instance.ArrowView.Fill (_dragDirection, _dragRate);
		InterfaceManager.Instance.ArrowView.transform.localPosition = _heroUnit.UnitView.transform.localPosition;
	}

	public void DragEnd(PointerEventData eventData) {
		InterfaceManager.Instance.ArrowView.Hide ();

		if(_dragRate == 1) {
			int x = _heroUnit.x;
			int z = _heroUnit.z;

			switch (_dragDirection) {
			case Define.Direction.Up: z++; break;
			case Define.Direction.Down: z--; break;
			case Define.Direction.Left: x--; break;
			case Define.Direction.Right: x++; break;
			}

			Action (x, z);
		}
	}

	public void Action(int x, int z) {
		StageManager sm = StageManager.Instance;

		ChipController chip = sm.GetChip (x, z);
		UnitController unit = sm.FindUnit (chip);

		if (chip != null && (unit == null || unit.IsRecievable)) {
			sm.UnitTo (_heroUnit, chip);
			ProgressAction ();
			StartCoroutine (CheckAction ());
		}
	}

	public void ProgressTurn() {
		_turn++;
		_heroUnit.NextTurn ();
		foreach (UnitController monsterUnit in _monsterUnits) {
			monsterUnit.NextTurn ();
		}
	}
		
	public void ProgressAction() {
		StageManager sm = StageManager.Instance;
		foreach (UnitController monsterUnit in _monsterUnits) {
			if (!monsterUnit.IsEnableAction)
				continue;

			int xDiff = _heroUnit.x - monsterUnit.x;
			int zDiff = _heroUnit.z - monsterUnit.z;
			int x = monsterUnit.x;
			int z = monsterUnit.z;
			while (xDiff != 0 || zDiff != 0) {
				if (Mathf.Abs (xDiff) > Mathf.Abs (zDiff)) {
					if (xDiff > 0)
						x++;
					else
						x--;
					xDiff = 0;
				} else {
					if (zDiff > 0)
						z++;
					else
						z--;
					zDiff = 0;
				}

				UnitController rideUnit = sm.GetUnit (x, z);
				if (rideUnit == null || rideUnit.IsRecievable) {
					break;
				}
				x = monsterUnit.x;
				z = monsterUnit.z;
			}
			sm.UnitTo (monsterUnit, sm.GetChip (x, z));
		}

		sm.CalcTo ();
		sm.ExecTo ();
		if (sm.GetChip (_heroUnit.x, _heroUnit.z).ChipType == Define.Chip.Stairs) {
			CreateMap (_floorNo + 1);
		}
	}

	public void Skip() {
		Action (_heroUnit.x, _heroUnit.z);
		StartCoroutine(InterfaceManager.Instance.Skip ());
	}

	// todo refactaling
	public void Attack(UnitController offence, UnitController defence) {
		int attackPoint = offence.UnitActiveData.GetAttackPoint ();
		int defencePoint = defence.UnitActiveData.GetDefencePoint ();

		defence.UnitActiveData.RecieveDamage (Mathf.Max(1, attackPoint - defencePoint));
		if (defence.UnitActiveData.Status.IsDead) {
			StageManager.Instance.RemoveUnit (defence);
//			defence.UnitView.gameObject.SetActive(false);
		}
		defence.Reaction ();	// view
	}
		
	void CreateMap(int floorNo) {
		_floorNo = floorNo;
		InterfaceManager.Instance.DispFloor ("普通のダンジョン\nB"+floorNo+"F");
		StageManager.Instance.Clear ();

		foreach (UnitController monsterUnit in _monsterUnits)
			monsterUnit.Remove();
		_monsterUnits.Clear ();

		foreach (UnitController objectUnit in _objectUnits)
			objectUnit.Remove();
		_objectUnits.Clear ();

		List<int> blankChips = new List<int> ();
		List<int> blankUnits = new List<int> ();
		Dictionary<int, Define.Chip> entryChips = new Dictionary<int, Define.Chip> ();
		Dictionary<int, Define.Unit> entryUnits = new Dictionary<int, Define.Unit> ();
		for (int x = 0; x < Define.StageWidth; ++x) {
			for (int z = 0; z < Define.StageDepth; ++z) {
				int posKey = x + z * 100;
				blankChips.Add (posKey);
				blankUnits.Add (posKey);
			}
		}
		
		int stairsLot = Random.Range (0, blankChips.Count);
		int stairsPos = blankChips[stairsLot];
		entryChips.Add(stairsPos, Define.Chip.Stairs);
		blankChips.Remove (stairsPos);
		blankUnits.Remove (stairsPos);	// can't set on stair

		int heroLot = Random.Range(0, blankChips.Count);
		int heroPos = blankChips[heroLot];
		entryUnits.Add (heroPos, Define.Unit.Hero);
		blankUnits.Remove (heroPos);
		entryChips.Add(heroPos, Define.Chip.Flat);
		blankChips.Remove (heroPos);	// first chip is flat

		int wallNum = Random.Range (10, 20);	// todo stage data
		for (int i = 0; i < wallNum; ++i) {
			int wallLot = Random.Range (0, blankUnits.Count);
			int wallPos = blankUnits[wallLot];
			entryUnits.Add (wallPos, Define.Unit.Wall);
			blankUnits.Remove (wallPos);
		}

		int monsterNum = Random.Range (5, 10);	// todo stage data
		for (int i = 0; i < monsterNum; ++i) {
			int monsterLot = Random.Range (0, blankUnits.Count);
			int monsterPos = blankUnits[monsterLot];
			entryUnits.Add (monsterPos, Define.Unit.Monster);
			blankUnits.Remove (monsterPos);
		}

		foreach(int posKey in blankChips)
			entryChips [posKey] = Define.Chip.Flat;
		blankChips.Clear ();

		foreach(int posKey in entryChips.Keys) {
			Define.Chip chipType = entryChips [posKey];
			int x = posKey % 100;
			int z = posKey / 100;

			ChipController chipController = new GameObject (typeof(ChipController).ToString() + "["+ x + "]["+ z + "]").AddComponent<ChipController> ();
			chipController.transform.SetParent (_chipRoot.transform);
			chipController.Setup (_master.FindChipData (chipType.ToString()), chipType);	// todo data name to enum
			chipController.x = x;
			chipController.z = z;
			StageManager.Instance.SetChip (chipController);
		}	

		foreach (int posKey in entryUnits.Keys) {
			Define.Unit unitType = entryUnits [posKey];
			int x = posKey % 100;
			int z = posKey / 100;

			UnitController unitController = null;
			switch (unitType) {
			case Define.Unit.Hero:
				unitController = _heroUnit;
				unitController.ResetTurn ();
				break;
			case Define.Unit.Wall:
				unitController = CreateUnit ("Wall", unitType, Define.Side.Neutral);
				_objectUnits.Add (unitController);
				break;
			case Define.Unit.Monster:
				unitController = CreateUnit ("Slime", unitType, Define.Side.Enemy);
				unitController.ResetTurn ();
				_monsterUnits.Add (unitController);
				break;
			}
			unitController.transform.SetParent(_unitRoot.transform);
			unitController.x = x;
			unitController.z = z;
			StageManager.Instance.SetUnit(unitController);
		}
	}

	private UnitController CreateUnit(string unitName, Define.Unit unitType, Define.Side side) {
		UnitController result = new GameObject ().AddComponent<UnitController> ();
		UnitMasterData unitMaster = _master.FindUnitData (unitName);
		result.Setup (unitMaster, unitType, side);
		result.name = unitName;
		return result;
	}

	private IEnumerator CheckAction() {
		if (_heroUnit.IsEnableAction) {
			yield break;	// wait for input
		}
		bool isAction = false;
		foreach (UnitController monsterUnit in _monsterUnits) {
			if (monsterUnit.IsEnableAction) {
				isAction = true;
				break;
			}
		}
		if(isAction)
			ProgressAction ();
		else ProgressTurn ();
		yield return 0;

		yield return StartCoroutine(CheckAction ());
	}

	// Use this for initialization
	void Start () {
		
		_master = Resources.Load<MasterData> ("Datas/MasterData");
		_chipRoot = new GameObject ("ChipRoot");
		_chipRoot.transform.SetParent (transform);

		_unitRoot = new GameObject ("UnitRoot");
		_unitRoot.transform.SetParent (transform);

		_heroUnit = CreateUnit("Hero", Define.Unit.Hero, Define.Side.Party);

		CreateMap (1);
		StartCoroutine(CheckAction ());
	}
	
	// Update is called once per frame
	void Update () {
#if !UNITY_EDITOR
		Vector2 gravity2D = new Vector2(Input.acceleration.x, Input.acceleration.y).normalized;
		Physics2D.gravity = gravity2D * Physics2D.gravity.magnitude;
#endif
	}

	void OnGUI() {
		#if UNITY_EDITOR
		GUILayout.Label ("turn "+ _turn);
		GUILayout.Label (_heroUnit.name + " ap " + _heroUnit.ActionPoint);
		foreach (UnitController monsterUnit in _monsterUnits)
			GUILayout.Label (monsterUnit.name + " ap "+ monsterUnit.ActionPoint);
		#endif
	}
}
