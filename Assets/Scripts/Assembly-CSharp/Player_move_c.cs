using System.Collections;
using UnityEngine;

public class Player_move_c : MonoBehaviour
{
	public GUISkin MySkin;

	public Texture2D ammoTexture;

	public Texture2D scoreTexture;

	public Texture2D enemiesTxture;

	public Texture HeartTexture;

	public Texture buyTexture;

	public Texture2D AimTexture;

	public int AimTextureWidth = 50;

	public int AimTextureHeight = 50;

	private Transform GunFlash;

	public int BulletForce = 5000;

	public GameObject renderAllObjectPrefab;

	private Texture zaglushkaTexture;

	public GUIStyle labelStyle;

	private bool productPurchased;

	public Texture minerWeaponSoldTexture;

	public Texture swordSoldTexture;

	public Texture hasElixirTexture;

	public Texture elixir;

	public GUIStyle elixirsCountStyle;

	private string[] productIdentifiers = new string[5]
	{
		StoreKitEventListener.fullHealthID,
		StoreKitEventListener.bigAmmoPackID,
		StoreKitEventListener.crystalswordID,
		StoreKitEventListener.minerWeaponID,
		StoreKitEventListener.elixirID
	};

	private GameObject _leftJoystick;

	private GameObject _rightJoystick;

	private int _curHealth;

	private float _timeWhenPurchShown;

	public int MaxHealth;

	public int AmmoBoxWidth = 100;

	public int AmmoBoxHeight = 100;

	public int AmmoBoxOffset = 10;

	public int ScoreBoxWidth = 100;

	public int ScoreBoxHeight = 100;

	public int ScoreBoxOffset = 10;

	private float GunFlashLifetime;

	public GUIStyle ScoreBox;

	public GUIStyle EnemiesBox;

	public GUIStyle AmmoBox;

	public GUIStyle HealthBox;

	public GUIStyle pauseStyle;

	public GUIStyle pauseFonStyle;

	public GUIStyle resumeStyle;

	public GUIStyle menuStyle;

	public GUIStyle soundStyle;

	public GUIStyle buyStyle;

	public GUIStyle resumePauseStyle;

	public GUIStyle enemiesLeftStyle;

	private GameObject damage;

	private bool damageShown;

	public Texture pauseFon;

	private Pauser _pauser;

	public Texture pauseTitle;

	private GameObject _gameController;

	private bool _backWasPressed;

	private GameObject _player;

	public GameObject bulletPrefab;

	private GameObject _bulletSpawnPoint;

	private GameObject _inAppGameObject;

	private StoreKitEventListener _listener;

	public Texture inAppFon;

	public GUIStyle puliInApp;

	public GUIStyle healthInApp;

	public GUIStyle pulemetInApp;

	public GUIStyle crystalSwordInapp;

	public GUIStyle elixirInapp;

	public bool isInappWinOpen;

	private ZombieCreator _zombieCreator;

	public WeaponManager _weaponManager;

	private Vector2 leftFingerPos = Vector2.zero;

	private Vector2 leftFingerLastPos = Vector2.zero;

	private Vector2 leftFingerMovedBy = Vector2.zero;

	private bool canReceiveSwipes = true;

	public float slideMagnitudeX;

	public float slideMagnitudeY;

	public AudioClip ChangeWeaponClip;

	private float height = (float)Screen.height * 0.078f;

	private float _width = 125f;

	public static int MaxPlayerHealth
	{
		get
		{
			return 9;
		}
	}

	public int CurHealth
	{
		get
		{
			return _curHealth;
		}
		set
		{
			_curHealth = value;
		}
	}

	public int curHealthProp
	{
		get
		{
			return CurHealth;
		}
		set
		{
			if (CurHealth > value && !damageShown)
			{
				StartCoroutine(FlashWhenHit());
			}
			CurHealth = value;
		}
	}

	public static int FontSizeForMessages
	{
		get
		{
			return Mathf.RoundToInt((float)Screen.height * 0.03f);
		}
	}

	private void WalkAnimation()
	{
		if ((bool)_weaponManager)
		{
			_weaponManager.currentWeaponSounds.animationObject.GetComponent<Animation>().CrossFade("Walk");
		}
	}

	private void IdleAnimation()
	{
		if ((bool)_weaponManager)
		{
			_weaponManager.currentWeaponSounds.animationObject.GetComponent<Animation>().CrossFade("Idle");
		}
	}

