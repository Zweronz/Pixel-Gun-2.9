using System;
using System.Collections.Generic;
using UnityEngine;

public class Initializer : MonoBehaviour
{
	private GameObject _playerPrefab;

	private bool _isMultiplayer;

	private List<Vector3> _initPlayerPositions = new List<Vector3>();

	private List<float> _rots = new List<float>();

	public static event Action PlayerAddedEvent;

	private void Awake()
	{
		GameObject original = Resources.Load("BackgroundMusic/BackgroundMusic_Level" + GlobalGameController.currentLevel) as GameObject;
		UnityEngine.Object.Instantiate(original);
		GameObject gameObject = GameObject.FindGameObjectWithTag("Configurator");
		CoinConfigurator component = gameObject.GetComponent<CoinConfigurator>();
		if (component.CoinIsPresent)
		{
			GameObject original2 = Resources.Load("coin") as GameObject;
			UnityEngine.Object.Instantiate(original2, component.pos, Quaternion.Euler(270f, 0f, 0f));
		}
	}

	private void Start()
	{
		if (!_isMultiplayer)
		{
			_initPlayerPositions.Add(new Vector3(12f, 1f, 9f));
			_initPlayerPositions.Add(new Vector3(17f, 1f, -15f));
			_initPlayerPositions.Add(new Vector3(-30f, 1f, -35f));
			_initPlayerPositions.Add(new Vector3(0f, 1f, 0f));
			_initPlayerPositions.Add(new Vector3(-33f, 1.2f, -13f));
			_initPlayerPositions.Add(new Vector3(-2.67f, 1f, 2.67f));
			_initPlayerPositions.Add(new Vector3(0f, 1f, 0f));
			_initPlayerPositions.Add(new Vector3(19f, 1f, -0.8f));
			_rots.Add(0f);
			_rots.Add(0f);
			_rots.Add(270f);
			_rots.Add(0f);
			_rots.Add(180f);
			_rots.Add(0f);
			_rots.Add(0f);
			_rots.Add(270f);
			AddPlayer();
		}
	}

	private void AddPlayer()
	{
		_playerPrefab = Resources.Load("Player") as GameObject;
		UnityEngine.Object.Instantiate(_playerPrefab, _initPlayerPositions[GlobalGameController.currentLevel - 1], Quaternion.Euler(0f, _rots[GlobalGameController.currentLevel - 1], 0f));
		Invoke("SetupObjectThatNeedsPlayer", 0.01f);
	}

	private void SetupObjectThatNeedsPlayer()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("CoinBonus");
		if ((bool)gameObject)
		{
			CoinBonus component = gameObject.GetComponent<CoinBonus>();
			if ((bool)component)
			{
				component.SetPlayer();
			}
		}
		GetComponent<ZombieCreator>().BeganCreateEnemies();
		GetComponent<BonusCreator>().BeginCreateBonuses();
		Initializer.PlayerAddedEvent();
	}
}
