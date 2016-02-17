using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

[System.Serializable]
public class EquipInterfaceData {
	[SerializeField] Define.Region _region;
	public bool IsRegion (Define.Region region) {
		return _region == region;
	}

	[SerializeField] Sprite _iconSprite;
	public Sprite IconSprite { get { return _iconSprite; } }

	[SerializeField] bool _isFlip;
	public bool IsFlip { get { return _isFlip; } }
};

public class InterfaceManager : Utility.Singleton<InterfaceManager> {

	[SerializeField] RectTransform _interfaceRoot;
	[SerializeField] ArrowCanvasView _arrowView;
	[SerializeField] Image _skipButton; 
	[SerializeField] float _skipRotateSec;
	[SerializeField] Text _floorText;
	[SerializeField] HorizontalLayoutGroup _itemGroup;
	[SerializeField] Image _shader;
	[SerializeField] CanvasGroup _treasure;
	[SerializeField] Button _buttonTerasureOpen;
	[SerializeField] Button _buttonTerasureDestruction;
	[SerializeField] UpperInterfaceView _upperInterfaceView;
	public UpperInterfaceView UpperInterfaceView { get { return _upperInterfaceView; } }

	[SerializeField] List<EquipInterfaceData> _equipDatas; 

	private int _selectItemHolderIndex = -1;
	public int SelectItemHolderIndex { get { return _selectItemHolderIndex; } }

	List<ItemHolderView> _itemHolders = new List<ItemHolderView>();
	public ArrowCanvasView ArrowView { get { return _arrowView; } }

	public HudView CreateHudView(int hpMax) {
		HudView hudView = Instantiate<HudView>(Resources.Load<HudView> ("Prefabs/HudView"));
		hudView.CreateHeart (hpMax);
		hudView.transform.SetParent (_interfaceRoot);
		return hudView;
	}

	public IEnumerator Skip() {
		float angle = 0;
		while(angle > -360f) {
			angle += (-360f / _skipRotateSec) * Time.deltaTime;
			_skipButton.transform.localEulerAngles = Vector3.forward * angle;
			yield return 0;
		}
		_skipButton.transform.localEulerAngles = Vector3.zero;
	}

	public void AttachTreasureMethod(UnityAction open, UnityAction destruction) {
		_buttonTerasureOpen.onClick.AddListener (open);
		_buttonTerasureDestruction.onClick.AddListener (destruction);
	}

	public void SetVisibleTreasure(bool visible) {
		_shader.gameObject.SetActive(visible);
		_treasure.gameObject.SetActive(visible);
	}

	[SerializeField] float _floorFadeInSec;
	[SerializeField] float _floorFadeOutSec;
	[SerializeField] float _floorDisplaySec;
	[SerializeField] float _floorMoveInSec;
	public void DispFloor(string text) {
		_floorText.enabled = true;
		_floorText.text = text;
		_floorText.color = new Color(1,1,1,0);
		_floorText.transform.localScale = Vector3.one;
		_floorText.DOFade (1, _floorFadeInSec);
		DOTween.Sequence()
			.Append(_floorText.transform.DOScale (Vector3.one * 1.5f, _floorMoveInSec).From ())
			.AppendInterval(_floorDisplaySec)
			.Append(_floorText.DOFade(0, _floorFadeOutSec));
	}

	public void SetHolderItem(int index, ItemMasterData item) {
		if (item != null) {
			EquipInterfaceData data = _equipDatas.Find (elem => elem.IsRegion (item.EquipRegion));
			_itemHolders [index].SetItem (item.ViewSprite, data.IconSprite, data.IsFlip);
		}
		else _itemHolders [index].SetItem (null, null, false);
	}

	public void ClickItemHolder(ItemHolderView view) {
		int selectItemHolderIndex = _itemHolders.IndexOf (view);
		if (_selectItemHolderIndex >= 0) {
			_itemHolders [_selectItemHolderIndex].SetSelect (false);
			if (_selectItemHolderIndex == selectItemHolderIndex) {
				_selectItemHolderIndex = -1;
				return;
			}
		} 
		if(view != null)
			view.SetSelect (true);
		_selectItemHolderIndex = selectItemHolderIndex;
	}

	public void SetEquip(int index, bool isEquip) {
		_itemHolders [index].SetEquip (isEquip);
	}

	void Awake() {
		_floorText.enabled = false;
		for (int i = 0; i < Define.ItemHolderMax; ++i) {
			ItemHolderView view = Instantiate<ItemHolderView> (Resources.Load<ItemHolderView> ("Prefabs/ItemHolderView"));
			view.transform.SetParent(_itemGroup.transform);
			view.Setup (ClickItemHolder);
			_itemHolders.Add (view);
		}
	}

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
