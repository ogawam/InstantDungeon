using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class ChipController : MonoBehaviour {

	ChipData _chipData = null;
	ChipView _chipView = null;
	public ChipView ChipView { get { return _chipView; } } 

	public int x;
	public int z;

	public void Setup(ChipData chipData) {
		_chipData = chipData;
	}

	public void CreateView(Transform parent, Vector3 position) {
		string viewPath = "Prefabs/" + _chipData.ViewName;
		Debug.Log ("Load "+ viewPath);
		_chipView = Instantiate<ChipView> (Resources.Load<ChipView> (viewPath));
		_chipView.transform.SetParent (parent, false);
		_chipView.transform.localPosition = position;
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
}
