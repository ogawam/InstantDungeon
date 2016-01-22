using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;

public class UnitView : MonoBehaviour {

	List<UnitRegionView> _regions = new List<UnitRegionView> ();
	CanvasGroup _canvasGroup = null;

	void Awake() {
		_canvasGroup = GetComponent<CanvasGroup> ();
		_regions.AddRange(GetComponentsInChildren<UnitRegionView> ());
	}

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	}

	public void Stop() {
		transform.DOKill ();
	}

	public void Damage(bool isDead) {
		foreach(UnitRegionView region in _regions)
			if(region.RigidBody2D != null)
				region.RigidBody2D.AddTorque (1000000);
		if (isDead) {
			_canvasGroup.DOFade (0, 2);
		}
	}

	public void EquipItem(Define.Region region, Sprite sprite) {
		UnitRegionView view = _regions.FirstOrDefault (elem => elem.Type == region);
		if (view != null) {
			view.Equip (sprite);
		}
	}

	public void MoveTo(ChipView chipTo) {
		transform.DOLocalJump(chipTo.transform.localPosition, 40, 1, 0.5f);
	}

	void OnDrawGizmos() {
		Gizmos.DrawWireCube (transform.position, Vector3.one * 40);
	}
}
