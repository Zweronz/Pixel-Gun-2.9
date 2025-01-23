using System;
using System.Collections;
using UnityEngine;

public class BotHealth : MonoBehaviour
{
	private static SkinsManagerPixlGun _skinsManager;

	public string myName = "Bot";

	private bool IsLife = true;

	public Texture hitTexture;

	private BotAI ai;

	private Player_move_c healthDown;

	private bool _flashing;

	private GameObject _modelChild;

	private Sounds _soundClips;

	private Texture _skin;

	private void Start()
	{
		IEnumerator enumerator = base.transform.GetEnumerator();
		try
		{
			if (enumerator.MoveNext())
			{
				Transform transform = (Transform)enumerator.Current;
				_modelChild = transform.gameObject;
			}
		}
		finally
		{
			IDisposable disposable = enumerator as IDisposable;
			if (disposable != null)
			{
				disposable.Dispose();
			}
		}
		_soundClips = _modelChild.GetComponent<Sounds>();
		ai = GetComponent<BotAI>();
		healthDown = GameObject.FindGameObjectWithTag("PlayerGun").GetComponent<Player_move_c>();
		_skin = SetSkinForObj(_modelChild);
	}

	public static Texture SetSkinForObj(GameObject go)
	{
		if (!_skinsManager)
		{
			_skinsManager = GameObject.FindGameObjectWithTag("SkinsManager").GetComponent<SkinsManagerPixlGun>();
		}
		Texture texture = null;
		string text = SkinNameForObj(go.name);
		if (!(texture = _skinsManager.skins[text] as Texture))
		{
			Debug.Log("No skin: " + text);
		}
		SetTextureRecursivelyFrom(go, texture);
		return texture;
	}

	public static string SkinNameForObj(string objName)
	{
		return objName + "_Level" + GlobalGameController.currentLevel;
	}

	public static void SetTextureRecursivelyFrom(GameObject obj, Texture txt)
	{
		foreach (Transform item in obj.transform)
		{
			if ((bool)item.gameObject.GetComponent<Renderer>() && (bool)item.gameObject.GetComponent<Renderer>().material)
			{
				if (item.gameObject.GetComponent<Renderer>().materials.Length > 1 && item.gameObject.name.Equals("raven_head"))
				{
					Material[] materials = item.gameObject.GetComponent<Renderer>().materials;
					foreach (Material material in materials)
					{
						if (material.name.Equals("raven_eye (Instance)"))
						{
							if (GlobalGameController.currentLevel == 6)
							{
								material.color = new Color(0.32156864f, 0f, 44f / 85f);
							}
						}
						else
						{
							material.mainTexture = txt;
						}
					}
				}
				else
				{
					item.gameObject.GetComponent<Renderer>().material.mainTexture = txt;
				}
			}
			SetTextureRecursivelyFrom(item.gameObject, txt);
		}
	}

	private IEnumerator Flash()
	{
		_flashing = true;
		SetTextureRecursivelyFrom(_modelChild, hitTexture);
		yield return new WaitForSeconds(0.125f);
		SetTextureRecursivelyFrom(_modelChild, _skin);
		_flashing = false;
	}

	public void adjustHealth(float _health, Transform target)
	{
		if (_health < 0f && !_flashing)
		{
			StartCoroutine(Flash());
		}
		_soundClips.health += _health;
		if (_soundClips.health < 0f)
		{
			_soundClips.health = 0f;
		}
		if (_soundClips.health == 0f)
		{
			IsLife = false;
		}
		else
		{
			GlobalGameController.Score += 5;
		}
		base.GetComponent<AudioSource>().PlayOneShot(_soundClips.hurt);
		ai.SetTarget(target, true);
	}

	public bool getIsLife()
	{
		return IsLife;
	}
}
