using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(Image))]
public class UnitRegionView : MonoBehaviour {

	[SerializeField] Define.Region _type;
	public Define.Region Type { get { return _type; } }

	Rigidbody2D _rigidBody2D;
	public Rigidbody2D RigidBody2D { get { return _rigidBody2D; } }
	Image _imageRegion;
	Image _imageEquip;

	public void Equip(Sprite sprite) {
		if (_imageEquip != null) {
			_imageEquip.sprite = sprite;
			_imageEquip.color = sprite ? Color.white : Color.clear;
			_imageEquip.SetNativeSize ();
		}
	}

	void Awake() {
		_rigidBody2D = GetComponent<Rigidbody2D> ();
		_imageRegion = GetComponent<Image> ();
		foreach(Transform child in transform)
			_imageEquip = child.GetComponent<Image> ();
		if(_imageRegion != null) {
			Vector2 pivot = new Vector2 (_imageRegion.sprite.pivot.x / _imageRegion.sprite.rect.width, _imageRegion.sprite.pivot.y / _imageRegion.sprite.rect.height);
	//		Debug.Log ("pivot x " + pivot.x + " y " + pivot.y);
		//	((RectTransform)transform).pivot = pivot;
		}
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