	public void AddWeapon(GameObject weaponPrefab)
	{
		int score;
		if (_weaponManager.AddWeapon(weaponPrefab, out score))
		{
			ChangeWeapon(_weaponManager.CurrentWeaponIndex, false);
			return;
		}
		if (weaponPrefab != _weaponManager.GetPickPrefab() && weaponPrefab != _weaponManager.GetSwordPrefab())
		{
			GlobalGameController.Score += score;
			base.gameObject.GetComponent<AudioSource>().PlayOneShot(ChangeWeaponClip);
			return;
		}
		foreach (Weapon playerWeapon in _weaponManager.playerWeapons)
		{
			if (playerWeapon.weaponPrefab == weaponPrefab)
			{
				ChangeWeapon(_weaponManager.playerWeapons.IndexOf(playerWeapon), false);
				break;
			}
		}
	}

	public void ChangeWeapon(int index, bool shouldSetMaxAmmo = true)
	{
		Quaternion rotation = _player.transform.rotation;
		if ((bool)_weaponManager.currentWeaponSounds)
		{
			rotation = _weaponManager.currentWeaponSounds.gameObject.transform.rotation;
			_SetGunFlashActive(false);
			_weaponManager.currentWeaponSounds.gameObject.transform.parent = null;
			Object.Destroy(_weaponManager.currentWeaponSounds.gameObject);
			_weaponManager.currentWeaponSounds = null;
		}
		GameObject gameObject = (GameObject)Object.Instantiate(((Weapon)_weaponManager.playerWeapons[index]).weaponPrefab, Vector3.zero, Quaternion.identity);
		gameObject.transform.parent = base.gameObject.transform;
		gameObject.transform.rotation = rotation;
		_weaponManager.CurrentWeaponIndex = index;
		_weaponManager.currentWeaponSounds = gameObject.GetComponent<WeaponSounds>();
		gameObject.transform.position = gameObject.transform.parent.TransformPoint(_weaponManager.currentWeaponSounds.gunPosition);
		_rightJoystick.SendMessage("setSeriya", _weaponManager.currentWeaponSounds.isSerialShooting);
		if (shouldSetMaxAmmo)
		{
		}
		if (((Weapon)_weaponManager.playerWeapons[_weaponManager.CurrentWeaponIndex]).currentAmmoInClip > 0 || _weaponManager.currentWeaponSounds.isMelee)
		{
			_rightJoystick.SendMessage("HasAmmo");
		}
		else
		{
			_rightJoystick.SendMessage("NoAmmo");
		}
		_weaponManager.currentWeaponSounds.animationObject.GetComponent<Animation>()["Reload"].layer = 1;
		_weaponManager.currentWeaponSounds.animationObject.GetComponent<Animation>()["Shoot"].layer = 1;
		if (!_weaponManager.currentWeaponSounds.isMelee)
		{
			foreach (Transform item in _weaponManager.currentWeaponSounds.gameObject.transform)
			{
				if (item.name.Equals("BulletSpawnPoint"))
				{
					_bulletSpawnPoint = item.gameObject;
					break;
				}
			}
			GunFlash = GameObject.Find("GunFlash").transform;
		}
		SendSpeedModifier();
		base.gameObject.GetComponent<AudioSource>().PlayOneShot(ChangeWeaponClip);
	}

	private void SendSpeedModifier()
	{
		_player.SendMessage("SetSpeedModifier", _weaponManager.currentWeaponSounds.speedModifier);
	}

	public bool NeedAmmo()
	{
		int currentWeaponIndex = _weaponManager.CurrentWeaponIndex;
		Weapon weapon = (Weapon)_weaponManager.playerWeapons[currentWeaponIndex];
		return weapon.currentAmmoInBackpack < _weaponManager.currentWeaponSounds.MaxAmmoWithRespectToInApp;
	}

	private void OnEnable()
	{
	}

	private void OnDisable()
	{
	}

