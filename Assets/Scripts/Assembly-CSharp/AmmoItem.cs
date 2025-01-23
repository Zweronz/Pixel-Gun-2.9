using UnityEngine;

public class AmmoItem : MonoBehaviour
{
	private Player_move_c test;

	private GameObject player;

	public AudioClip AmmoItemUp;

	private void Start()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("PlayerGun");
		if ((bool)gameObject)
		{
			test = gameObject.GetComponent<Player_move_c>();
		}
		player = GameObject.FindGameObjectWithTag("Player");
	}

	private void Update()
	{
		if (Vector3.Distance(base.transform.position, player.transform.position) < 2f && test.NeedAmmo())
		{
			if (!GameObject.FindGameObjectWithTag("WeaponManager").GetComponent<WeaponManager>().AddAmmo())
			{
				GlobalGameController.Score += Defs.ScoreForSurplusAmmo;
			}
			test.gameObject.GetComponent<AudioSource>().PlayOneShot(AmmoItemUp);
			Object.Destroy(base.gameObject);
		}
	}
}
