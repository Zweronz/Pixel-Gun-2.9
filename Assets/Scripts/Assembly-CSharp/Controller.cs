using System.Collections;
using UnityEngine;

public class Controller : MonoBehaviour
{
	private static float koefMashtab = (float)Screen.height / 768f;

	public bool showEnabled;

	private SpisokSkinov spisokSkinovContoller;

	private ViborChastiTela viborChastiTelaController;

	public ArrayList arrNameSkin;

	public ArrayList arrTitleSkin;

	public GameObject objPeople;

	public PreviewController previewControl;

	public Texture2D fon;

	public Texture2D title;

	public Texture2D plashkaNiz;

	public Texture2D plashkaInfo;

	public Texture2D palshkaAbout;

	public GUIStyle butCreateNew;

	public GUIStyle butPresets;

	public GUIStyle butUpload;

	public GUIStyle butAbout;

	public GUIStyle butBack;

	public GUIStyle butInfo;

	public GUIStyle textInfo;

	public GUIStyle backBut;

	public GUIStyle textBrowserInfo;

	public GUIStyle stButHome;

	private Rect rectCreateNew;

	private Rect rectPresets;

	private Rect rectUpload;

	private Rect rectAbout;

	private bool _browseIsShown;

	private WebViewObject _wvo;

	private float bottomPnaelHeight = Screen.height / 8;

	private bool infoActive;

	private bool aboutActive;

	private Rect rectInfo;

	private Rect rectScroll;

	private Vector2 scrollPosition;

	public GUISkin optionsSkin;

	private string txtAbout = "\"Presets\" section.\nHere you can choose favorite skin from the presets collection and edit it. Also you can save it to the gallery.\n\n\"Create New\" section.\nHere you can make you own skin and save it to the \"Presets\" section.\n\n\"Upload\" section.\nHere you can upload skin to your Mojang profile.\n\n     Why does this app need access to your pictures (If you use iOS 6):\nIt needs access in order to save new skins in your photo album.This is required by the device's operating system and it's the only way to save skins on your device. App will not delete, read or use your private photos. Why does this app need access to your location.\n(If you use iOS 5):\nThis is required for creating a Album for your new skins. Album are often associated with locations when you are taking pictures and this is an automatic setting in the operating system for creating of albums. If you do not give to Pixlgun 3D access to location.\n     The skins will be saved to Camera Roll. If you declined giving  Access  to  your photos ,you will not be able to save new skins to the separate album. To give to this app access to Photos, go to SETTINGS on your device. Select PRIVACY and then PHOTOS. Turn the switch next to Pixlgun 3D - ON.";

	private string txtInfo = "To upload skin:\n\n - Log in Majong account (app doesn't have access to your data about username and password)\n\n- Click \"HOME\" button on the lower panel\n\n- Click on the \"Choose file\", then on \"Choose Existing\" and select the skin from folder \"Pixlgun 3D Skins\" in the gallery.\n\n- Click \"Upload\" button.";

