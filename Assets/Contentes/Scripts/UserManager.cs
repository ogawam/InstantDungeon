using UnityEngine;
using System.Collections;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

public class UserManager : Utility.Singleton<UserManager> {

	[SerializeField] UserData _userData;
	string _filePath;

	public void Save() {
		string json = JsonUtility.ToJson (_userData);
		Debug.Log (json);

		FileStream fs = new FileStream(_filePath, FileMode.Create, FileAccess.Write);
		BinaryFormatter bf = new BinaryFormatter();
		//シリアル化して書き込む
		bf.Serialize(fs, json);
		fs.Close();
	}

	public bool Load() {
		if (Directory.Exists (_filePath)) {
			FileStream fs = new FileStream (_filePath, FileMode.Open, FileAccess.Read);
			BinaryFormatter bf = new BinaryFormatter ();
			//読み込んで逆シリアル化する
			_userData = JsonUtility.FromJson<UserData> ((string)bf.Deserialize (fs));
			fs.Close ();
			return true;
		}
		return false;
	}

	public void Clear() {
		File.Delete(_filePath);
	}

	// Use this for initialization
	void Start () {
		_filePath = Application.persistentDataPath + "savedata.dat";
		_userData = new UserData ();
		Load ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
