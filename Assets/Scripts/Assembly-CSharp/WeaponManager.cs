using System.Collections;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
	private Object[] _weaponsInGame;

	private ArrayList _playerWeapons = new ArrayList();

	public int CurrentWeaponIndex;

	private WeaponSounds _currentWeaponSounds;

	public static string _initialWeaponName
	{
		get
		{
			return "FirstPistol";
		}
	}

	public static string PickWeaponName
	{
		get
		{
			return "Weapon6";
		}
	}

	public static string SwordWeaponName
	{
		get
		{
			return "Weapon7";
		}
	}

	public Object[] weaponsInGame
	{
		get
		{
			return _weaponsInGame;
		}
	}

	public ArrayList playerWeapons
	{
		get
		{
			return _playerWeapons;
		}
	}

	public WeaponSounds currentWeaponSounds
	{
		get
		{
			return _currentWeaponSounds;
		}
		set
		{
			_currentWeaponSounds = value;
		}
	}

	private Object[] GetWeaponPrefabs()
	{
		return Resources.LoadAll<GameObject>("Weapons");
	}

	public void Reset()
	{
		_playerWeapons.Clear();
		CurrentWeaponIndex = 0;
		Object[] array = weaponsInGame;
		for (int i = 0; i < array.Length; i++)
		{
			GameObject gameObject = (GameObject)array[i];
			if (gameObject.CompareTag(_initialWeaponName))
			{
				Weapon weapon = new Weapon();
				weapon.weaponPrefab = gameObject;
				weapon.currentAmmoInBackpack = weapon.weaponPrefab.GetComponent<WeaponSounds>().InitialAmmo;
				weapon.currentAmmoInClip = weapon.weaponPrefab.GetComponent<WeaponSounds>().ammoInClip;
				_playerWeapons.Add(weapon);
				break;
			}
		}
		if (PlayerPrefs.GetInt(Defs.MinerWeaponSett, 0) > 0)
		{
			Object[] array2 = weaponsInGame;
			for (int j = 0; j < array2.Length; j++)
			{
				GameObject gameObject2 = (GameObject)array2[j];
				if (gameObject2.name.Equals(PickWeaponName))
				{
					Weapon weapon2 = new Weapon();
					weapon2.weaponPrefab = gameObject2;
					weapon2.currentAmmoInBackpack = 0;
					weapon2.currentAmmoInClip = 0;
					_playerWeapons.Add(weapon2);
					break;
				}
			}
		}
		if (PlayerPrefs.GetInt(Defs.SwordSett, 0) <= 0)
		{
			return;
		}
		Object[] array3 = weaponsInGame;
		for (int k = 0; k < array3.Length; k++)
		{
			GameObject gameObject3 = (GameObject)array3[k];
			if (gameObject3.name.Equals(SwordWeaponName))
			{
				Weapon weapon3 = new Weapon();
				weapon3.weaponPrefab = gameObject3;
				weapon3.currentAmmoInBackpack = 0;
				weapon3.currentAmmoInClip = 0;
				_playerWeapons.Add(weapon3);
				break;
			}
		}
	}

	public bool AddWeapon(GameObject weaponPrefab, out int score)
	{
		score = 0;
		foreach (Weapon playerWeapon in playerWeapons)
		{
			if (playerWeapon.weaponPrefab.CompareTag(weaponPrefab.tag))
			{
				int idx = playerWeapons.IndexOf(playerWeapon);
				if (!AddAmmo(idx))
				{
					score += Defs.ScoreForSurplusAmmo;
				}
				return false;
			}
		}
		Weapon weapon2 = new Weapon();
		weapon2.weaponPrefab = weaponPrefab;
		weapon2.currentAmmoInBackpack = weapon2.weaponPrefab.GetComponent<WeaponSounds>().InitialAmmo;
		weapon2.currentAmmoInClip = weapon2.weaponPrefab.GetComponent<WeaponSounds>().ammoInClip;
		playerWeapons.Add(weapon2);
		playerWeapons.Sort(new WeaponsComparer());
		CurrentWeaponIndex = playerWeapons.IndexOf(weapon2);
		return true;
	}

	public GameObject GetPickPrefab()
	{
		Object[] array = weaponsInGame;
		for (int i = 0; i < array.Length; i++)
		{
			GameObject gameObject = (GameObject)array[i];
			if (gameObject.name.Equals(PickWeaponName))
			{
				return gameObject;
			}
		}
		return null;
	}

	public GameObject GetSwordPrefab()
	{
		Object[] array = weaponsInGame;
		for (int i = 0; i < array.Length; i++)
		{
			GameObject gameObject = (GameObject)array[i];
			if (gameObject.name.Equals(SwordWeaponName))
			{
				return gameObject;
			}
		}
		return null;
	}

	public bool AddAmmo(int idx = -1)
	{
		if (idx == -1)
		{
			idx = CurrentWeaponIndex;
		}
		if (idx == CurrentWeaponIndex && currentWeaponSounds.isMelee)
		{
			return false;
		}
		Weapon weapon = (Weapon)playerWeapons[idx];
		WeaponSounds component = weapon.weaponPrefab.GetComponent<WeaponSounds>();
		if (weapon.currentAmmoInBackpack < component.MaxAmmoWithRespectToInApp)
		{
			weapon.currentAmmoInBackpack += component.ammoInClip;
			if (weapon.currentAmmoInBackpack > component.MaxAmmoWithRespectToInApp)
			{
				weapon.currentAmmoInBackpack = component.MaxAmmoWithRespectToInApp;
			}
			return true;
		}
		return false;
	}

	public void SetMaxAmmoFrAllWeapons()
	{
		foreach (Weapon playerWeapon in playerWeapons)
		{
			playerWeapon.currentAmmoInClip = playerWeapon.weaponPrefab.GetComponent<WeaponSounds>().ammoInClip;
			playerWeapon.currentAmmoInBackpack = playerWeapon.weaponPrefab.GetComponent<WeaponSounds>().MaxAmmoWithRespectToInApp;
		}
	}

	private void Start()
	{
		Object.DontDestroyOnLoad(base.gameObject);
		_weaponsInGame = GetWeaponPrefabs();
		Reset();
	}

	public void AddMinerWeaponToInventoryAndSaveInApp()
	{
		Player_move_c.SaveMinerWeaponInPrefabs();
		int score;
		AddWeapon(GetPickPrefab(), out score);
	}

	public void AddSwordToInventoryAndSaveInApp()
	{
		Player_move_c.SaveSwordInPrefs();
		int score;
		AddWeapon(GetSwordPrefab(), out score);
	}

	private void Update()
	{
	}

	public void Reload()
	{
		currentWeaponSounds.animationObject.GetComponent<Animation>().Stop("Empty");
		currentWeaponSounds.animationObject.GetComponent<Animation>().CrossFade("Shoot");
		currentWeaponSounds.animationObject.GetComponent<Animation>().Play("Reload");
		int num = currentWeaponSounds.ammoInClip - ((Weapon)playerWeapons[CurrentWeaponIndex]).currentAmmoInClip;
		if (((Weapon)playerWeapons[CurrentWeaponIndex]).currentAmmoInBackpack >= num)
		{
			((Weapon)playerWeapons[CurrentWeaponIndex]).currentAmmoInClip += num;
			((Weapon)playerWeapons[CurrentWeaponIndex]).currentAmmoInBackpack -= num;
		}
		else
		{
			((Weapon)playerWeapons[CurrentWeaponIndex]).currentAmmoInClip += ((Weapon)playerWeapons[CurrentWeaponIndex]).currentAmmoInBackpack;
			((Weapon)playerWeapons[CurrentWeaponIndex]).currentAmmoInBackpack = 0;
		}
	}
}
