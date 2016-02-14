using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class PopController : MonoBehaviour {

	DropMasterData _dropMasterData;
	public DropMasterData DropMasterData { get { return _dropMasterData; } }

	PopView _popView = null;
	public PopView PopView { get { return _popView; } } 

	public int x;
	public int z;

	public void Setup(DropMasterData dropMasterData) {
		_dropMasterData = dropMasterData;

		string viewPath = "Prefabs/" + _dropMasterData.PopType + "PopView";
		Debug.Log ("Load "+ viewPath);
		_popView = Instantiate<PopView> (Resources.Load<PopView> (viewPath));
	}

	public void Remove() {
		if(_popView != null) Destroy (_popView.gameObject);
	}

	public IEnumerator Appear() {
		yield return StartCoroutine(_popView.Appear ());
	}
}
