using UnityEngine;
using System.Collections;

public class GameManager : Utility.Singleton<GameManager> {

	GameObject _chipRoot = null;
	GameObject _unitRoot = null;

	// Use this for initialization
	void Start () {
		_chipRoot = new GameObject ("ChipRoot");
		_chipRoot.transform.SetParent (transform);
		for(int x = 0; x < Define.StageWidth; ++x) {
			for (int z = 0; z < Define.StageDepth; ++z) {
				ChipController chipController = new GameObject (typeof(ChipController).ToString() + "["+ x + "]["+ z + "]").AddComponent<ChipController> ();
				chipController.transform.SetParent (_chipRoot.transform);

				StageManager.Instance.SetChip (x, z, chipController);
			}
		}	
		_unitRoot = new GameObject ("UnitRoot");
		_unitRoot.transform.SetParent (transform);
		MasterData master = Resources.Load<MasterData> ("Datas/MasterData");
		UnitMasterData heroMaster = master.FindMasterData ("Hero");
		UnitController unitController = new GameObject(heroMaster.UnitName).AddComponent<UnitController>();
		unitController.Setup (heroMaster);
		StageManager.Instance.SetUnit(0, 0, unitController);
	}
	
	// Update is called once per frame
	void Update () {
		Vector2 gravity2D = new Vector2(Input.acceleration.x, Input.acceleration.y).normalized;
		Physics2D.gravity = gravity2D * Physics2D.gravity.magnitude;
	}
}
