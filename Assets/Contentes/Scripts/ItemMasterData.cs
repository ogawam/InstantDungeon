using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

// todo : serialize for save
[System.Serializable]
public class ItemMasterData {

	[SerializeField] string _unitName;
	public string UnitName { get { return _unitName; } }

	[SerializeField] Sprite _viewSprite;
	public Sprite ViewSprite { get { return _viewSprite; } }

	[SerializeField] StatusData _status;
	public StatusData Status { get { return _status; } }
}
