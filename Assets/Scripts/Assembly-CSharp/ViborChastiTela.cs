using System.Collections;
using UnityEngine;

public class ViborChastiTela : MonoBehaviour
{
	public bool showEnabled;

	private static float koefMashtab = (float)Screen.height / 768f;

	public Texture2D skinForRedact;

	public Texture2D clearSkin;

	public ArrayList arrChastiTela;

	private SpisokSkinov spisokSkinovController;

	private ViborStoroniForRedact storoniForRedactController;

	private Controller mainController;

	public Texture2D plashkaNiz;

	public Texture2D title;

	public Texture2D podTitle;

	public Texture2D nadPlaskoi;

	public Texture2D oknoVvodaName;

	public Texture2D plashkaExitBezSave;

	public GUIStyle butBack;

	public GUIStyle butSave;

	public GUIStyle butDlgCancel;

	public GUIStyle butDlgOk;

	public GUIStyle poleVvoda;

	private int nomSkinaForRedact;

	private bool dialogSaveNeActiv = true;

	private bool exitBezSaveActiv;

	private string nameNewSkin = string.Empty;

	private Rect rectDialogSave;

	private Rect rectExitBezSave;

	public static bool skinIzm = false;

	private void Start()
	{
		storoniForRedactController = GetComponent<ViborStoroniForRedact>();
		spisokSkinovController = GetComponent<SpisokSkinov>();
		mainController = GetComponent<Controller>();
		arrChastiTela = new ArrayList();
		mainController.previewControl._partEslected = shooseChastTela;
		poleVvoda.fontSize = Mathf.RoundToInt(35f * koefMashtab);
		rectDialogSave = new Rect((float)Screen.width * 0.5f - (float)oknoVvodaName.width * 0.5f * koefMashtab, 0f, (float)oknoVvodaName.width * koefMashtab, (float)oknoVvodaName.height * koefMashtab);
		rectExitBezSave = new Rect((float)Screen.width * 0.5f - (float)plashkaExitBezSave.width * 0.5f * koefMashtab, (float)Screen.height * 0.5f - (float)plashkaExitBezSave.height * 0.5f * koefMashtab, (float)plashkaExitBezSave.width * koefMashtab, (float)plashkaExitBezSave.height * koefMashtab);
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
		GUI.DrawTexture(new Rect((float)Screen.width * 0.5f - (float)title.width * 0.5f * koefMashtab, 30f * koefMashtab, (float)title.width * koefMashtab, (float)title.height * koefMashtab), title);
		GUI.DrawTexture(new Rect((float)Screen.width * 0.5f - (float)podTitle.width * 0.5f * koefMashtab, 110f * koefMashtab, (float)podTitle.width * koefMashtab, (float)podTitle.height * koefMashtab), podTitle);
		GUI.DrawTexture(new Rect((float)Screen.width * 0.5f - (float)nadPlaskoi.width * 0.5f * koefMashtab, (float)Screen.height - 150f * koefMashtab, (float)nadPlaskoi.width * koefMashtab, (float)nadPlaskoi.height * koefMashtab), nadPlaskoi);
		GUI.DrawTexture(new Rect(0f, (float)Screen.height - (float)plashkaNiz.height * koefMashtab, Screen.width, (float)plashkaNiz.height * koefMashtab), plashkaNiz);
		if (GUI.Button(new Rect(55f * koefMashtab, (float)Screen.height - (9f + (float)butBack.normal.background.height) * koefMashtab, (float)butBack.normal.background.width * koefMashtab, (float)butBack.normal.background.height * koefMashtab), string.Empty, butBack) && dialogSaveNeActiv && !exitBezSaveActiv)
		{
			if (skinIzm)
			{
				exitBezSaveActiv = true;
				mainController.previewControl.Locked = true;
			}
			else
			{
				exit();
			}
		}
		if (GUI.Button(new Rect((float)Screen.width - 55f * koefMashtab - (float)butSave.normal.background.width * koefMashtab, (float)Screen.height - (9f + (float)butSave.normal.background.height) * koefMashtab, (float)butSave.normal.background.width * koefMashtab, (float)butSave.normal.background.height * koefMashtab), string.Empty, butSave) && dialogSaveNeActiv && !exitBezSaveActiv)
		{
			nameNewSkin = string.Empty;
			dialogSaveNeActiv = false;
		}
		if (!dialogSaveNeActiv)
		{
			GUI.DrawTexture(rectDialogSave, oknoVvodaName);
			nameNewSkin = GUI.TextField(new Rect(rectDialogSave.x + 55f * koefMashtab, rectDialogSave.y + rectDialogSave.height * 0.5f - 40f * koefMashtab, rectDialogSave.width - 110f * koefMashtab, 50f * koefMashtab), nameNewSkin, poleVvoda);
			if (GUI.Button(new Rect(rectDialogSave.x + 55f * koefMashtab, rectDialogSave.y + rectDialogSave.height - 125f * koefMashtab, (float)butDlgCancel.normal.background.width * koefMashtab, (float)butDlgCancel.normal.background.height * koefMashtab), string.Empty, butDlgCancel))
			{
				dialogSaveNeActiv = true;
			}
			if (GUI.Button(new Rect(rectDialogSave.x + rectDialogSave.width - 55f * koefMashtab - (float)butDlgOk.normal.background.width * koefMashtab, rectDialogSave.y + rectDialogSave.height - 125f * koefMashtab, (float)butDlgOk.normal.background.width * koefMashtab, (float)butDlgOk.normal.background.height * koefMashtab), string.Empty, butDlgOk))
			{
				int num = Load.LoadInt("nomSkinForSave");
				string text = "Skin_" + num;
				SkinsManager.SaveTextureWithName(sobratSkinIzArr(), text);
				spisokSkinovController.arrTitleSkin.Add(nameNewSkin);
				spisokSkinovController.arrNameSkin.Add(text);
				num++;
				Save.SaveInt("nomSkinForSave", num);
				string[] variable = spisokSkinovController.arrNameSkin.ToArray(typeof(string)) as string[];
				string[] variable2 = spisokSkinovController.arrTitleSkin.ToArray(typeof(string)) as string[];
				Save.SaveStringArray("arrNameSkin", variable);
				Save.SaveStringArray("arrTitleSkin", variable2);
				mainController.previewControl.updateSpisok();
				dialogSaveNeActiv = true;
				mainController.previewControl.ShowSkin(spisokSkinovController.arrNameSkin.Count - 1);
				showEnabled = false;
				spisokSkinovController.showEnabled = true;
				skinIzm = false;
				spisokSkinovController.showMsg();
			}
		}
		if (exitBezSaveActiv)
		{
			GUI.DrawTexture(rectExitBezSave, plashkaExitBezSave);
			if (GUI.Button(new Rect(rectExitBezSave.x + 55f * koefMashtab, rectExitBezSave.y + rectExitBezSave.height - 125f * koefMashtab, (float)butDlgCancel.normal.background.width * koefMashtab, (float)butDlgCancel.normal.background.height * koefMashtab), string.Empty, butDlgCancel))
			{
				mainController.previewControl.Locked = false;
				exitBezSaveActiv = false;
			}
			if (GUI.Button(new Rect(rectExitBezSave.x + rectExitBezSave.width - 55f * koefMashtab - (float)butDlgOk.normal.background.width * koefMashtab, rectExitBezSave.y + rectExitBezSave.height - 125f * koefMashtab, (float)butDlgOk.normal.background.width * koefMashtab, (float)butDlgOk.normal.background.height * koefMashtab), string.Empty, butDlgOk))
			{
				exit();
			}
		}
	}

