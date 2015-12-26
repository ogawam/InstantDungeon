using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(Image))]
public class UnitRegionView : MonoBehaviour {

	[SerializeField] Define.Region type;
	public Define.Region Type { get { return type; } }

	Image image;
	void Awake() {
		image = GetComponent<Image> ();
		Vector2 pivot = new Vector2 (image.sprite.pivot.x / image.sprite.rect.width, image.sprite.pivot.y / image.sprite.rect.height);
//		Debug.Log ("pivot x " + pivot.x + " y " + pivot.y);
	//	((RectTransform)transform).pivot = pivot;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnDrawGizmos() {
		if (transform.childCount > 1)
			Debug.LogError ("only one attachment point");
		foreach(Transform child in transform) {
			Gizmos.DrawIcon (child.position, "equip.png");
		}
	}
}
