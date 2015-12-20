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
		Debug.Log ("delta " + (eventData.position - eventData.pressPosition));
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
			StageManager sm = StageManager.Instance;
			int x = _heroUnit.x;
			int z = _heroUnit.z;

			switch (_dragDirection) {
			case Define.Direction.Up: z++; break;
			case Define.Direction.Down: z--; break;
			case Define.Direction.Left: x--; break;
			case Define.Direction.Right: x++; break;
			}

			ChipController chip = sm.GetChip (x, z);
			UnitController unit = sm.FindUnit (chip);
			if (chip != null && unit == null || unit.UnitMasterData.Status.Agi > 0) {
				sm.UnitTo (_heroUnit, chip);
				sm.CalcTo ();
				sm.ExecTo ();
			}
		}
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

		unitController = new GameObject(unitMaster.UnitName).AddComponent<UnitController>();
		unitController.transform.SetParent(_unitRoot.transform);
		unitController.Setup (unitMaster);
		StageManager.Instance.SetUnit (4, 5, unitController);

		unitController = new GameObject(unitMaster.UnitName).AddComponent<UnitController>();
		unitController.transform.SetParent(_unitRoot.transform);
		unitController.Setup (unitMaster);
		StageManager.Instance.SetUnit (3, 5, unitController);

		unitController = new GameObject(unitMaster.UnitName).AddComponent<UnitController>();
		unitController.transform.SetParent(_unitRoot.transform);
		unitController.Setup (unitMaster);
		StageManager.Instance.SetUnit (4, 4, unitController);

		unitController = new GameObject(unitMaster.UnitName).AddComponent<UnitController>();
		unitController.transform.SetParent(_unitRoot.transform);
		unitController.Setup (unitMaster);
		StageManager.Instance.SetUnit (4, 3, unitController);
	}
	
	// Update is called once per frame
	void Update () {
		Vector2 gravity2D = new Vector2(Input.acceleration.x, Input.acceleration.y).normalized;
		Physics2D.gravity = gravity2D * Physics2D.gravity.magnitude;
	}
}
