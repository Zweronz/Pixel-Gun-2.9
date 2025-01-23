using UnityEngine;

public class CoinBonus : MonoBehaviour
{
	private GameObject player;

	public AudioClip CoinItemUpAudioClip;

	private Player_move_c test;

	public void SetPlayer()
	{
		test = GameObject.FindGameObjectWithTag("PlayerGun").GetComponent<Player_move_c>();
		player = GameObject.FindGameObjectWithTag("Player");
	}

	private void Update()
	{
		if (!(test == null) && !(player == null) && Vector3.Distance(base.transform.position, player.transform.position) < 2f)
		{
			test.gameObject.GetComponent<AudioSource>().PlayOneShot(CoinItemUpAudioClip);
			GlobalGameController.Score += 1000;
			Object.Destroy(base.gameObject);
		}
	}
}
