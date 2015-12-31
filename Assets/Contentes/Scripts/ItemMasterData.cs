using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

// todo : serialize for save
[System.Serializable]
public class ItemMasterData {

	[SerializeField] string _itemName;
	public string ItemName { get { return _itemName; } }

	[SerializeField] Sprite _viewSprite;
	public Sprite ViewSprite { get { return _viewSprite; } }

	[SerializeField] StatusData _status;
	public StatusData Status { get { return _status; } }

	[SerializeField] Define.Region _equipRegion;
	public Define.Region EquipRegion { get { return _equipRegion; } }
}
