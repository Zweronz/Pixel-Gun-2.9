using System;
using UnityEngine;

public class GotToNextLevel : MonoBehaviour
{
	private Action OnPlayerAddedAct;

	private GameObject _player;

	private Player_move_c _playerMoveC;

	private void Awake()
	{
		OnPlayerAddedAct = delegate
		{
			_player = GameObject.FindGameObjectWithTag("Player");
			_playerMoveC = GameObject.FindGameObjectWithTag("PlayerGun").GetComponent<Player_move_c>();
		};
		Initializer.PlayerAddedEvent += OnPlayerAddedAct;
	}

	private void OnDestroy()
	{
		Initializer.PlayerAddedEvent -= OnPlayerAddedAct;
	}

	private void Update()
	{
		if (!(_player == null) && !(_playerMoveC == null) && Vector3.Distance(base.transform.position, _player.transform.position) < 1.5f)
		{
			PlayerPrefs.SetInt(Defs.CurrentHealthSett, _playerMoveC.CurHealth);
			AutoFade.LoadLevel("Loading", 0.5f, 0.5f, Color.white);
		}
	}
}
