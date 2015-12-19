using UnityEngine;
using System.Collections;

public class UnitController : MonoBehaviour {

	UnitMasterData _unitMasterData = null;
	public UnitMasterData UnitMasterData { get { return _unitMasterData; } }
	UnitActiveData _unitActiveData = null;

	public void Setup(UnitMasterData unitMasterData) {
		Setup (unitMasterData, new UnitActiveData());
	}
		
	public void Setup(UnitMasterData unitMasterData, UnitActiveData unitActiveData) {
		_unitMasterData = unitMasterData;
		_unitActiveData = unitActiveData;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
