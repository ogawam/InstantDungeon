using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;

public class UnitView : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.anyKeyDown) {
			transform.DOLocalMove (Vector2.left * 80, 0.5f).SetRelative ();
			foreach (Rigidbody2D rigid2D in transform.GetComponentsInChildren<Rigidbody2D> ()) {
			//	rigid2D.AddTorque (1000000);
				Debug.Log (rigid2D.name);
			}
		}
	}

	void OnDrawGizmos() {
		Gizmos.DrawWireCube (transform.position, Vector3.one * 40);
	}
}
