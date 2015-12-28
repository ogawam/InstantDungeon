using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class HudView : MonoBehaviour {

	[SerializeField] Image heartImage;
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
	
	// Use this for initialization
	void Awake () {
		_canvasGroup = GetComponent<CanvasGroup> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
