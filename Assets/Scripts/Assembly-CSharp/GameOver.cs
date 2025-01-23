using UnityEngine;

public class GameOver : MonoBehaviour
{
	public Texture elixir;

	public Texture noElixir;

	public GUIStyle resurrect;

	public GUIStyle retry;

	public GUIStyle quit;

	public GUIStyle decline;

	public GUIStyle buy;

	private bool haveNoElixirSh;

	private float coef = (float)Screen.height / 768f;

	private void Start()
	{
	}

	private void OnEnable()
	{
		GoogleIABManager.purchaseSucceededEvent += ElixirBuyAndr;
	}

	private void OnDisable()
	{
		GoogleIABManager.purchaseSucceededEvent -= ElixirBuyAndr;
	}

	private void ElixirBuyAndr(GooglePurchase purchase)
	{
		if (purchase.productId.Equals(StoreKitEventListener.elixirID))
		{
			_Resurrect();
		}
	}

	private void _Resurrect()
	{
		PlayerPrefs.SetInt(Defs.NumberOfElixirsSett, PlayerPrefs.GetInt(Defs.NumberOfElixirsSett, 1) - 1);
		PlayerPrefs.Save();
		WeaponManager component = GameObject.FindGameObjectWithTag("WeaponManager").GetComponent<WeaponManager>();
		foreach (Weapon playerWeapon in component.playerWeapons)
		{
			WeaponSounds component2 = playerWeapon.weaponPrefab.GetComponent<WeaponSounds>();
			if (playerWeapon.currentAmmoInClip + playerWeapon.currentAmmoInBackpack < component2.InitialAmmo + component2.ammoInClip)
			{
				playerWeapon.currentAmmoInClip = component2.ammoInClip;
				playerWeapon.currentAmmoInBackpack = component2.InitialAmmo;
			}
		}
		GlobalGameController.currentLevel--;
		PlayerPrefs.SetInt(Defs.CurrentHealthSett, Player_move_c.MaxPlayerHealth);
		Application.LoadLevel("Loading");
	}

	private void _Retry()
	{
	}

	private void _Buy()
	{
		GoogleIAB.purchaseProduct(StoreKitEventListener.elixirID);
	}

	private void OnGUI()
	{
		int depth = GUI.depth;
		float num = (float)Screen.width * 0.31f * coef;
		float num2 = num * ((float)resurrect.normal.background.height / (float)resurrect.normal.background.width);
		float num3 = num2 * 0.2f;
		Rect position = new Rect((float)(Screen.width / 2) - num / 2f, (float)Screen.height - num2 * 3f - num3 * 3f, num, num2);
		GUI.enabled = !haveNoElixirSh;
		if (GUI.Button(position, string.Empty, resurrect))
		{
			if (PlayerPrefs.GetInt(Defs.NumberOfElixirsSett, 1) > 0)
			{
				_Resurrect();
			}
			else
			{
				haveNoElixirSh = true;
			}
		}
		GUI.enabled = !haveNoElixirSh;
		if (GUI.Button(new Rect((float)(Screen.width / 2) - num / 2f, (float)Screen.height - num2 * 2f - num3 * 2f, num, num2), string.Empty, retry))
		{
			GUI.enabled = true;
			GlobalGameController.ResetParameters();
			GlobalGameController.Score = 0;
			GameObject.FindGameObjectWithTag("WeaponManager").GetComponent<WeaponManager>().Reset();
			PlayerPrefs.SetInt(Defs.CurrentHealthSett, Player_move_c.MaxPlayerHealth);
			Application.LoadLevel("LoadingNoWait");
		}
		float num4 = num * ((float)quit.normal.background.width / (float)resurrect.normal.background.width);
		float height = num4 * ((float)quit.normal.background.height / (float)quit.normal.background.width);
		if (GUI.Button(new Rect((float)Screen.width * 0.491f - num4 / 2f, (float)Screen.height - num2 - num3 * 1f, num4, height), string.Empty, quit))
		{
			GUI.enabled = true;
			Application.LoadLevel("Restart");
		}
		float num5 = num * 0.4f;
		float num6 = num5 * ((float)elixir.height / (float)elixir.width);
		GUI.DrawTexture(new Rect(position.x + position.width + num3, position.y + position.height / 2f - num6 / 2f, num5, num6), elixir);
		if (haveNoElixirSh)
		{
			GUI.enabled = true;
			float num7 = (float)Screen.width * 0.45f * coef;
			float num8 = num7 * ((float)noElixir.height / (float)noElixir.width);
			float num9 = num7 * 0.27f;
			float num10 = num9 * ((float)buy.normal.background.height / (float)buy.normal.background.width);
			float num11 = num9 / 10f;
			float num12 = num10;
			GUI.DrawTexture(new Rect((float)(Screen.width / 2) - num7 / 2f, (float)(Screen.height / 2) - num8 / 2f, num7, num8), noElixir);
			if (GUI.Button(new Rect((float)(Screen.width / 2) + num11, (float)(Screen.height / 2) + num12, num9, num10), string.Empty, buy))
			{
				haveNoElixirSh = false;
				_Buy();
			}
			if (haveNoElixirSh && GUI.Button(new Rect((float)(Screen.width / 2) - num11 - num9, (float)(Screen.height / 2) + num12, num9, num10), string.Empty, decline))
			{
				haveNoElixirSh = false;
			}
			GUI.enabled = false;
		}
		GUI.depth = depth;
	}
}