	public void shooseChastTela(int chastTela)
	{
		if (dialogSaveNeActiv && !exitBezSaveActiv)
		{
			Debug.Log("chastTela=" + chastTela);
			storoniForRedactController.shooseChastTela(chastTela);
			mainController.objPeople.active = false;
			showEnabled = false;
			storoniForRedactController.showEnabled = true;
		}
	}

	public void cutSkin(int tekSkinNum)
	{
		if (tekSkinNum >= 0)
		{
			skinForRedact = SkinsManager.TextureForName((string)spisokSkinovController.arrNameSkin[tekSkinNum]);
		}
		else
		{
			skinForRedact = clearSkin;
			PreviewController.SetTextureRecursivelyFrom(mainController.objPeople, skinForRedact);
		}
		nomSkinaForRedact = tekSkinNum;
		Texture2D texForCut = skinForRedact;
		if (arrChastiTela.Count > 0)
		{
			arrChastiTela.Clear();
		}
		arrChastiTela.Add(getTexFromTexByRect(texForCut, new Rect(8f, 24f, 8f, 8f)));
		arrChastiTela.Add(getTexFromTexByRect(texForCut, new Rect(16f, 24f, 8f, 8f)));
		arrChastiTela.Add(getTexFromTexByRect(texForCut, new Rect(0f, 16f, 8f, 8f)));
		arrChastiTela.Add(getTexFromTexByRect(texForCut, new Rect(8f, 16f, 8f, 8f)));
		arrChastiTela.Add(getTexFromTexByRect(texForCut, new Rect(16f, 16f, 8f, 8f)));
		arrChastiTela.Add(getTexFromTexByRect(texForCut, new Rect(24f, 16f, 8f, 8f)));
		arrChastiTela.Add(getTexFromTexByRect(texForCut, new Rect(4f, 12f, 4f, 4f)));
		arrChastiTela.Add(getTexFromTexByRect(texForCut, new Rect(8f, 12f, 4f, 4f)));
		arrChastiTela.Add(getTexFromTexByRect(texForCut, new Rect(0f, 0f, 4f, 12f)));
		arrChastiTela.Add(getTexFromTexByRect(texForCut, new Rect(4f, 0f, 4f, 12f)));
		arrChastiTela.Add(getTexFromTexByRect(texForCut, new Rect(8f, 0f, 4f, 12f)));
		arrChastiTela.Add(getTexFromTexByRect(texForCut, new Rect(12f, 0f, 4f, 12f)));
		arrChastiTela.Add(getTexFromTexByRect(texForCut, new Rect(20f, 12f, 8f, 4f)));
		arrChastiTela.Add(getTexFromTexByRect(texForCut, new Rect(28f, 12f, 8f, 4f)));
		arrChastiTela.Add(getTexFromTexByRect(texForCut, new Rect(16f, 0f, 4f, 12f)));
		arrChastiTela.Add(getTexFromTexByRect(texForCut, new Rect(20f, 0f, 8f, 12f)));
		arrChastiTela.Add(getTexFromTexByRect(texForCut, new Rect(28f, 0f, 4f, 12f)));
		arrChastiTela.Add(getTexFromTexByRect(texForCut, new Rect(32f, 0f, 8f, 12f)));
		arrChastiTela.Add(getTexFromTexByRect(texForCut, new Rect(44f, 12f, 4f, 4f)));
		arrChastiTela.Add(getTexFromTexByRect(texForCut, new Rect(48f, 12f, 4f, 4f)));
		arrChastiTela.Add(getTexFromTexByRect(texForCut, new Rect(40f, 0f, 4f, 12f)));
		arrChastiTela.Add(getTexFromTexByRect(texForCut, new Rect(44f, 0f, 4f, 12f)));
		arrChastiTela.Add(getTexFromTexByRect(texForCut, new Rect(48f, 0f, 4f, 12f)));
		arrChastiTela.Add(getTexFromTexByRect(texForCut, new Rect(52f, 0f, 4f, 12f)));
	}