	private void Start()
	{
		_inAppGameObject = GameObject.FindGameObjectWithTag("InAppGameObject");
		_listener = _inAppGameObject.GetComponent<StoreKitEventListener>();
		GoogleIABManager.purchaseSucceededEvent += purchaseSuccessful;
		GoogleIABManager.consumePurchaseSucceededEvent += consumptionSucceeded;
		_player = GameObject.FindGameObjectWithTag("Player");
		_weaponManager = GameObject.FindGameObjectWithTag("WeaponManager").GetComponent<WeaponManager>();
		GameObject original = Resources.Load("Damage") as GameObject;
		damage = (GameObject)Object.Instantiate(original);
		_gameController = GameObject.FindGameObjectWithTag("GameController");
		_zombieCreator = _gameController.GetComponent<ZombieCreator>();
		_pauser = GameObject.FindGameObjectWithTag("GameController").GetComponent<Pauser>();
		_leftJoystick = GameObject.Find("LeftTouchPad");
		_rightJoystick = GameObject.Find("RightTouchPad");
		ChangeWeapon(_weaponManager.CurrentWeaponIndex, false);
		_weaponManager.currentWeaponSounds.animationObject.GetComponent<Animation>()["Reload"].layer = 1;
		_weaponManager.currentWeaponSounds.animationObject.GetComponent<Animation>().Stop();
		_SetGunFlashActive(false);
		CurHealth = PlayerPrefs.GetInt(Defs.CurrentHealthSett, MaxPlayerHealth);
		Color color = damage.GetComponent<GUITexture>().color;
		color.a = 0f;
		damage.GetComponent<GUITexture>().color = color;
		Invoke("SendSpeedModifier", 0.5f);
		GameObject gameObject = (GameObject)Object.Instantiate(renderAllObjectPrefab, Vector3.zero, Quaternion.identity);
	}

	private void OnDestroy()
	{
		GoogleIABManager.purchaseSucceededEvent -= purchaseSuccessful;
		GoogleIABManager.consumePurchaseSucceededEvent -= consumptionSucceeded;
	}

	private void _SetGunFlashActive(bool state)
	{
		if (!_weaponManager.currentWeaponSounds.isMelee && GunFlash != null)
		{
			GunFlash.gameObject.SetActive(state);
		}
	}

	private void ReloadPressed()
	{
		if (!_weaponManager.currentWeaponSounds.isMelee && _weaponManager.CurrentWeaponIndex >= 0 && _weaponManager.CurrentWeaponIndex < _weaponManager.playerWeapons.Count && ((Weapon)_weaponManager.playerWeapons[_weaponManager.CurrentWeaponIndex]).currentAmmoInBackpack > 0 && ((Weapon)_weaponManager.playerWeapons[_weaponManager.CurrentWeaponIndex]).currentAmmoInClip != _weaponManager.currentWeaponSounds.ammoInClip)
		{
			_weaponManager.Reload();
			base.GetComponent<AudioSource>().PlayOneShot(_weaponManager.currentWeaponSounds.reload);
			_rightJoystick.SendMessage("HasAmmo");
		}
	}

	private void ShotPressed()
	{
		if (_weaponManager.currentWeaponSounds.animationObject.GetComponent<Animation>().IsPlaying("Shoot") || _weaponManager.currentWeaponSounds.animationObject.GetComponent<Animation>().IsPlaying("Reload"))
		{
			return;
		}
		_weaponManager.currentWeaponSounds.animationObject.GetComponent<Animation>().Stop();
		if (_weaponManager.currentWeaponSounds.isMelee)
		{
			_Shot();
		}
		else if (((Weapon)_weaponManager.playerWeapons[_weaponManager.CurrentWeaponIndex]).currentAmmoInClip > 0)
		{
			((Weapon)_weaponManager.playerWeapons[_weaponManager.CurrentWeaponIndex]).currentAmmoInClip--;
			if (((Weapon)_weaponManager.playerWeapons[_weaponManager.CurrentWeaponIndex]).currentAmmoInClip == 0)
			{
				_rightJoystick.SendMessage("NoAmmo");
			}
			_Shot();
			_SetGunFlashActive(true);
			GunFlashLifetime = 0.1f;
		}
		else
		{
			_weaponManager.currentWeaponSounds.animationObject.GetComponent<Animation>().Play("Empty");
			base.GetComponent<AudioSource>().PlayOneShot(_weaponManager.currentWeaponSounds.empty);
		}
	}

	private void _Shot()
	{
		_weaponManager.currentWeaponSounds.animationObject.GetComponent<Animation>().Play("Shoot");
		shootS();
		base.GetComponent<AudioSource>().PlayOneShot(_weaponManager.currentWeaponSounds.shoot);
	}

