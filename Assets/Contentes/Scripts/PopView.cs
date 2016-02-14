using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;
using DG.Tweening;

public class PopView : MonoBehaviour {

	[SerializeField ] Image _mainImage;

	public IEnumerator Appear() {
		_mainImage.color = new Color (1,1,1,0);
		Quaternion rotation = Quaternion.AngleAxis (Random.value * 360, Vector3.forward);
		yield return DOTween.Sequence ().
		Append (_mainImage.DOFade (1, 0.1f)).
			Join (transform.DOLocalJump (transform.localPosition + rotation * Vector3.up * 20, 80, 1, 0.25f)).
		WaitForCompletion ();
	}

	public IEnumerator TakeUp() {
		yield return DOTween.Sequence ().
		Append (_mainImage.DOFade (0, 1f)).
		Join (transform.DOLocalMoveY (40, 0.5f).SetEase(Ease.OutCirc, 2)).
		WaitForCompletion ();
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
