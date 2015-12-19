using UnityEngine;
using System.Collections;

public class StageManager : Utility.Singleton<StageManager> {

	[SerializeField] RectTransform _stageRoot;

	ChipController[,] _chips = new ChipController[Define.StageWidth, Define.StageDepth];
	UnitController[,] _units = new UnitController[Define.StageWidth, Define.StageDepth];

	public void SetChip(int x, int z, ChipController chip) {
		_chips[x, z] = chip;
	}

	public void SetUnit(int x, int z, UnitController unit) {
		string viewPath = "Prefabs/" + unit.UnitMasterData.ViewName;
		Debug.Log ("Load "+ viewPath);
		UnitView unitView = Instantiate(Resources.Load<UnitView>(viewPath));
		unitView.transform.SetParent (_stageRoot, false);
		_units[x, z] = unit;
	}

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
