using UnityEngine;

public class Switcher : MonoBehaviour
{
	public Texture fon;

	public Texture fonLevel2;

	public Texture fonLevel3;

	public Texture fonLevel4;

	public Texture fonLevel5;

	public Texture fonLevel6;

	public Texture fonLevel7;

	public Texture fonLevel8;

	public bool NoWait;

	public bool isGameOver;

	private Texture fonToDraw;

	public GameObject skinsManagerPrefab;

	public GameObject weaponManagerPrefab;

	private void Start()
	{
		if (GlobalGameController.currentLevel >= GlobalGameController.NumOfLevels)
		{
			GlobalGameController.currentLevel = 0;
			GlobalGameController.AllLevelsCompleted++;
		}
		switch (GlobalGameController.currentLevel)
		{
		case -1:
			fonToDraw = fon;
			break;
		case 0:
			fonToDraw = Resources.Load((GlobalGameController.AllLevelsCompleted != 0) ? "NextLoopFon" : "Level_1_Loading") as Texture;
			break;
		case 1:
			fonToDraw = fonLevel2;
			break;
		case 2:
			fonToDraw = fonLevel3;
			break;
		case 3:
			fonToDraw = fonLevel4;
			break;
		case 4:
			fonToDraw = fonLevel5;
			break;
		case 5:
			fonToDraw = fonLevel6;
			break;
		case 6:
			fonToDraw = fonLevel7;
			break;
		case 7:
			fonToDraw = fonLevel8;
			break;
		default:
			fonToDraw = fon;
			break;
		}
		if (!isGameOver)
		{
			if (NoWait)
			{
				LoadMenu();
			}
			else
			{
				Invoke("LoadMenu", 2.5f);
			}
			if (!GameObject.FindGameObjectWithTag("SkinsManager") && (bool)skinsManagerPrefab)
			{
				Object.Instantiate(skinsManagerPrefab, Vector3.zero, Quaternion.identity);
			}
			if (!GameObject.FindGameObjectWithTag("WeaponManager") && (bool)weaponManagerPrefab)
			{
				Object.Instantiate(weaponManagerPrefab, Vector3.zero, Quaternion.identity);
			}
		}
	}

	private void Method()
	{
	}

	private void OnGUI()
	{
		if (isGameOver)
		{
			GUI.depth = 4;
		}
		Rect position = new Rect(((float)Screen.width - 2048f * (float)Screen.height / 1154f) / 2f, 0f, 2048f * (float)Screen.height / 1154f, Screen.height);
		GUI.DrawTexture(position, fonToDraw, ScaleMode.StretchToFill);
	}

	private void LoadMenu()
	{
		string text;
		switch (GlobalGameController.currentLevel)
		{
		case -1:
			text = "Restart";
			break;
		case 0:
			text = "Cementery";
			break;
		case 1:
			text = "Maze";
			break;
		case 2:
			text = "City";
			break;
		case 3:
			text = "Hospital";
			break;
		case 4:
			text = "level_jail_2";
			break;
		case 5:
			text = "Gluk";
			break;
		case 6:
			text = "Arena";
			break;
		case 7:
			text = "Level_Area51";
			break;
		default:
			text = "Restart";
			break;
		}
		GlobalGameController.currentLevel++;
		Application.LoadLevel(text);
	}
}
