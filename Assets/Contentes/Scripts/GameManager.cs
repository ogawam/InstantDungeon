using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GameManager : Utility.Singleton<GameManager> {

	[SerializeField] string[] _monsterNames;

	enum CheckResult {
		PlayerInput,
		EnemyAction,
		AdvanceTurn,
	};

	GameObject _chipRoot = null;
	GameObject _unitRoot = null;

	MasterData _master = null;
	public MasterData Master { get { return _master; } }

	int _floorNo = 0;
	int _turn = 1;
	bool _inExec = false;

	ItemMasterData[] _holdItems = new ItemMasterData[Define.ItemHolderMax]; 

	UnitController _heroUnit = null;
	List<UnitController> _monsterUnits = new List<UnitController>();
	List<UnitController> _objectUnits  = new List<UnitController>();

	public void Click(ChipController chip) {
		if (_heroUnit.IsEnableAction) {
			int selectItemHolderIndex = InterfaceManager.Instance.SelectItemHolderIndex;
			if (selectItemHolderIndex < 0)
				return;

			ItemMasterData itemMasterData = _holdItems [selectItemHolderIndex];
			if (itemMasterData != null) {
				if (itemMasterData.EquipRegion != Define.Region.None
				    && StageManager.Instance.GetUnit (chip.x, chip.z) == _heroUnit) {
					_heroUnit.Equip (itemMasterData.EquipRegion, itemMasterData);
					InterfaceManager.Instance.SetEquip (selectItemHolderIndex, true);
					Action (_heroUnit.x, _heroUnit.z);
				}
			}

			InterfaceManager.Instance.ClickItemHolder (null);
		}
	}

	Define.Direction _dragDirection = Define.Direction.Up;
	float _dragRate = 0;
	bool _isDragged = false;

	public void Drag(PointerEventData eventData) {

		if (_heroUnit.IsEnableAction) {
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
		} else {
			InterfaceManager.Instance.ArrowView.Hide ();
		}
	}

	public void DragEnd(PointerEventData eventData) {
		InterfaceManager.Instance.ArrowView.Hide ();
		if(_dragRate == 1) {
			_isDragged = true;
		}
	}

	public void Action(int x, int z) {
		if (_heroUnit.IsEnableAction) {
			StageManager sm = StageManager.Instance;

			ChipController chip = sm.GetChip (x, z);
			UnitController unit = sm.FindUnit (chip);

			if (chip != null && (unit == null || unit.IsRecievable)) {
				sm.UnitTo (_heroUnit, chip);
				StartCoroutine (ProgressAction ());
			}
		}
	}

	public void ProgressTurn() {
		_turn++;
		_heroUnit.NextTurn ();
		foreach (UnitController monsterUnit in _monsterUnits) {
			monsterUnit.NextTurn ();
		}
	}
		
	public IEnumerator ProgressAction() {
		_inExec = true;
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
		_heroUnit.CalcEnd ();
		foreach(UnitController monsterUnit in _monsterUnits)
			monsterUnit.CalcEnd ();

		yield return StartCoroutine(sm.ExecTo ());
		_heroUnit.ExecEnd ();
		foreach (UnitController unit in _monsterUnits)
			unit.ExecEnd ();
		sm.Sort ();
		
		if (sm.GetChip (_heroUnit.x, _heroUnit.z).ChipType == Define.Chip.Stairs) {
			CreateMap (_floorNo + 1);
		}
		_inExec = false;

		switch (Check ()) {
		case CheckResult.PlayerInput:
			break;
		case CheckResult.EnemyAction:
			yield return StartCoroutine (ProgressAction());
			break;
		case CheckResult.AdvanceTurn:
			ProgressTurn ();
			break;
		}
	}

	public void Skip() {
		Action (_heroUnit.x, _heroUnit.z);
		StartCoroutine(InterfaceManager.Instance.Skip ());
	}

	// todo refactaling
	public void Action(UnitController sender, List<UnitController> receivers) {
		Development.LogAction ("-----------------------------");
		sender.CalcCommandResult(receivers, _master.FindCommandData("Attack"));
		foreach(UnitController receiver in receivers) {
			if (receiver.UnitActiveData.CalcStatus.GetCondition (Define.Condition.Dead) != null) {
				StageManager.Instance.RemoveUnit (receiver);
			}
		}
	}

	public void Open(UnitController targetUnit, UnitController actionUnit) {
				
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

		for (int i = 0; i < Random.Range (0, 64); ++i) {
			int sandLot = Random.Range (0, blankChips.Count);
			int sandPos = blankChips[sandLot];
			entryChips.Add(sandPos, Define.Chip.Sand);
			blankChips.Remove (sandPos);
		}

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
		ChipController stairChip = StageManager.Instance.GetChip (stairsPos % 100, stairsPos / 100);
		stairChip.ChipView.transform.SetSiblingIndex (stairChip.ChipView.transform.parent.childCount);

		foreach (int posKey in entryUnits.Keys) {
			Define.Unit unitType = entryUnits [posKey];
			int x = posKey % 100;
			int z = posKey / 100;

			UnitController unitController = null;
			switch (unitType) {
			case Define.Unit.Hero:
				unitController = _heroUnit;
//				unitController.UnitView.EquipItem (Define.Region.Head, Random.value < 0.5f ? _master.FindItemData("BronzeHelm").ViewSprite: null);
//				unitController.UnitView.EquipItem (Define.Region.Body, Random.value < 0.5f ? _master.FindItemData("BronzeArmor").ViewSprite: null);
				unitController.ResetTurn ();
				break;
			case Define.Unit.Wall:
				unitController = CreateUnit ("Wall", unitType, Define.Side.Neutral);
				_objectUnits.Add (unitController);
				break;
			case Define.Unit.Monster:
				unitController = CreateUnit (_monsterNames [Random.Range (0, _monsterNames.Count ())], unitType, Define.Side.Enemy);
				if(Random.value < 0.25f) unitController.UnitView.EquipItem (Define.Region.Head, _master.FindItemData("BronzeHelm").ViewSprite);
				if(Random.value < 0.25f) unitController.UnitView.EquipItem (Define.Region.Body, _master.FindItemData("BronzeArmor").ViewSprite);
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

	static int totalNo = 0;
	private UnitController CreateUnit(string unitName, Define.Unit unitType, Define.Side side) {
		UnitController result = new GameObject ().AddComponent<UnitController> ();
		UnitMasterData unitMaster = _master.FindUnitData (unitName);
		result.Setup (unitMaster, unitType, side);
		result.name = System.String.Format("{0,2:D2}:{1}",totalNo.ToString(), unitName);
		result.UnitView.name = result.name + "View";

		totalNo++;
		return result;
	}

	private CheckResult Check() {
		if (_heroUnit.IsEnableAction)
			return CheckResult.PlayerInput;
		foreach (UnitController monsterUnit in _monsterUnits)
			if (monsterUnit.IsEnableAction)
				return CheckResult.EnemyAction;
		return CheckResult.AdvanceTurn;
	}

	// Use this for initialization
	void Start () {
		
		_master = Resources.Load<MasterData> ("Datas/MasterData");

		_chipRoot = new GameObject ("ChipRoot");
		_chipRoot.transform.SetParent (transform);

		_unitRoot = new GameObject ("UnitRoot");
		_unitRoot.transform.SetParent (transform);

		_heroUnit = CreateUnit("Hero", Define.Unit.Hero, Define.Side.Party);

		_holdItems [0] = _master.FindItemData ("BronzeHelm");
		_holdItems [4] = _master.FindItemData ("BronzeArmor");
		for(int i = 0; i < _holdItems.Length; ++i)
			InterfaceManager.Instance.SetHolderItem(i, _holdItems[i]);

		CreateMap (1);

		switch (Check ()) {
		case CheckResult.PlayerInput:
			break;
		case CheckResult.EnemyAction:
			StartCoroutine (ProgressAction());
			break;
		case CheckResult.AdvanceTurn:
			ProgressTurn ();
			break;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (_isDragged && !_inExec) {
			int x = _heroUnit.x;
			int z = _heroUnit.z;

			switch (_dragDirection) {
			case Define.Direction.Up:
				z++;
				break;
			case Define.Direction.Down:
				z--;
				break;
			case Define.Direction.Left:
				x--;
				break;
			case Define.Direction.Right:
				x++;
				break;
			}

			Action (x, z);
			_isDragged = false;
		}
#if !UNITY_EDITOR
		Vector2 gravity2D = new Vector2(Input.acceleration.x, Input.acceleration.y).normalized;
		Physics2D.gravity = gravity2D * Physics2D.gravity.magnitude;
#endif
	}

	void OnGUI() {
		#if UNITY_EDITOR
		GUILayout.Label ("turn "+ _turn + " dragged " + _isDragged);
		GUILayout.Label (_heroUnit.name + " ap " + _heroUnit.UnitActiveData.BaseStatus.ap);
		foreach (UnitController monsterUnit in _monsterUnits)
			GUILayout.Label (monsterUnit.name + " ap "+ monsterUnit.UnitActiveData.BaseStatus.ap);
		#endif
	}
}
