using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class HudView : MonoBehaviour {

	[SerializeField] Image heartImage;
	[SerializeField] Text orderText;
	CanvasGroup _canvasGroup;
	List<Image> _hearts = new List<Image>();

	public void CreateHeart(int count) {
		for (int i = 1; i < count; ++i)
			Instantiate<Image> (heartImage).transform.SetParent(heartImage.transform.parent, false);
		_hearts.AddRange (GetComponentsInChildren<Image> ());
	}

	public void SetHeartPoint(int point) {
		for (int i = 0; i < _hearts.Count; ++i) {
			_hearts [i].color = i < point ? Color.white : Color.black;
		}
		if (point == 0) {
			_canvasGroup.DOFade (0, 0.5f);
		}
	}

	public void SetVisible(bool isVisible) {
		_canvasGroup.DOKill ();
		_canvasGroup.alpha = isVisible ? 1: 0;
	}

	public void Display(float seconds) {
		SetVisible (true);
		_canvasGroup.DOFade (0, 0.5f).SetDelay (seconds);
	}

	public void SetOrder(int order, int scale) {
		string text = order.ToString ();
		if (scale < 0)
			text += "<size=24px>1/2</size>";
		else if(scale > 0)
			text += "<size=24px>x2</size>";
		orderText.text = text;
	}
	
	// Use this for initialization
	void Awake () {
		_canvasGroup = GetComponent<CanvasGroup> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
