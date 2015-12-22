using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class HudView : MonoBehaviour {

	[SerializeField] Image heartImage;
	List<Image> hearts = new List<Image>();

	public void CreateHeart(int count) {
		for (int i = 1; i < count; ++i)
			Instantiate<Image> (heartImage).transform.SetParent(heartImage.transform.parent, false);
		hearts.AddRange (GetComponentsInChildren<Image> ());
	}

	public void SetHeartPoint(int point) {
		for (int i = 0; i < hearts.Count; ++i) {
			hearts [i].color = i < point ? Color.white : Color.black;
		}
	}
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
