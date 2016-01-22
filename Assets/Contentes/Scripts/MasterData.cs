using UnityEngine;
using System.Collections;
using System.Linq;

public class MasterData : ScriptableObject {

	[SerializeField] UnitMasterData[] _unitMasterData;
	public UnitMasterData FindUnitData(string name) {
		return _unitMasterData.First (unit => unit.UnitName == name);
	}

	[SerializeField] ChipData[] _chipData;
	public ChipData FindChipData(string name) {
		return _chipData.First (chip => chip.ChipName == name);
	}

	[SerializeField] ItemMasterData[] _itemMasterData;
	public ItemMasterData FindItemData(string name) {
		return _itemMasterData.First (item => item.ItemName == name);
	}

	[SerializeField] CommandData[] _commandData;
	public CommandData FindCommandData(string name) {
		return _commandData.First (command => command.Name == name);
	}

	[SerializeField] int _agiOnceToTwice;
	public int AgiOnceToTwice { get { return _agiOnceToTwice; } }
	[SerializeField] int _agiTwiceAtTime;
	public int AgiTwiceAtTime { get { return _agiTwiceAtTime; } }
}
