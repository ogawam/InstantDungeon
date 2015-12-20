using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using DG;

public class ArrowCanvasView : MonoBehaviour {

	[SerializeField] Image _upArrowImage;
	[SerializeField] Image _downArrowImage;
	[SerializeField] Image _leftArrowImage;
	[SerializeField] Image _rightArrowImage;
	Dictionary<Define.Direction, Image> _arrowImages = new Dictionary<Define.Direction, Image>();

	public void Disp() {
		_canvasGroup.alpha = 1;
		foreach(Image arrowImage in _arrowImages.Values)
			arrowImage.fillAmount = 0;
	}

	public void Hide() {
		_canvasGroup.alpha = 0;
	}

	public void Fill(Define.Direction direction, float rate) {
		foreach (Define.Direction d in Enum.GetValues(typeof(Define.Direction)))
			_arrowImages[d].fillAmount = (d != direction) ? 0 : rate;
	}

	CanvasGroup _canvasGroup = null;
	void Awake () {
		_canvasGroup = GetComponent<CanvasGroup> ();
		_arrowImages [Define.Direction.Up] = _upArrowImage;
		_arrowImages [Define.Direction.Down] = _downArrowImage;
		_arrowImages [Define.Direction.Left] = _leftArrowImage;
		_arrowImages [Define.Direction.Right] = _rightArrowImage;
		Hide ();
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
