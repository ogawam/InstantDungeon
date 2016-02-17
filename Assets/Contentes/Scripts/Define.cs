using UnityEngine;
using System.Collections;

static public class Define {

	static public bool isEditor { 
		get {
			return Application.platform == RuntimePlatform.OSXEditor 
				|| Application.platform == RuntimePlatform.WindowsEditor;
		}
	}

	public enum Element {
		Holy = (1<<0),
		Fire = (1<<1),
		Wind = (1<<2),
		Water = (1<<3),
		Earth = (1<<4),
		Thunder = (1<<5),
		Ice = (1<<6),
	};

	public enum Nature {
		Physical,
		Magical,
	};

	public enum Method {
		Addition,
		Multiply,
	};

	public enum Region {
		None = -1,
		Body,
		Head,
		RArm,
		LArm,
		RLeg,
		Tail, 
	};

	public enum Direction {
		Up,
		Down,
		Left,
		Right,
	};

	public enum Race {
		Anknown,
		Human,
		Beast,
		Undead,
		Dragon,	// 
		Wizard,	// 魔法使い
		Rizard,	// 爬虫類
	};

	public enum Condition {
		Poison = (1 << 0),
		Stun = (1 << 1),
		Sleep = (1 << 2),
		Stone = (1 << 3),
		Confuse = (1 << 4),
		Dead = (1 << 5),
		BadConditions = Poison | Stun | Sleep | Stone | Confuse,

		Shield = (1 << 6),
	};

	public enum Side {
		Neutral,
		Party,
		Enemy,
	};

	public enum Unit {
		Hero,
		Wall,
		Monster,
		Treasure
	};

	public enum Chip {
		Flat,
		Sand,
		Flad,
		Hole,
		Move,
		Damage,
		Poison,
		Frozen,
		Stairs,
	};

	public enum Ability {
		Hp,
		Mp,
		HpMax,
		MpMax,
		Str,
		Def,
		Agi,
		Lck
	};

	public enum DropType {
		Item,
		Pop,
	};

	public enum PopType {
		None,
		Heart,	// 体力回復
		Magic,	// 魔力回復
		Medic,	// 状態回復
		Money,	// ゴールド
	};

	static public readonly float ChipWidth = 80;
	static public readonly float ChipDepth = 80;
	static public readonly int StageWidth = 8;
	static public readonly int StageDepth = 8;

	static public readonly float DragToMove = 80;
	static public readonly int ItemHolderMax = 8;
	static public readonly int defaultActionPoint = 10;
}
