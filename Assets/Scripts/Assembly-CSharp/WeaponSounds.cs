using UnityEngine;

public class WeaponSounds : MonoBehaviour
{
	public AudioClip shoot;

	public AudioClip reload;

	public AudioClip empty;

	public bool isSerialShooting;

	public GameObject bonusPrefab;

	public int InitialAmmo = 24;

	public int ammoInClip = 12;

	public int maxAmmo = 84;

	public bool isMelee;

	public float range = 3f;

	public int damage = 50;

	public float speedModifier = 1f;

	public GameObject animationObject;

	public int Probability = 1;

	public Vector2 damageRange = new Vector2(-15f, 15f);

	public Vector3 gunPosition = new Vector3(0.35f, -0.25f, 0.6f);

	public Texture preview;

	public int inAppExtensionModifier = 10;

	public int MaxAmmoWithRespectToInApp
	{
		get
		{
			return maxAmmo * ((PlayerPrefs.GetInt("BigAmmoPackBought", 0) <= 0) ? 1 : inAppExtensionModifier);
		}
	}
}
