using System.Collections;
using UnityEngine;

public class SkinsManagerPixlGun : MonoBehaviour
{
	public Hashtable skins = new Hashtable();

	private void OnLevelWasLoaded(int idx)
	{
		Debug.Log("OnLevelWasLoaded");
		if (skins.Count > 0)
		{
			skins.Clear();
			Debug.Log("Clear");
		}
		string path = "EnemySkins/Level" + GlobalGameController.currentLevel;
		Object[] array = Resources.LoadAll(path);
		Object[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			Texture texture = (Texture)array2[i];
			skins.Add(texture.name, texture);
		}
	}

	private void Start()
	{
		Object.DontDestroyOnLoad(base.gameObject);
	}
}
