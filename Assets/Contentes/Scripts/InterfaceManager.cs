using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;

public class InterfaceManager : Utility.Singleton<InterfaceManager> {

	[SerializeField] RectTransform _interfaceRoot;
	[SerializeField] ArrowCanvasView _arrowView;
	[SerializeField] Image _skipButton; 
	[SerializeField] float _skipRotateSec;
	[SerializeField] Text _floorText;
	public ArrowCanvasView ArrowView { get { return _arrowView; } }

	public HudView CreateHudView(int hpMax) {
		HudView hudView = Instantiate<HudView>(Resources.Load<HudView> ("Prefabs/HudView"));
		hudView.CreateHeart (hpMax);
		hudView.transform.SetParent (_interfaceRoot);
		return hudView;
	}

	public IEnumerator Skip() {
		float angle = 0;
		while(angle > -360f) {
			angle += (-360f / _skipRotateSec) * Time.deltaTime;
			_skipButton.transform.localEulerAngles = Vector3.forward * angle;
			yield return 0;
		}
		_skipButton.transform.localEulerAngles = Vector3.zero;
	}

	[SerializeField] float _floorFadeInSec;
	[SerializeField] float _floorFadeOutSec;
	[SerializeField] float _floorDisplaySec;
	[SerializeField] float _floorMoveInSec;
	public void DispFloor(string text) {
		_floorText.enabled = true;
		_floorText.text = text;
		_floorText.color = new Color(1,1,1,0);
		_floorText.transform.localScale = Vector3.one;
		_floorText.DOFade (1, _floorFadeInSec);
		DOTween.Sequence()
			.Append(_floorText.transform.DOScale (Vector3.one * 1.5f, _floorMoveInSec).From ())
			.AppendInterval(_floorDisplaySec)
			.Append(_floorText.DOFade(0, _floorFadeOutSec));
	}

	void Awake() {
		_floorText.enabled = false;
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
