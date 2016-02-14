using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UnitController : MonoBehaviour {

	UnitMasterData _unitMasterData = null;
	public UnitMasterData UnitMasterData { get { return _unitMasterData; } }
	UnitActiveData _unitActiveData = null;
	public UnitActiveData UnitActiveData { get { return _unitActiveData; } } 
	UnitView _unitView = null;
	public UnitView UnitView { get { return _unitView; } }
	HudView _hudView = null;

	public PopController dropPop = null;
	public UnitController dropUnit = null;
	public UnitController openUnit = null;
	public ItemMasterData dropItem = null;

	Define.Unit _unitType;
	public Define.Unit UnitType { get { return _unitType; } }
	public bool IsRecievable { get { 
		return _unitType != Define.Unit.Wall; // or brake by pickaxe
	} }

	Define.Side _side;
	public Define.Side Side { get { return _side; } }

	Dictionary<Define.Region, ItemMasterData> _equipItems = new Dictionary<Define.Region, ItemMasterData>();

	public bool IsEnableAction { 
		get { return UnitActiveData.CalcStatus.IsLive && UnitActiveData.CalcStatus.ap >= 10; } 
	}

	public int x { 
		get { return UnitActiveData.CalcStatus.x; } 
		set { UnitActiveData.BaseStatus.x = UnitActiveData.CalcStatus.x = value; } 
	}
	public int z { 
		get { return UnitActiveData.CalcStatus.z; } 
		set { UnitActiveData.BaseStatus.z = UnitActiveData.CalcStatus.z = value; } 
	}

	public void Setup(UnitMasterData unitMasterData, Define.Unit unitType, Define.Side side) {
		_side = side;
		_unitType = unitType;
		Setup (unitMasterData, new UnitActiveData(unitMasterData));
	}
		
	public void Setup(UnitMasterData unitMasterData, UnitActiveData unitActiveData) {
		_unitMasterData = unitMasterData;
		_unitActiveData = unitActiveData;

		MasterData master = GameManager.Instance.Master;
		string viewPath = "Prefabs/" + _unitMasterData.ViewName;
//		Debug.Log ("Load "+ viewPath);
		_unitView = Instantiate(Resources.Load<UnitView>(viewPath));
		if (IsRecievable) {
			_hudView = InterfaceManager.Instance.CreateHudView (_unitActiveData.BaseStatus.Hp);
			ResetTurn ();
		}
	}

	public void Equip(Define.Region region, ItemMasterData itemData) {
		if (_equipItems.ContainsKey (region)) {
			// reject item 
		}
		_equipItems[region] = itemData;
		_unitView.EquipItem (region, itemData != null ? itemData.ViewSprite : null);
	}

	public void CalcCommandResult(ChipController chip, CommandData command) {
		_commandResultData = new CommandResultData () { 
			sender = this, 
			chip = chip, 
			moveStatus = new StatusData (UnitActiveData.CalcStatus)
		};

		_commandResultData.moveStatus.x = chip.x;
		_commandResultData.moveStatus.z = chip.z;
		_commandResultData.moveStatus.ap -= 10;
		UnitActiveData.CalcStatus = _commandResultData.moveStatus;
	}

	// todo refactaling
	public void CalcCommandResult(List<UnitController> receivers, CommandData command) {
		Development.LogAction(UnitMasterData.UnitName + "." + command.Name);
		_commandResultData = new CommandResultData () { sender = this };
		foreach (UnitController receiver in receivers) {
			Development.LogAction("receiver " + receiver.UnitMasterData.UnitName);
			_commandResultData.recievers [receiver] = UnitActiveData.CalcCommandResult (receiver.UnitActiveData, command);
		}
	}

	public void CalcEnd() {
		UnitActiveData.NextStatus = UnitActiveData.CalcStatus;
		UnitActiveData.CalcStatus = UnitActiveData.EquipStatus;
	}

	public void ExecEnd() {
		UnitActiveData.Update ();
		_commandResultData = null;
	}

	public void NextTurn() {
		if (_unitActiveData.NextStatus.IsLive) {
			_unitActiveData.NextStatus.ap += _unitActiveData.ActionPointBonus;
		}
		UnitActiveData.Update ();
//		Debug.Log ("action point "+ _actionPoint);
	}

	public void ResetTurn() {
		_unitActiveData.NextStatus.ap = _unitActiveData.ActionPointBonus;
		UnitActiveData.Update ();
	}

	public void Reaction() {
		_hudView.SetHeartPoint (_unitActiveData.CalcStatus.Hp);
		_unitView.Damage (_unitActiveData.CalcStatus.IsDead);
	}

	public IEnumerator Open() {
		yield return StartCoroutine(_unitView.Open (dropItem.ViewSprite));
	}

	public IEnumerator Destruction() {
		yield return StartCoroutine(_unitView.Destruction ());
	}

	CommandResultData _commandResultData = null;
	public CommandResultData CommandResult { get { return _commandResultData; } }
		
	public IEnumerator DoMove(ChipController chipTo) {
		yield return StartCoroutine(_unitView.DoMove (chipTo.ChipView));
	}

	public IEnumerator Appear() {
		UnitView.AppearType type = UnitView.AppearType.Moment;
		if (UnitType != Define.Unit.Wall)
			type = UnitView.AppearType.Warp;
		yield return StartCoroutine(_unitView.Appear (type));
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(_hudView != null)
			_hudView.transform.localPosition = _unitView.transform.localPosition + Vector3.up * 80;
	}

	public void Remove() {
		Destroy (_unitView.gameObject);
		if(_hudView != null) 
			Destroy (_hudView.gameObject);
		Destroy (gameObject);
	}
}