	private void Awake()
	{
		objPeople = GameObject.Find("PreviewObject");
		previewControl = objPeople.GetComponent<PreviewController>();
		objPeople.active = false;
		spisokSkinovContoller = GetComponent<SpisokSkinov>();
		viborChastiTelaController = GetComponent<ViborChastiTela>();
		optionsSkin.verticalScrollbar.fixedWidth = 35f * koefMashtab;
		optionsSkin.verticalScrollbarThumb.fixedWidth = 35f * koefMashtab;
		textInfo.fontSize = Mathf.RoundToInt(30f * koefMashtab);
		rectInfo = new Rect((float)Screen.width * 0.5f - (float)plashkaInfo.width * 0.5f * koefMashtab, 100f * koefMashtab, (float)plashkaInfo.width * koefMashtab, (float)plashkaInfo.height * koefMashtab);
		rectAbout = new Rect((float)Screen.width * 0.5f - (float)butAbout.normal.background.width * 0.5f * koefMashtab, (float)Screen.height - (float)butAbout.normal.background.height * koefMashtab - 45f * koefMashtab, (float)butAbout.normal.background.width * koefMashtab, (float)butAbout.normal.background.height * koefMashtab);
		rectUpload = new Rect((float)Screen.width * 0.5f - (float)butUpload.normal.background.width * 0.5f * koefMashtab, rectAbout.y - (float)butUpload.normal.background.height * koefMashtab - 25f * koefMashtab, (float)butUpload.normal.background.width * koefMashtab, (float)butUpload.normal.background.height * koefMashtab);
		rectPresets = new Rect(rectUpload.x, rectUpload.y - (rectAbout.y - rectUpload.y), rectUpload.width, rectUpload.height);
		rectCreateNew = new Rect(rectUpload.x, rectPresets.y - (rectAbout.y - rectUpload.y), rectUpload.width, rectUpload.height);
		rectScroll = new Rect(rectInfo.x + 50f * koefMashtab, rectInfo.y + 120f * koefMashtab, rectInfo.width - 100f * koefMashtab, rectInfo.height - 180f * koefMashtab);
		spisokSkinovContoller.showEnabled = false;
		arrNameSkin = new ArrayList();
		arrTitleSkin = new ArrayList();
		if (!Load.LoadBool("CreateSpisokSkinov"))
		{
			string[] array = new string[16]
			{
				"Harlem Boy", "Slender-Man", "Star Capitan", "Bear", "Boy", "Girl", "Vampire", "Zombie", "Knight", "Alien",
				"Man in black", "Super hero", "Robot", "Fiance", "Bridge", "Ninja"
			};
			object[] array2 = Resources.LoadAll("Skins");
			for (int i = 0; i < array2.Length; i++)
			{
				string text = "Skin_" + i;
				arrTitleSkin.Add(array[i]);
				arrNameSkin.Add(text);
				SkinsManager.SaveTextureWithName((Texture2D)array2[i], text);
			}
			string[] variable = arrNameSkin.ToArray(typeof(string)) as string[];
			Save.SaveStringArray("arrNameSkin", variable);
			Save.SaveStringArray("arrTitleSkin", array);
			spisokSkinovContoller.arrNameSkin = arrNameSkin;
			spisokSkinovContoller.arrTitleSkin = arrTitleSkin;
			Save.SaveInt("nomSkinForSave", array2.Length);
			Save.SaveBool("CreateSpisokSkinov", true);
		}
		else
		{
			string[] c = Load.LoadStringArray("arrNameSkin");
			arrNameSkin.AddRange(c);
			spisokSkinovContoller.arrNameSkin = arrNameSkin;
			string[] c2 = Load.LoadStringArray("arrTitleSkin");
			arrTitleSkin.AddRange(c2);
			spisokSkinovContoller.arrTitleSkin = arrTitleSkin;
		}
	}

