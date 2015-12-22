using UnityEngine;
using System.Collections;

public class InterfaceManager : Utility.Singleton<InterfaceManager> {

	[SerializeField] RectTransform _interfaceRoot;
	[SerializeField] ArrowCanvasView _arrowView;
	public ArrowCanvasView ArrowView { get { return _arrowView; } }

	public HudView CreateHudView(int hpMax) {
		HudView hudView = Instantiate<HudView>(Resources.Load<HudView> ("Prefabs/HudView"));
		hudView.CreateHeart (hpMax);
		hudView.transform.SetParent (_interfaceRoot);
		return hudView;
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
