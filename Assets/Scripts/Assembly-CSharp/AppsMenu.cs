using System.Collections;
using UnityEngine;

public class AppsMenu : MonoBehaviour
{
	public Texture fon;

	public Texture pixlgun3d;

	public Texture man;

	public Texture androidFon;

	public GUIStyle shooter;

	public GUIStyle skinsmaker;

	private string expPath;

	private string logtxt;

	private bool downloadStarted;

	private void Update()
	{
		if (Application.platform == RuntimePlatform.Android && Input.GetKey(KeyCode.Escape))
		{
			Application.Quit();
		}
	}

	private void Start()
	{
		Application.LoadLevel("Loading");
	}

	private void LoadLoading()
	{
		GlobalGameController.currentLevel = -1;
		Application.LoadLevel("Loading");
	}
}