	public void setClearSkin()
	{
		PreviewController.SetTextureRecursivelyFrom(mainController.objPeople, skinForRedact);
	}

	private Texture2D getTexFromTexByRect(Texture2D texForCut, Rect rectForCut)
	{
		Color[] pixels = texForCut.GetPixels((int)rectForCut.x, (int)rectForCut.y, (int)rectForCut.width, (int)rectForCut.height);
		Texture2D texture2D = new Texture2D((int)rectForCut.width, (int)rectForCut.height);
		texture2D.filterMode = FilterMode.Point;
		texture2D.SetPixels(pixels);
		texture2D.Apply();
		return texture2D;
	}

	public Texture2D sobratSkinIzArr()
	{
		Texture2D texture2D = new Texture2D(64, 32);
		Color color = new Color(0f, 0f, 0f, 0f);
		for (int i = 0; i < 32; i++)
		{
			for (int j = 0; j < 64; j++)
			{
				texture2D.SetPixel(j, i, color);
			}
		}
		texture2D.SetPixels(8, 24, 8, 8, ((Texture2D)arrChastiTela[0]).GetPixels());
		texture2D.SetPixels(16, 24, 8, 8, ((Texture2D)arrChastiTela[1]).GetPixels());
		texture2D.SetPixels(0, 16, 8, 8, ((Texture2D)arrChastiTela[2]).GetPixels());
		texture2D.SetPixels(8, 16, 8, 8, ((Texture2D)arrChastiTela[3]).GetPixels());
		texture2D.SetPixels(16, 16, 8, 8, ((Texture2D)arrChastiTela[4]).GetPixels());
		texture2D.SetPixels(24, 16, 8, 8, ((Texture2D)arrChastiTela[5]).GetPixels());
		texture2D.SetPixels(4, 12, 4, 4, ((Texture2D)arrChastiTela[6]).GetPixels());
		texture2D.SetPixels(8, 12, 4, 4, ((Texture2D)arrChastiTela[7]).GetPixels());
		texture2D.SetPixels(0, 0, 4, 12, ((Texture2D)arrChastiTela[8]).GetPixels());
		texture2D.SetPixels(4, 0, 4, 12, ((Texture2D)arrChastiTela[9]).GetPixels());
		texture2D.SetPixels(8, 0, 4, 12, ((Texture2D)arrChastiTela[10]).GetPixels());
		texture2D.SetPixels(12, 0, 4, 12, ((Texture2D)arrChastiTela[11]).GetPixels());
		texture2D.SetPixels(20, 12, 8, 4, ((Texture2D)arrChastiTela[12]).GetPixels());
		texture2D.SetPixels(28, 12, 8, 4, ((Texture2D)arrChastiTela[13]).GetPixels());
		texture2D.SetPixels(16, 0, 4, 12, ((Texture2D)arrChastiTela[14]).GetPixels());
		texture2D.SetPixels(20, 0, 8, 12, ((Texture2D)arrChastiTela[15]).GetPixels());
		texture2D.SetPixels(28, 0, 4, 12, ((Texture2D)arrChastiTela[16]).GetPixels());
		texture2D.SetPixels(32, 0, 8, 12, ((Texture2D)arrChastiTela[17]).GetPixels());
		texture2D.SetPixels(44, 12, 4, 4, ((Texture2D)arrChastiTela[18]).GetPixels());
		texture2D.SetPixels(48, 12, 4, 4, ((Texture2D)arrChastiTela[19]).GetPixels());
		texture2D.SetPixels(40, 0, 4, 12, ((Texture2D)arrChastiTela[20]).GetPixels());
		texture2D.SetPixels(44, 0, 4, 12, ((Texture2D)arrChastiTela[21]).GetPixels());
		texture2D.SetPixels(48, 0, 4, 12, ((Texture2D)arrChastiTela[22]).GetPixels());
		texture2D.SetPixels(52, 0, 4, 12, ((Texture2D)arrChastiTela[23]).GetPixels());
		texture2D.filterMode = FilterMode.Point;
		texture2D.Apply();
		return texture2D;
	}

	private void exit()
	{
		mainController.previewControl.Locked = false;
		exitBezSaveActiv = false;
		showEnabled = false;
		if (nomSkinaForRedact >= 0)
		{
			mainController.previewControl.ShowSkin(nomSkinaForRedact);
			spisokSkinovController.showEnabled = true;
		}
		else
		{
			mainController.showEnabled = true;
		}
	}
}
