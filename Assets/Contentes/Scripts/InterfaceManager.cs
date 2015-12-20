using UnityEngine;
using System.Collections;

public class InterfaceManager : Utility.Singleton<InterfaceManager> {

	[SerializeField] RectTransform _interfaceRoot;
	[SerializeField] ArrowCanvasView _arrowView;
	public ArrowCanvasView ArrowView { get { return _arrowView; } }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
