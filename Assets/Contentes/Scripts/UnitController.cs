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

	Define.Unit _unitType;
	public Define.Unit UnitType { get { return _unitType; } }
	public bool IsRecievable { get { 
		return _unitType != Define.Unit.Wall; // or brake by pickaxe
	} }

	Define.Side _side;
	public Define.Side Side { get { return _side; } }

	Dictionary<Define.Region, ItemMasterData> _equipItems = new Dictionary<Define.Region, ItemMasterData>();

	private int _actionPoint = 0;
	public int ActionPoint { get { return _actionPoint; } } 
	public bool IsEnableAction { get { return UnitActiveData.Status.IsLive && _actionPoint >= 10; } }

	public int x;
	public int z;

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
			_hudView = InterfaceManager.Instance.CreateHudView (_unitActiveData.Status.Hp);
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

	public void Action() {
		_actionPoint -= 10;
	}

	public void NextTurn() {
		if(_unitActiveData.Status.IsLive)
			_actionPoint += _unitActiveData.ActionPoint;
		Debug.Log ("action point "+ _actionPoint);
	}

	public void ResetTurn() {
		_actionPoint = _unitActiveData.ActionPoint;
	}

	public void Reaction() {
		if (_unitActiveData.Status.IsDead)
			_actionPoint = 0;
		_hudView.SetHeartPoint (_unitActiveData.Status.Hp);
		_unitView.Damage (_unitActiveData.Status.IsDead);
	}

	public void MoveTo(ChipController chipTo) {
		_unitView.MoveTo (chipTo.ChipView);
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
		if(_hudView != null) Destroy (_hudView.gameObject);
		Destroy (gameObject);
	}
}
