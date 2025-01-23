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
		expPath = GooglePlayDownloader.GetExpansionFilePath();
		StartCoroutine(loadLevel());
	}

	private void LoadLoading()
	{
		GlobalGameController.currentLevel = -1;
		Application.LoadLevel("Loading");
	}

	private void log(string t)
	{
		logtxt = logtxt + t + "\n";
		MonoBehaviour.print("MYLOG " + t);
	}

	private void OnGUI()
	{
		GUI.DrawTexture(new Rect(((float)Screen.width - 2048f * (float)Screen.height / 1154f) / 2f, 0f, 2048f * (float)Screen.height / 1154f, Screen.height), androidFon, ScaleMode.StretchToFill);
		if (!GooglePlayDownloader.RunningOnAndroid())
		{
			GUI.Label(new Rect(10f, 10f, Screen.width - 10, 20f), "Use GooglePlayDownloader only on Android device!");
		}
		else if (expPath == null)
		{
			GUI.Label(new Rect(10f, 10f, Screen.width - 10, 20f), "External storage is not available!");
		}
	}

	protected IEnumerator loadLevel()
	{
		string mainPath = GooglePlayDownloader.GetMainOBBPath(expPath);
		if (mainPath == null)
		{
			GooglePlayDownloader.FetchOBB();
		}
		while (mainPath == null)
		{
			log("waiting mainPath " + mainPath);
			yield return new WaitForSeconds(0.5f);
			mainPath = GooglePlayDownloader.GetMainOBBPath(expPath);
		}
		Application.LoadLevel("Loading");
	}
}
