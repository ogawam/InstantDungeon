using UnityEngine;
using System.Collections;

public class ChipController : MonoBehaviour {

	ChipData _chipData = null;

	public bool IsValid { get{ return (_chipData != null); } }

	public void Setup(ChipData chipData) {
		_chipData = chipData;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