	public void shootS()
	{
		if (!_weaponManager.currentWeaponSounds.isMelee)
		{
			GameObject gameObject = (GameObject)Object.Instantiate(bulletPrefab, _bulletSpawnPoint.transform.position, Quaternion.LookRotation(Camera.main.transform.TransformDirection(Vector3.forward)));
			Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0f));
			RaycastHit hitInfo;
			if (Physics.Raycast(ray, out hitInfo, 100f, -5) && (bool)hitInfo.collider.gameObject.transform.parent && hitInfo.collider.gameObject.transform.parent.CompareTag("Enemy"))
			{
				BotHealth component = hitInfo.collider.gameObject.transform.parent.GetComponent<BotHealth>();
				component.adjustHealth((float)(-_weaponManager.currentWeaponSounds.damage) + Random.Range(_weaponManager.currentWeaponSounds.damageRange.x, _weaponManager.currentWeaponSounds.damageRange.y), Camera.main.transform);
			}
			return;
		}
		GameObject[] array = GameObject.FindGameObjectsWithTag("Enemy");
		GameObject gameObject2 = null;
		float num = float.PositiveInfinity;
		GameObject[] array2 = array;
		foreach (GameObject gameObject3 in array2)
		{
			Vector3 to = gameObject3.transform.position - _player.transform.position;
			float magnitude = to.magnitude;
			if (magnitude < num && ((magnitude < _weaponManager.currentWeaponSounds.range && Vector3.Angle(_player.transform.forward, to) < 45f) || magnitude < 1.5f))
			{
				num = magnitude;
				gameObject2 = gameObject3;
			}
		}
		if ((bool)gameObject2)
		{
			StartCoroutine(HitByMelee(gameObject2));
		}
	}

	private IEnumerator HitByMelee(GameObject enemyToHit)
	{
		yield return new WaitForSeconds(_weaponManager.currentWeaponSounds.animationObject.GetComponent<Animation>()["Shoot"].length * 0.57f);
		if ((bool)enemyToHit && (bool)enemyToHit.GetComponent<BotHealth>() && enemyToHit.GetComponent<BotHealth>().getIsLife())
		{
			enemyToHit.GetComponent<BotHealth>().adjustHealth((float)(-_weaponManager.currentWeaponSounds.damage) + Random.Range(_weaponManager.currentWeaponSounds.damageRange.x, _weaponManager.currentWeaponSounds.damageRange.y), Camera.main.transform);
		}
	}

	private IEnumerator Fade(float start, float end, float length, GameObject currentObject)
	{
		for (float i = 0f; i < 1f; i += Time.deltaTime / length)
		{
			Color rgba = currentObject.GetComponent<GUITexture>().color;
			rgba.a = Mathf.Lerp(start, end, i);
			currentObject.GetComponent<GUITexture>().color = rgba;
			yield return 0;
			Color rgba_ = currentObject.GetComponent<GUITexture>().color;
			rgba_.a = end;
			currentObject.GetComponent<GUITexture>().color = rgba_;
		}
	}

	private IEnumerator FlashWhenHit()
	{
		damageShown = true;
		Color rgba = damage.GetComponent<GUITexture>().color;
		rgba.a = 0f;
		damage.GetComponent<GUITexture>().color = rgba;
		float danageTime = 0.15f;
		yield return StartCoroutine(Fade(0f, 1f, danageTime, damage));
		yield return new WaitForSeconds(0.01f);
		yield return StartCoroutine(Fade(1f, 0f, danageTime, damage));
		damageShown = false;
	}

	private IEnumerator SetCanReceiveSwipes()
	{
		yield return new WaitForSeconds(0.1f);
		canReceiveSwipes = true;
	}

	private void Update()
	{
		if (!_pauser.paused && canReceiveSwipes && !isInappWinOpen)
		{
			Rect rect = new Rect((float)Screen.width - 264f * (float)Screen.width / 1024f, (float)Screen.height - 94f * (float)Screen.width / 1024f - 95f * (float)Screen.width / 1024f, 264f * (float)Screen.width / 1024f, 95f * (float)Screen.width / 1024f);
			Touch[] touches = Input.touches;
			for (int i = 0; i < touches.Length; i++)
			{
				Touch touch = touches[i];
				if (!rect.Contains(touch.position))
				{
					continue;
				}
				if (touch.phase == TouchPhase.Began)
				{
					leftFingerPos = Vector2.zero;
					leftFingerLastPos = Vector2.zero;
					leftFingerMovedBy = Vector2.zero;
					slideMagnitudeX = 0f;
					slideMagnitudeY = 0f;
					leftFingerPos = touch.position;
				}
				else if (touch.phase == TouchPhase.Moved)
				{
					leftFingerMovedBy = touch.position - leftFingerPos;
					leftFingerLastPos = leftFingerPos;
					leftFingerPos = touch.position;
					slideMagnitudeX = leftFingerMovedBy.x / (float)Screen.width;
					Debug.Log("slideMagnitudeX = " + slideMagnitudeX);
					float num = 0.02f;
					if (slideMagnitudeX > num)
					{
						_weaponManager.CurrentWeaponIndex++;
						int count = _weaponManager.playerWeapons.Count;
						count = ((count == 0) ? 1 : count);
						_weaponManager.CurrentWeaponIndex %= count;
						ChangeWeapon(_weaponManager.CurrentWeaponIndex, false);
						canReceiveSwipes = false;
						StartCoroutine(SetCanReceiveSwipes());
						leftFingerLastPos = leftFingerPos;
						leftFingerPos = touch.position;
						slideMagnitudeX = 0f;
					}
					else if (slideMagnitudeX < 0f - num)
					{
						_weaponManager.CurrentWeaponIndex--;
						if (_weaponManager.CurrentWeaponIndex < 0)
						{
							_weaponManager.CurrentWeaponIndex = _weaponManager.playerWeapons.Count - 1;
						}
						_weaponManager.CurrentWeaponIndex %= _weaponManager.playerWeapons.Count;
						ChangeWeapon(_weaponManager.CurrentWeaponIndex, false);
						canReceiveSwipes = false;
						StartCoroutine(SetCanReceiveSwipes());
						leftFingerLastPos = leftFingerPos;
						leftFingerPos = touch.position;
						slideMagnitudeX = 0f;
					}
					slideMagnitudeY = leftFingerMovedBy.y / (float)Screen.height;
				}
				else if (touch.phase == TouchPhase.Stationary)
				{
					leftFingerLastPos = leftFingerPos;
					leftFingerPos = touch.position;
					slideMagnitudeX = 0f;
					slideMagnitudeY = 0f;
				}
				else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
				{
					slideMagnitudeX = 0f;
					slideMagnitudeY = 0f;
				}
			}
		}
		if (GunFlashLifetime > 0f)
		{
			GunFlashLifetime -= Time.deltaTime;
		}
		if (GunFlashLifetime <= 0f)
		{
			_SetGunFlashActive(false);
		}
		if (CurHealth <= 0)
		{
			if (GlobalGameController.Score > PlayerPrefs.GetInt(Defs.BestScoreSett, 0))
			{
				PlayerPrefs.SetInt(Defs.BestScoreSett, GlobalGameController.Score);
				PlayerPrefs.Save();
			}
			Application.LoadLevel("GameOver");
		}
	}

	public static Rect SuccessMessageRect()
	{
		return new Rect((float)(Screen.width / 2) - (float)Screen.height * 0.5f, (float)Screen.height * 0.5f - (float)Screen.height * 0.0525f, Screen.height, (float)Screen.height * 0.105f);
	}

	private void OnGUI()
	{
		GUI.skin = MySkin;
		GUI.DrawTexture(new Rect((float)(Screen.width / 2) - (float)Screen.height * 0.023f, (float)(Screen.height / 2) - (float)Screen.height * 0.023f, (float)Screen.height * 0.046f, (float)Screen.height * 0.046f), AimTexture);
		Rect rect = new Rect((float)Screen.width - (float)Screen.width * 0.208f, 0f, (float)Screen.width * 0.208f, (float)Screen.height * 0.078f);
		float num = (float)(buyStyle.normal.background.width * Screen.width) / 1024f;
		float num2 = num * ((float)buyStyle.normal.background.height / (float)buyStyle.normal.background.width);
		Rect position = new Rect((float)Screen.width - num, 0f, num, num2);
		Rect position2 = new Rect(0f, 0f, 73f * (float)Screen.width / 1024f, 73f * (float)Screen.width / 1024f);
		AmmoBox.fontSize = Mathf.RoundToInt(18f * (float)Screen.width / 1024f);
		Rect position3 = new Rect((float)Screen.width - 264f * (float)Screen.width / 1024f, 94f * (float)Screen.width / 1024f, 264f * (float)Screen.width / 1024f, 95f * (float)Screen.width / 1024f);
		Rect position4 = new Rect((float)Screen.width - 172f * (float)Screen.width / 1024f, 186f * (float)Screen.width / 1024f, (float)(40 * Screen.width) / 1024f, (float)(28 * Screen.width) / 1024f);
		Rect position5 = new Rect((float)Screen.width - 135f * (float)Screen.width / 1024f, 186f * (float)Screen.width / 1024f, 130f * (float)Screen.width / 1024f, (float)(28 * Screen.width) / 1024f);
		if (_weaponManager.CurrentWeaponIndex >= 0 && _weaponManager.CurrentWeaponIndex < _weaponManager.playerWeapons.Count && !_weaponManager.currentWeaponSounds.isMelee)
		{
			GUI.DrawTexture(position4, ammoTexture);
			GUI.Box(position5, ((Weapon)_weaponManager.playerWeapons[_weaponManager.CurrentWeaponIndex]).currentAmmoInClip + "/" + ((Weapon)_weaponManager.playerWeapons[_weaponManager.CurrentWeaponIndex]).currentAmmoInBackpack, AmmoBox);
		}
		if (PlayerPrefs.GetInt(Defs.NumberOfElixirsSett, 1) > 0)
		{
			float num3 = 1.5f;
			float num4 = position3.height * (float)(elixir.width / elixir.height) / num3;
			float num5 = position3.height / num3;
			Rect rect2 = new Rect((float)Screen.width - position5.x - position5.width, position5.y, position5.width, position5.height);
			Rect position6 = new Rect(rect2.x + rect2.width / 2f - num4 / 2f, position3.y, num4, num5);
			GUI.DrawTexture(position6, elixir);
		}
		ScoreBox.fontSize = Mathf.RoundToInt((float)Screen.height * 0.035f);
		float num6 = (float)(enemiesTxture.width * Screen.width) / 1024f;
		float num7 = num6 * ((float)enemiesTxture.height / (float)enemiesTxture.width);
		float num8 = 13f;
		EnemiesBox.fontSize = Mathf.RoundToInt((float)Screen.height * 0.035f);
		GUI.DrawTexture(new Rect(position.x - num6, 0f, num6, num7), enemiesTxture);
		GUI.Box(new Rect(position.x - num6, num8 * (float)Screen.width / 1024f, num6, num7), string.Empty + (GlobalGameController.EnemiesToKill - _zombieCreator.NumOfDeadZombies), EnemiesBox);
		GUI.DrawTexture(new Rect(position.x - num6 - num6, 0f, num6, num7), scoreTexture);
		GUI.Box(new Rect(position.x - num6 - num6, num8 * (float)Screen.width / 1024f, num6, num7), string.Empty + GlobalGameController.Score, ScoreBox);
		bool flag = true;
		float left = (float)Screen.width * 0.271f;
		float width = (float)Screen.width * 0.521f;
		float num9 = position2.x + position2.width * 1.5f;
		for (int i = 0; i < CurHealth; i++)
		{
			GUI.DrawTexture(new Rect(num9 + (float)i * (49.5f * (float)Screen.width / 1024f), 12f * (float)Screen.width / 1024f, 31f * (float)Screen.width / 1024f, 30f * (float)Screen.width / 1024f), HeartTexture);
		}
		GUI.DrawTexture(position3, _weaponManager.currentWeaponSounds.preview);
		if ((bool)_weaponManager && _weaponManager.playerWeapons != null && _weaponManager.playerWeapons.Count > 1)
		{
			GUI.Box(new Rect((float)Screen.width - 186f * (float)Screen.width / 1024f, 94f * (float)Screen.width / 1024f, 186f * (float)Screen.width / 1024f, 23f * (float)Screen.width / 1024f), "< SWIPE >", ScoreBox);
		}
		bool flag2 = false;
		if (Application.platform == RuntimePlatform.Android)
		{
			if (Input.GetKey(KeyCode.Escape))
			{
				_backWasPressed = true;
				Debug.Log("407 androidBackPressed " + flag2);
			}
			else
			{
				if (_backWasPressed)
				{
					flag2 = true;
				}
				_backWasPressed = false;
			}
		}
		if ((GUI.Button(position2, string.Empty, pauseStyle) || (flag2 && !_pauser.paused)) && !_pauser.paused)
		{
			flag2 = false;
			if (CurHealth > 0)
			{
				SetPause();
			}
		}
		if (GlobalGameController.EnemiesToKill - _zombieCreator.NumOfDeadZombies == 0)
		{
			enemiesLeftStyle.fontSize = Mathf.RoundToInt((float)Screen.height * 0.035f);
			GUI.Box(new Rect(left, height + (float)(enemiesLeftStyle.fontSize * 2), width, height), "Enter the Portal...", enemiesLeftStyle);
		}
		GUI.DrawTexture(position, buyStyle.active.background);
		if (StoreKitEventListener.billingSupported)
		{
			if (GUI.Button(position, string.Empty, buyStyle) && !_pauser.paused && CurHealth > 0)
			{
				SetInApp();
				SetPause();
			}
		}
		else
		{
			GUI.DrawTexture(position, buyTexture);
		}
		if (isInappWinOpen)
		{
			Rect position7 = new Rect(((float)Screen.width - 2048f * (float)Screen.height / 1154f) / 2f, 0f, 2048f * (float)Screen.height / 1154f, Screen.height);
			GUI.DrawTexture(position7, inAppFon, ScaleMode.StretchToFill);
			int num10 = StoreKitEventListener.skus.Length;
			for (int j = 0; j < num10; j++)
			{
				GUIStyle style = puliInApp;
				if (StoreKitEventListener.skus[j].Equals(StoreKitEventListener.bigAmmoPackID))
				{
					style = puliInApp;
				}
				if (StoreKitEventListener.skus[j].Equals(StoreKitEventListener.fullHealthID))
				{
					style = healthInApp;
				}
				bool flag3 = StoreKitEventListener.skus[j].Equals(StoreKitEventListener.elixirID);
				if (flag3)
				{
					style = elixirInapp;
				}
				bool flag4 = StoreKitEventListener.skus[j].Equals(StoreKitEventListener.crystalswordID);
				if (flag4)
				{
					style = crystalSwordInapp;
				}
				float num11 = 320f / (float)healthInApp.normal.background.height;
				float num12 = (float)(healthInApp.normal.background.width * Screen.height) / 768f * num11;
				float num13 = num12 * ((float)healthInApp.normal.background.height / (float)healthInApp.normal.background.width);
				Rect position8 = new Rect((float)(Screen.width / (num10 + 1) * (j + 1)) - num12 * 0.5f, (float)Screen.height * 0.62f - 367f * (float)Screen.height / 768f * 0.5f - num13 * 0.15f, num12, num13);
				bool flag5 = StoreKitEventListener.skus[j].Equals(StoreKitEventListener.minerWeaponID);
				if (flag5)
				{
					style = pulemetInApp;
				}
				if (flag5 && PlayerPrefs.GetInt(Defs.MinerWeaponSett, 0) > 0)
				{
					GUI.DrawTexture(position8, minerWeaponSoldTexture, ScaleMode.StretchToFill);
				}
				else if (flag4 && PlayerPrefs.GetInt(Defs.SwordSett, 0) > 0)
				{
					GUI.DrawTexture(position8, swordSoldTexture, ScaleMode.StretchToFill);
				}
				else if (flag3 && PlayerPrefs.GetInt(Defs.NumberOfElixirsSett, 1) == 1)
				{
					GUI.DrawTexture(position8, hasElixirTexture, ScaleMode.StretchToFill);
				}
				else if (GUI.Button(position8, string.Empty, style))
				{
					GoogleIAB.purchaseProduct(StoreKitEventListener.skus[j]);
				}
			}
			float num14 = (float)Screen.height * 0.22f * 1.5f;
			if (GUI.Button(new Rect((float)Screen.width * 0.5f - num14 / 2f, (float)Screen.height * 0.86f, num14, (float)Screen.height * 0.078f * 1.5f), string.Empty, resumeStyle))
			{
				SetInApp();
				SetPause();
			}
		}
		if (Time.realtimeSinceStartup - _timeWhenPurchShown >= 2f)
		{
			productPurchased = false;
		}
		if (productPurchased)
		{
			labelStyle.fontSize = FontSizeForMessages;
			GUI.Label(SuccessMessageRect(), "Purchase was successful", labelStyle);
		}
		if ((bool)_pauser && _pauser.paused && !isInappWinOpen)
		{
			Rect position9 = new Rect(((float)Screen.width - 2048f * (float)Screen.height / 1154f) / 2f, 0f, 2048f * (float)Screen.height / 1154f, Screen.height);
			GUI.DrawTexture(position9, pauseFon, ScaleMode.StretchToFill);
			float num15 = 15f;
			Vector2 vector = new Vector2(176f, 150f - num15);
			Rect position10 = new Rect((float)Screen.width * 0.57f, (float)Screen.height * 0.05f, (float)Screen.height * 0.4781f, (float)Screen.height * 0.1343f);
			GUI.DrawTexture(position10, pauseTitle);
			float num16 = (float)resumePauseStyle.normal.background.width * Defs.Coef;
			float num17 = num16 * ((float)resumePauseStyle.normal.background.height / (float)resumePauseStyle.normal.background.width);
			float num18 = num17 * 0.33f;
			if (GUI.Button(new Rect(position10.x + position10.width / 2f - num16 / 2f, position10.y + position10.height + num18 * 1.5f, num16, num17), string.Empty, resumePauseStyle) || flag2)
			{
				SetPause();
			}
			if (GUI.Button(new Rect(position10.x + position10.width / 2f - num16 / 2f, position10.y + position10.height + num17 + num18 * 2.5f, num16, num17), string.Empty, menuStyle))
			{
				Time.timeScale = 1f;
				Application.LoadLevel("Restart");
			}
			float num19 = 15f;
			bool @bool = PlayerPrefsX.GetBool(PlayerPrefsX.SndSetting, true);
			@bool = GUI.Toggle(new Rect((float)Screen.width * 0.05f, (float)Screen.height * 0.923f - (float)Screen.height * 0.0525f, (float)Screen.height * 0.105f, (float)Screen.height * 0.105f), @bool, string.Empty, soundStyle);
			AudioListener.volume = (@bool ? 1 : 0);
			PlayerPrefsX.SetBool(PlayerPrefsX.SndSetting, @bool);
			PlayerPrefs.Save();
		}
	}

	private void SetPause()
	{
		_pauser.paused = !_pauser.paused;
		if (_pauser.paused)
		{
			Time.timeScale = 0f;
		}
		else
		{
			Time.timeScale = 1f;
		}
	}

	private void SetInApp()
	{
		isInappWinOpen = !isInappWinOpen;
		if (isInappWinOpen)
		{
			Time.timeScale = 0f;
		}
		else
		{
			Time.timeScale = 1f;
		}
	}

	private void ProvideAmmo()
	{
		_listener.ProvideContent();
		_weaponManager.SetMaxAmmoFrAllWeapons();
		_rightJoystick.SendMessage("HasAmmo");
	}

	private void ProvideHealth()
	{
		CurHealth = MaxHealth;
	}

	public static void SaveMinerWeaponInPrefabs()
	{
		PlayerPrefs.SetInt(Defs.MinerWeaponSett, 1);
		PlayerPrefs.Save();
	}

	public static void SaveSwordInPrefs()
	{
		PlayerPrefs.SetInt(Defs.SwordSett, 1);
		PlayerPrefs.Save();
	}

	private void provideminerweapon()
	{
		GameObject pickPrefab = _weaponManager.GetPickPrefab();
		SaveMinerWeaponInPrefabs();
		AddWeapon(pickPrefab);
	}

	private void providesword()
	{
		GameObject swordPrefab = _weaponManager.GetSwordPrefab();
		SaveSwordInPrefs();
		AddWeapon(swordPrefab);
	}

	private void purchaseSuccessful(GooglePurchase purchase)
	{
		Debug.Log("purchaseSucceededEvent: " + purchase);
		if (purchase.productId.Equals(StoreKitEventListener.bigAmmoPackID) || purchase.productId.Equals(StoreKitEventListener.fullHealthID))
		{
			GoogleIAB.consumeProduct(purchase.productId);
			if (purchase.productId.Equals(StoreKitEventListener.bigAmmoPackID))
			{
				ProvideAmmo();
			}
			if (purchase.productId.Equals(StoreKitEventListener.fullHealthID))
			{
				ProvideHealth();
			}
		}
		if (purchase.productId.Equals(StoreKitEventListener.minerWeaponID))
		{
			provideminerweapon();
		}
		if (purchase.productId.Equals(StoreKitEventListener.crystalswordID))
		{
			providesword();
		}
		productPurchased = true;
		_timeWhenPurchShown = Time.realtimeSinceStartup;
	}

	private void consumptionSucceeded(GooglePurchase purchase)
	{
	}

	private IEnumerator _ResetProductPurchased()
	{
		yield return new WaitForSeconds(1f);
		productPurchased = false;
	}
}
