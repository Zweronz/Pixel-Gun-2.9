using System.Collections;
using UnityEngine;

public class SpisokSkinov : MonoBehaviour
{
	public bool showEnabled;

	private static float koefMashtab = (float)Screen.height / 768f;

	public Controller mainController;

	private ViborChastiTela viborChastiTelaController;

	public ArrayList arrNameSkin;

	public ArrayList arrTitleSkin;

	public Texture2D fonTitle;

	public Texture2D plashkaNiz;

	public Texture2D oknoDelSkin;

	public GUIStyle butBack;

	public GUIStyle labelTitle;

	public GUIStyle butDel;

	public GUIStyle labelTitleSkin;

	public GUIStyle butDlgOk;

	public GUIStyle butDlgCancel;

	private bool dialogDelNeActiv = true;

	private bool msgSaveShow;

	private Rect rectDialogDel;

	private void Start()
	{
		mainController = GetComponent<Controller>();
		viborChastiTelaController = GetComponent<ViborChastiTela>();
		mainController.previewControl.editModeEnteredDelegate = shooseSkin;
		rectDialogDel = new Rect((float)Screen.width * 0.5f - (float)oknoDelSkin.width * 0.5f * koefMashtab, (float)Screen.height * 0.5f - (float)oknoDelSkin.height * 0.5f * koefMashtab, (float)oknoDelSkin.width * koefMashtab, (float)oknoDelSkin.height * koefMashtab);
	}

	private void Update()
	{
		if (!showEnabled)
		{
		}
	}

	private void OnGUI()
	{
		if (!showEnabled)
		{
			return;
		}
		GUI.DrawTexture(new Rect(0f, 0f, Screen.width, (float)fonTitle.height * koefMashtab), fonTitle);
		GUI.Label(new Rect(0f, 0f, Screen.width, (float)fonTitle.height * koefMashtab), "CHOOSE THE SKIN", labelTitle);
		GUI.DrawTexture(new Rect(0f, (float)Screen.height - (float)plashkaNiz.height * koefMashtab, Screen.width, (float)plashkaNiz.height * koefMashtab), plashkaNiz);
		if (GUI.Button(new Rect(55f * koefMashtab, (float)Screen.height - (9f + (float)butBack.normal.background.height) * koefMashtab, (float)butBack.normal.background.width * koefMashtab, (float)butBack.normal.background.height * koefMashtab), string.Empty, butBack) && dialogDelNeActiv)
		{
			mainController.showEnabled = true;
			showEnabled = false;
			mainController.objPeople.active = false;
		}
		GUI.Label(new Rect(0f, 120f * koefMashtab, Screen.width, 50f * koefMashtab), (string)arrTitleSkin[mainController.previewControl.CurrentTextureIndex], labelTitleSkin);
		if (mainController.previewControl.CurrentTextureIndex > 15 && GUI.Button(new Rect((float)Screen.width - 55f * koefMashtab - (float)butDel.normal.background.width * koefMashtab, (float)Screen.height - (9f + (float)butDel.normal.background.height) * koefMashtab, (float)butDel.normal.background.width * koefMashtab, (float)butDel.normal.background.height * koefMashtab), string.Empty, butDel) && dialogDelNeActiv)
		{
			dialogDelNeActiv = false;
			mainController.previewControl.Locked = true;
		}
		if (!dialogDelNeActiv)
		{
			GUI.DrawTexture(rectDialogDel, oknoDelSkin);
			if (GUI.Button(new Rect(rectDialogDel.x + 55f * koefMashtab, rectDialogDel.y + rectDialogDel.height - 125f * koefMashtab, (float)butDlgCancel.normal.background.width * koefMashtab, (float)butDlgCancel.normal.background.height * koefMashtab), string.Empty, butDlgCancel))
			{
				dialogDelNeActiv = true;
				mainController.previewControl.Locked = false;
			}
			if (GUI.Button(new Rect(rectDialogDel.x + rectDialogDel.width - 55f * koefMashtab - (float)butDlgOk.normal.background.width * koefMashtab, rectDialogDel.y + rectDialogDel.height - 125f * koefMashtab, (float)butDlgOk.normal.background.width * koefMashtab, (float)butDlgOk.normal.background.height * koefMashtab), string.Empty, butDlgOk))
			{
				SkinsManager.DeleteTexture((string)arrNameSkin[mainController.previewControl.CurrentTextureIndex]);
				arrNameSkin.RemoveAt(mainController.previewControl.CurrentTextureIndex);
				arrTitleSkin.RemoveAt(mainController.previewControl.CurrentTextureIndex);
				string[] variable = arrNameSkin.ToArray(typeof(string)) as string[];
				string[] variable2 = arrTitleSkin.ToArray(typeof(string)) as string[];
				Save.SaveStringArray("arrNameSkin", variable);
				Save.SaveStringArray("arrTitleSkin", variable2);
				mainController.previewControl.updateSpisok();
				mainController.previewControl.ShowSkin(mainController.previewControl.CurrentTextureIndex - 1);
				mainController.previewControl.Locked = false;
				dialogDelNeActiv = true;
			}
		}
		if (msgSaveShow)
		{
			GUI.Label(new Rect(0f, (float)Screen.height - 200f * koefMashtab, Screen.width, 100f * koefMashtab), "The Skin has been  saved to gallery", labelTitle);
		}
	}

	private void shooseSkin()
	{
		viborChastiTelaController.cutSkin(mainController.previewControl.CurrentTextureIndex);
		showEnabled = false;
		viborChastiTelaController.showEnabled = true;
		ViborChastiTela.skinIzm = false;
	}

	public void hideMsg()
	{
		msgSaveShow = false;
	}

	public void showMsg()
	{
		msgSaveShow = true;
		Invoke("hideMsg", 2f);
	}
}
