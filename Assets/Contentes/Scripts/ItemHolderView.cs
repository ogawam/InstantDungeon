using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;
using DG.Tweening;

public class ItemHolderView : MonoBehaviour, IPointerClickHandler {

	[SerializeField] float _scaleRate;
	[SerializeField] float _scaleSec;
	[SerializeField] Image _itemImage;
	[SerializeField] Image _equipImage;
	Image _frameImage;

	System.Action<ItemHolderView> _clickAction;
	public void Setup(System.Action<ItemHolderView> clickAction) {
		_clickAction = clickAction;
	}

	public void SetItem(Sprite itemSprite, Sprite equipSprite, bool isFlip) {
		_itemImage.sprite = itemSprite;
		_itemImage.enabled = (itemSprite != null);
		if (itemSprite != null) {
			Rect rect = itemSprite.textureRect;
			float rate = 80f / (rect.width > rect.height ? rect.width : rect.height);
			_itemImage.rectTransform.sizeDelta = new Vector2 (rect.width * rate, rect.height * rate);
		}
		_equipImage.sprite = equipSprite;
//		if(equipSprite != null)
//			_equipImage.;
	}

	public void SetSelect(bool select) {
		_frameImage.transform.localScale = Vector3.one;
		if (select)
			_frameImage.transform.DOScale (Vector3.one * _scaleRate, _scaleSec).SetLoops (-1, LoopType.Yoyo);
		else _frameImage.transform.DOKill (true);
	}

	public void SetEquip(bool isEquip) {
		_equipImage.enabled = isEquip;
	}

	public void OnPointerClick (PointerEventData eventData) {
		_clickAction (this);
	}

	// Use this for initialization
	void Start () {
		_frameImage = GetComponent<Image> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
