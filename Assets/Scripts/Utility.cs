using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Utility {

	static public class Methods {
		static public bool Same<T>(this T src, params T[] dst) where T : IConvertible {
			int srcInt = src.ToInt32 (null);
			foreach(T t in dst)
				if ((srcInt & t.ToInt32(null)) == 0)
					return false;
			return true;
		}

		static public bool Contains<T>(this T src, params T[] dst) where T : IConvertible {
			int srcInt = src.ToInt32 (null);
			foreach (T t in dst)	
				if ((srcInt & t.ToInt32(null)) != 0)
					return true;
			return false;
		}
	}

	public class Singleton<T> : MonoBehaviour where T : Singleton<T> {
		static T _instance = null;
		static public T Instance { get { if(_instance == null) _instance = FindObjectOfType<T>(); return _instance; } }
	}
}
