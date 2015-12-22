using UnityEngine;
using System.Collections;

public class UnitController : MonoBehaviour {

	UnitMasterData _unitMasterData = null;
	public UnitMasterData UnitMasterData { get { return _unitMasterData; } }
	UnitActiveData _unitActiveData = null;
	public UnitActiveData UnitActiveData { get { return _unitActiveData; } } 
	UnitView _unitView = null;
	public UnitView UnitView { get { return _unitView; } }
	HudView _hudView = null;

	public int x;
	public int z;

	public void Setup(UnitMasterData unitMasterData) {
		Setup (unitMasterData, new UnitActiveData(unitMasterData));
	}
		
	public void Setup(UnitMasterData unitMasterData, UnitActiveData unitActiveData) {
		_unitMasterData = unitMasterData;
		_unitActiveData = unitActiveData;
	}

	public void CreateView(Transform parent, Vector3 position) {
		string viewPath = "Prefabs/" + _unitMasterData.ViewName;
		Debug.Log ("Load "+ viewPath);
		_unitView = Instantiate(Resources.Load<UnitView>(viewPath));
		_unitView.transform.SetParent (parent, false);
		_unitView.transform.localPosition = position;
		if (_unitMasterData.Status.Agi > 0) {
			_hudView = InterfaceManager.Instance.CreateHudView (_unitActiveData.Status.Hp);
			_hudView.transform.localPosition = position + Vector3.up * 80;
		}
	}

	public void Action() {
	}

	public void Reaction() {
		_hudView.SetHeartPoint (_unitActiveData.Status.Hp);
		_unitView.Damage (_unitActiveData.Status.Hp == 0);
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
}
