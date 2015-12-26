using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class UnitView : MonoBehaviour {

	List<UnitRegionView> regions = new List<UnitRegionView> ();
	CanvasGroup _canvasGroup = null;

	// Use this for initialization
	void Start () {
		_canvasGroup = GetComponent<CanvasGroup> ();
		regions.AddRange(GetComponentsInChildren<UnitRegionView> ());
	}
	
	// Update is called once per frame
	void Update () {
/*		
		if (Input.anyKeyDown) {
			transform.DOLocalMove (Vector2.left * 80, 0.5f).SetRelative ();
			foreach (Rigidbody2D rigid2D in transform.GetComponentsInChildren<Rigidbody2D> ()) {
			//	rigid2D.AddTorque (1000000);
				Debug.Log (rigid2D.name);
			}
		}
*/
}

	public void Stop() {
		transform.DOKill ();
	}

	public void Damage(bool isDead) {
		foreach (Rigidbody2D child in GetComponentsInChildren<Rigidbody2D> ())
			child.AddTorque (100000);
		if (isDead) {
			_canvasGroup.DOFade (0, 2);
		}
	}

	public void MoveTo(ChipView chipTo) {
		transform.DOLocalJump(chipTo.transform.localPosition, 40, 1, 0.5f);
	}

	void OnDrawGizmos() {
		Gizmos.DrawWireCube (transform.position, Vector3.one * 40);
	}
}
