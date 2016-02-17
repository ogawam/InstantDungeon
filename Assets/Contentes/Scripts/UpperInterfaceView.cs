using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class UpperInterfaceView : MonoBehaviour {

	CanvasGroup _canvasGroup;
	[SerializeField] HorizontalLayoutGroup _heartGrop;
	[SerializeField] Image _heartImage;
	List<Image> _hearts = new List<Image>();

	public void CreateHeart(int count) {
		for (int i = 1; i < count; ++i)
			Instantiate<Image> (_heartImage).transform.SetParent(_heartGrop.transform, false);
		_hearts.AddRange (_heartGrop.transform.GetComponentsInChildren<Image> ());
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
