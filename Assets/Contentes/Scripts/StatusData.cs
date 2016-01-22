using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class StatusData {

	[SerializeField] int _hp;
	public int Hp { get { return _hp; } }
	[SerializeField] int _mp;
	public int Mp { get { return _mp; } }
	[SerializeField] int _str;
	public int Str { get { return _str; } }
	[SerializeField] int _def;
	public int Def { get { return _def; } }
	[SerializeField] int _agi;
	public int Agi { get { return _agi; } }
	[SerializeField] int _lck;
	public int Lck { get { return _lck; } }
	[SerializeField] Define.Element _weakElement;
	public Define.Element WeakElement { get { return _weakElement; } }
	[SerializeField] Define.Element _attackElement;
	public Define.Element AttackElement { get { return _attackElement; } }
	[SerializeField] Define.Element _registElement;
	public Define.Element RegistElement { get { return _registElement; } }
	[SerializeField] Define.Element _absorbElement;
	public Define.Element AbsorbElement { get { return _absorbElement; } }
	[SerializeField] List<ConditionData> _conditions = new List<ConditionData>();

	public int x = 0;
	public int z = 0;
	public int ap = 0;

	public bool IsLive { get { return _hp > 0; } }
	public bool IsDead { get { return !IsLive; } }

	public void AddCondition(Define.Condition condition, int turn) {
		ConditionData conditionData = GetCondition(condition);
		if(conditionData != null)
			conditionData.turn = Mathf.Max(conditionData.turn, turn);
		else _conditions.Add(new ConditionData(){ type = condition, turn = turn });
	}
	public ConditionData GetCondition(Define.Condition condition) {
		return _conditions.FirstOrDefault (elem => elem.type == condition);
	}

	public void Copy(StatusData status) {
		x = status.x;
		z = status.z;
		ap = status.ap;
		_hp = status._hp;
		_mp = status._mp;
		_str = status._str;
		_def = status._def;
		_agi = status._agi;
		_lck = status._lck;
		_weakElement = status._weakElement;
		_attackElement = status._attackElement;
		_registElement = status._registElement;
		_absorbElement = status._absorbElement;
		_conditions = new List<ConditionData> (status._conditions);
	}

	public void Update(StatusData status) {
		Debug.Log("x "+ x + " -> "+ status.x+ " z " + z + " -> " + status.z);
		x = status.x;
		z = status.z;
		ap = status.ap;
		_hp = status._hp;
		_mp = status._mp;
		Debug.Log ("x " + x+ " z "+ z);
		_conditions = new List<ConditionData> (status._conditions);
	}

	public static StatusData operator+ (StatusData l, StatusData r) {
		StatusData result = new StatusData (l);
		result._hp += r._hp;
		result._mp += r._mp;
		result.ap += r.ap;
		result._str += r._str;
		result._def += r._def;
		result._agi += r._agi;
		result._lck += r._lck;
		result._weakElement |= r._weakElement;
		result._attackElement |= r._attackElement;
		result._registElement |= r._registElement;
		result._absorbElement |= r._absorbElement;
		foreach(ConditionData r_condition in r._conditions) {
			ConditionData l_condition = result.GetCondition(r_condition.type);
			if (l_condition != null)
				l_condition.turn = Mathf.Max (l_condition.turn, r_condition.turn);
			else result._conditions.Add (r_condition);
		}
		return result;
	}

	public StatusData() {}

	public StatusData(StatusData status) {
		Copy (status);
	}

	static public ActionResultData CalcActionResult(
		StatusData sender, StatusData receiver, ActionData action, 
		Define.Nature nature, Define.Element element) 
	{
		ActionResultData result = new ActionResultData () {
			action = action,
			receiverStatus = new StatusData (receiver),
			senderStatus = new StatusData (sender)
		};

		bool isAttack = false;
		int value = 0;
		int multi = 100;

		StatusData depend = null;
		switch (action.DependSide) {
		case Define.Side.Party:
			depend = sender;
			break;
		case Define.Side.Enemy:
			depend = receiver;
			break;
		}
		if (depend != null) {
			switch (action.DependAbility) {
			case Define.Ability.Hp: 
				value = depend.Hp; 
				break;
			case Define.Ability.Mp:
				value = depend.Mp;
				break;
			case Define.Ability.Str: 
				value = depend.Str; 
				break;
			case Define.Ability.Def: 
				value = depend.Def; 
				break;
			case Define.Ability.Agi: 
				value = depend.Agi; 
				break;
			case Define.Ability.Lck: 
				value = depend.Lck; 
				break;
			}
		}

		if(action.Action.Hp != 0) {
			
			switch (action.Method) {
			case Define.Method.Addition:
				value += action.Action.Hp;
				break;
			case Define.Method.Multiply:
				value = value * 100 / action.Action.Hp;
				break;
			}

			if (value < 0) {
				isAttack = true;
				value = Mathf.Min(0, value + result.receiverStatus.Def);

				if((element & result.receiverStatus.WeakElement) != 0) {
					multi = 200;
				}
				// Absorb and Regist Invoke at exist all element
				else if((element & (result.receiverStatus.AbsorbElement | result.receiverStatus.RegistElement)) == element) {
					if ((element & result.receiverStatus.AbsorbElement) == element) {
						value = result.senderStatus.Str;
						isAttack = false;
					}
					else multi = 0;
				}
			}
			int hpAdd = value != 0 && multi != 0 ? value * multi / 100: 0;

			if(isAttack) {
				if(nature == Define.Nature.Physical && result.receiverStatus.GetCondition(Define.Condition.Shield) != null)
					hpAdd = Mathf.Max(hpAdd, 0);
				else hpAdd = Mathf.Max(hpAdd, -1);
			}
			result.receiverStatus._hp += hpAdd;
			if (result.receiverStatus._hp <= 0) {
				result.receiverStatus._hp = 0;
				result.receiverStatus.AddCondition (Define.Condition.Dead, -1);
			}
		}
		return result;
	}
}
