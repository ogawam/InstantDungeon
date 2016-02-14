using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;

public class UnitView : MonoBehaviour {

	List<UnitRegionView> _regions = new List<UnitRegionView> ();
	CanvasGroup _canvasGroup = null;
	Vector3 _baseScale = Vector3.one;

	public enum AppearType {
		Moment,
		Warp,
	};

	void Awake() {
		_canvasGroup = GetComponent<CanvasGroup> ();
		_canvasGroup.alpha = 0;
		_baseScale = transform.localScale;
		_regions.AddRange(GetComponentsInChildren<UnitRegionView> ());
	}

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	}

	public IEnumerator Appear(AppearType type) {
		switch (type) {
		case AppearType.Moment:
			transform.localScale = _baseScale;
			_canvasGroup.alpha = 1;
			break;
		case AppearType.Warp:
			transform.localScale = new Vector3(0, 1.25f * _baseScale.y);
			yield return DOTween.Sequence ()
				.Append (_canvasGroup.DOFade (1, 0.25f))
				.Join (transform.DOScale (_baseScale, 0.5f).SetEase (Ease.InBounce))
				.WaitForCompletion ();
			break;
		}
	}

	public void Stop() {
		transform.DOKill ();
	}

	public void Damage(bool isDead) {
		foreach(UnitRegionView region in _regions)
			if(region.RigidBody2D != null)
				region.RigidBody2D.AddTorque (100000);
		if (isDead) {
			_canvasGroup.DOFade (0, 2);
		}
	}

	public IEnumerator Open(Sprite sprite) {
		UnitRegionView headRegion = GetRegion (Define.Region.Head);
		if (headRegion != null && headRegion.RigidBody2D != null) {
			headRegion.RigidBody2D.isKinematic = false;
			yield return 0;
			headRegion.RigidBody2D.AddTorque (1000000);	
		}
		yield return new WaitForSeconds (0.5f);
		// TODO ここで演出全部やれる？

		UnitRegionView bodyRegion = GetRegion (Define.Region.Body);
		GameObject treasure = new GameObject ();
		Image treasureImage = treasure.AddComponent<Image> ();
		treasureImage.sprite = sprite;
		treasureImage.color = Color.clear;
		treasureImage.DOFade(1, 0.1f);
		treasure.transform.SetParent (transform, false);
		treasure.transform.localScale = Vector3.one * 0.5f;
		treasure.transform.localPosition = bodyRegion.transform.localPosition;
		yield return treasure.transform.DOLocalMoveY (80, 1).SetEase(Ease.OutCirc, 2).WaitForCompletion ();
		yield return StartCoroutine(Destruction ());
	}

	public IEnumerator Destruction() {
		_canvasGroup.DOFade (0, 1).WaitForCompletion();
		yield return new WaitForSeconds (0.1f);
	}

	public UnitRegionView GetRegion(Define.Region region) {
		return _regions.FirstOrDefault (elem => elem.Type == region);		
	}

	public void EquipItem(Define.Region region, Sprite sprite) {
		UnitRegionView view = GetRegion(region);
		if (view != null) {
			view.Equip (sprite);
		}
	}

	public IEnumerator DoMove(ChipView chipTo) {
		yield return transform.DOLocalJump (chipTo.transform.localPosition, 40, 1, 0.5f).WaitForCompletion();
	}

	void OnDrawGizmos() {
		Gizmos.DrawWireCube (transform.position, Vector3.one * 40);
	}
}
