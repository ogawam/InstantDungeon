using UnityEngine;
using System.Collections;

public class UnitController : MonoBehaviour {

	UnitMasterData _unitMasterData = null;
	public UnitMasterData UnitMasterData { get { return _unitMasterData; } }
	UnitActiveData _unitActiveData = null;
	UnitView _unitView = null;

	public int x;
	public int z;

	public void Setup(UnitMasterData unitMasterData) {
		Setup (unitMasterData, new UnitActiveData());
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
	}

	public void MoveTo(ChipController chipTo) {
		_unitView.MoveTo (chipTo.ChipView);
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
