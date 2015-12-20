using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GameManager : Utility.Singleton<GameManager> {

	GameObject _chipRoot = null;
	GameObject _unitRoot = null;

	UnitController _heroUnit = null;

	public void Click(ChipController chip) {
		StageManager sm = StageManager.Instance;
		UnitController unit = sm.FindUnit (chip);
		if (unit == null) {
			sm.UnitTo (_heroUnit, chip);
			sm.CalcTo ();
			sm.ExecTo ();
		}
	}

	public void Drag(PointerEventData eventData) {
		
	}

	public void DragEnd(PointerEventData eventData) {

	}

	// Use this for initialization
	void Start () {
		MasterData master = Resources.Load<MasterData> ("Datas/MasterData");
		_chipRoot = new GameObject ("ChipRoot");
		_chipRoot.transform.SetParent (transform);
		for(int x = 0; x < Define.StageWidth; ++x) {
			for (int z = 0; z < Define.StageDepth; ++z) {
				ChipController chipController = new GameObject (typeof(ChipController).ToString() + "["+ x + "]["+ z + "]").AddComponent<ChipController> ();
				chipController.transform.SetParent (_chipRoot.transform);
				chipController.Setup (master.FindChipData ("Flat"));
				StageManager.Instance.SetChip (x, z, chipController);
			}
		}	
		_unitRoot = new GameObject ("UnitRoot");
		_unitRoot.transform.SetParent (transform);
		UnitMasterData unitMaster = master.FindUnitData ("Hero");
		UnitController unitController = new GameObject(unitMaster.UnitName).AddComponent<UnitController>();
		unitController.transform.SetParent(_unitRoot.transform);
		unitController.Setup (unitMaster);
		_heroUnit = unitController;
		StageManager.Instance.SetUnit(0, 0, unitController);

		unitMaster = master.FindUnitData ("Wall");
		unitController = new GameObject(unitMaster.UnitName).AddComponent<UnitController>();
		unitController.transform.SetParent(_unitRoot.transform);
		unitController.Setup (unitMaster);
		StageManager.Instance.SetUnit (5, 5, unitController);
	}
	
	// Update is called once per frame
	void Update () {
		Vector2 gravity2D = new Vector2(Input.acceleration.x, Input.acceleration.y).normalized;
		Physics2D.gravity = gravity2D * Physics2D.gravity.magnitude;
	}
}
