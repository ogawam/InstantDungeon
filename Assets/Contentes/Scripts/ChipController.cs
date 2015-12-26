using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class ChipController : MonoBehaviour {

	ChipData _chipData = null;
	ChipView _chipView = null;
	public ChipView ChipView { get { return _chipView; } } 

	Define.Chip _chipType;
	public Define.Chip ChipType { get { return _chipType; } }

	public int x;
	public int z;

	public void Setup(ChipData chipData, Define.Chip chipType) {
		_chipData = chipData;
		_chipType = chipType;

		string viewPath = "Prefabs/" + _chipData.ViewName;
//		Debug.Log ("Load "+ viewPath);
		_chipView = Instantiate<ChipView> (Resources.Load<ChipView> (viewPath));
		_chipView.Setup (Click, Drag, DragEnd);
	}

	public void Click() {
		GameManager.Instance.Click (this);
	}

	public void Drag(PointerEventData eventData) {
		GameManager.Instance.Drag (eventData);
	}

	public void DragEnd(PointerEventData eventData) {
		GameManager.Instance.DragEnd (eventData);
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Remove() {
		if(_chipView != null) Destroy (_chipView.gameObject);
	}
}
