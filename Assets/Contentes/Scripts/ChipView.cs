using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;

public class ChipView : MonoBehaviour, IPointerClickHandler, IDragHandler, IEndDragHandler {

	[SerializeField ] Image _flatImage;

	UnityAction _clickAction;
	UnityAction<PointerEventData> _dragAction;
	UnityAction<PointerEventData> _endDragAction;
	public void Setup(UnityAction clickAction, UnityAction<PointerEventData> dragAction, UnityAction<PointerEventData> endDragAction) {
		_clickAction = clickAction;
		_dragAction = dragAction;
		_endDragAction = endDragAction;
	}

	public void OnPointerClick (PointerEventData eventData) {
		_clickAction();
	}

	public void OnDrag(PointerEventData eventData) {
		_dragAction (eventData);
	}

	public void OnEndDrag(PointerEventData eventData) {
		_endDragAction (eventData);
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
