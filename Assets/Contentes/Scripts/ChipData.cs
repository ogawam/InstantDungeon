using UnityEngine;
using System.Collections;

[System.Serializable]
public class ChipData {

	[SerializeField] string _chipName;
	public string ChipName{ get { return _chipName; } }
	[SerializeField] string _viewName;
	public string ViewName{ get { return _viewName; } }
	[SerializeField] int _moveCost;
}
