﻿using UnityEngine;
using System.Collections;

static public class Define {

	public enum Element {
		None,
		Holy,
		Fire,
		Wind,
		Water,
		Earth,
		Thunder,
		Ice,
	};

	public enum Region {
		None = -1,
		Body,
		Head,
		RArm,
		LArm,
		RLeg,
		LLeg,
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
		None,
		Poison,
		Stun,
		Sleep,
		Stone,
		Confuse,
		Dead,
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

	static public readonly float ChipWidth = 80;
	static public readonly float ChipDepth = 80;
	static public readonly int StageWidth = 8;
	static public readonly int StageDepth = 8;

	static public readonly float DragToMove = 80;
	static public readonly int ItemHolderMax = 8;
}