	private void OnGUI()
	{
		if (!showEnabled)
		{
			return;
		}
		GUI.skin = optionsSkin;
		if (aboutActive)
		{
			GUI.DrawTexture(new Rect(0f, 0f, Screen.width, Screen.height), fon, ScaleMode.ScaleAndCrop);
			GUI.DrawTexture(rectInfo, palshkaAbout);
			float num = 5.6f;
			scrollPosition = GUI.BeginScrollView(rectScroll, scrollPosition, new Rect(rectScroll.x, rectScroll.y, rectScroll.width - 40f * koefMashtab, rectScroll.height * num), false, true);
			GUI.Label(new Rect(rectScroll.x, rectScroll.y, rectScroll.width - 40f * koefMashtab, rectScroll.height * num), txtAbout, textInfo);
			GUI.EndScrollView();
			GUI.DrawTexture(new Rect(0f, (float)Screen.height - (float)plashkaNiz.height * koefMashtab, Screen.width, (float)plashkaNiz.height * koefMashtab), plashkaNiz);
			if (GUI.Button(new Rect(55f * koefMashtab, (float)Screen.height - (9f + (float)butBack.normal.background.height) * koefMashtab, (float)butBack.normal.background.width * koefMashtab, (float)butBack.normal.background.height * koefMashtab), string.Empty, butBack))
			{
				aboutActive = false;
			}
		}
		else if (!_browseIsShown)
		{
			GUI.DrawTexture(new Rect(0f, 0f, Screen.width, Screen.height), fon, ScaleMode.ScaleAndCrop);
			GUI.DrawTexture(new Rect((float)Screen.width * 0.5f - (float)title.width * 0.5f * koefMashtab, 45f * koefMashtab, (float)title.width * koefMashtab, (float)title.height * koefMashtab), title);
			float num2 = (float)Screen.height / 768f;
			if (GUI.Button(new Rect((float)backBut.active.background.width * 0.1f * num2, rectUpload.y + rectUpload.height / 2f - (float)(backBut.active.background.height / 2) * num2, (float)backBut.active.background.width * num2, (float)backBut.active.background.height * num2), string.Empty, backBut))
			{
				Application.LoadLevel("Restart");
			}
			if (GUI.Button(rectCreateNew, string.Empty, butCreateNew))
			{
				objPeople.active = true;
				showEnabled = false;
				previewControl.CurrentTextureIndex = -1;
				viborChastiTelaController.cutSkin(-1);
				viborChastiTelaController.showEnabled = true;
			}
			if (GUI.Button(rectPresets, string.Empty, butPresets))
			{
				objPeople.active = true;
				previewControl.updateSpisok();
				previewControl.CurrentTextureIndex = arrNameSkin.Count - 1;
				previewControl.ShowSkin(previewControl.CurrentTextureIndex);
				spisokSkinovContoller.showEnabled = true;
				showEnabled = false;
			}
			if (GUI.Button(rectUpload, string.Empty, butUpload))
			{
				_wvo = WebViewStarter.StartBrowser("http://minecraft.net/login");
				_wvo.SetMargins(0, 0, 0, Mathf.RoundToInt((float)plashkaNiz.height * koefMashtab));
				_browseIsShown = true;
			}
			if (GUI.Button(rectAbout, string.Empty, butAbout))
			{
				scrollPosition = new Vector2(0f, 0f);
				aboutActive = true;
			}
		}
		else if (infoActive)
		{
			GUI.DrawTexture(new Rect(0f, 0f, Screen.width, Screen.height), fon, ScaleMode.ScaleAndCrop);
			GUI.DrawTexture(rectInfo, plashkaInfo);
			GUI.Label(rectScroll, txtInfo, textBrowserInfo);
			GUI.DrawTexture(new Rect(0f, (float)Screen.height - (float)plashkaNiz.height * koefMashtab, Screen.width, (float)plashkaNiz.height * koefMashtab), plashkaNiz);
			if (GUI.Button(new Rect(55f * koefMashtab, (float)Screen.height - (9f + (float)butBack.normal.background.height) * koefMashtab, (float)butBack.normal.background.width * koefMashtab, (float)butBack.normal.background.height * koefMashtab), string.Empty, butBack))
			{
				_wvo.SetVisibility(true);
				infoActive = false;
			}
		}
		else
		{
			GUI.DrawTexture(new Rect(0f, (float)Screen.height - (float)plashkaNiz.height * koefMashtab, Screen.width, (float)plashkaNiz.height * koefMashtab), plashkaNiz);
			if (GUI.Button(new Rect(55f * koefMashtab, (float)Screen.height - (9f + (float)butBack.normal.background.height) * koefMashtab, (float)butBack.normal.background.width * koefMashtab, (float)butBack.normal.background.height * koefMashtab), string.Empty, butBack))
			{
				_browseIsShown = false;
				Object.Destroy(_wvo.gameObject);
				_wvo = null;
			}
			if (GUI.Button(new Rect((float)Screen.width * 0.5f - (float)stButHome.normal.background.width * koefMashtab * 0.5f, (float)Screen.height - (9f + (float)stButHome.normal.background.height) * koefMashtab, (float)stButHome.normal.background.width * koefMashtab, (float)stButHome.normal.background.height * koefMashtab), string.Empty, stButHome))
			{
				_wvo.goHome();
			}
			if (GUI.Button(new Rect((float)Screen.width - 55f * koefMashtab - (float)butInfo.normal.background.width * koefMashtab, (float)Screen.height - (9f + (float)butInfo.normal.background.height) * koefMashtab, (float)butInfo.normal.background.width * koefMashtab, (float)butInfo.normal.background.height * koefMashtab), string.Empty, butInfo))
			{
				_wvo.SetVisibility(false);
				infoActive = true;
			}
		}
	}
}
