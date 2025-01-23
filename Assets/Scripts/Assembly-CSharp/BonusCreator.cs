using System.Collections;
using UnityEngine;

public class BonusCreator : MonoBehaviour
{
	public GameObject[] bonusPrefabs;

	public float creationInterval = 10f;

	public float weaponCreationInterval = 30f;

	private Object[] weaponPrefabs;

	private int _lastWeapon = -1;

	private ArrayList bonuses = new ArrayList();

	private ArrayList _weapons = new ArrayList();

	private GameObject[] _bonusCreationZones;

	private ZombieCreator _zombieCreator;

	private ArrayList _weaponsProbDistr = new ArrayList();

	private float _DistrSum()
	{
		float num = 0f;
		foreach (int item in _weaponsProbDistr)
		{
			num += (float)item;
		}
		return num;
	}

	private void Awake()
	{
		weaponPrefabs = GameObject.FindGameObjectWithTag("WeaponManager").GetComponent<WeaponManager>().weaponsInGame;
		Object[] array = weaponPrefabs;
		for (int i = 0; i < array.Length; i++)
		{
			GameObject gameObject = (GameObject)array[i];
			WeaponSounds component = gameObject.GetComponent<WeaponSounds>();
			_weaponsProbDistr.Add(component.Probability);
		}
	}

	private void Start()
	{
		_bonusCreationZones = GameObject.FindGameObjectsWithTag("BonusCreationZone");
		_zombieCreator = base.gameObject.GetComponent<ZombieCreator>();
	}

	public void BeginCreateBonuses()
	{
		StartCoroutine(AddBonus());
		StartCoroutine(AddWeapon());
	}

	public GameObject GetPrefabWithTag(string tagName)
	{
		Object[] array = weaponPrefabs;
		for (int i = 0; i < array.Length; i++)
		{
			GameObject gameObject = (GameObject)array[i];
			if (gameObject.CompareTag(tagName))
			{
				return gameObject;
			}
		}
		return null;
	}

	private IEnumerator AddBonus()
	{
		while (true)
		{
			yield return new WaitForSeconds(creationInterval);
			int enemiesLeft = GlobalGameController.EnemiesToKill - _zombieCreator.NumOfDeadZombies;
			if (enemiesLeft <= 0)
			{
				break;
			}
			GameObject spawnZone = _bonusCreationZones[Random.Range(0, _bonusCreationZones.Length)];
			BoxCollider spawnZoneCollider = spawnZone.GetComponent<BoxCollider>();
			Vector2 sz = new Vector2(spawnZoneCollider.size.x * spawnZone.transform.localScale.x, spawnZoneCollider.size.z * spawnZone.transform.localScale.z);
			Rect zoneRect = new Rect(spawnZone.transform.position.x - sz.x / 2f, spawnZone.transform.position.z - sz.y / 2f, sz.x, sz.y);
			Vector3 pos = new Vector3(zoneRect.x + Random.Range(0f, zoneRect.width), 0.24f, zoneRect.y + Random.Range(0f, zoneRect.height));
			int type = Random.Range(0, 11);
			Object newBonus = (GameObject)Object.Instantiate(bonusPrefabs[(type >= 9) ? 1u : 0u], pos, Quaternion.identity);
			bonuses.Add(newBonus);
			if (bonuses.Count > 5)
			{
				Object.Destroy((GameObject)bonuses[0]);
				bonuses.RemoveAt(0);
			}
		}
	}

	private IEnumerator AddWeapon()
	{
		while (true)
		{
			yield return new WaitForSeconds(weaponCreationInterval);
			int enemiesLeft = GlobalGameController.EnemiesToKill - _zombieCreator.NumOfDeadZombies;
			if (enemiesLeft <= 0)
			{
				break;
			}
			GameObject spawnZone = _bonusCreationZones[Random.Range(0, _bonusCreationZones.Length)];
			BoxCollider spawnZoneCollider = spawnZone.GetComponent<BoxCollider>();
			Vector2 sz = new Vector2(spawnZoneCollider.size.x * spawnZone.transform.localScale.x, spawnZoneCollider.size.z * spawnZone.transform.localScale.z);
			Rect zoneRect = new Rect(spawnZone.transform.position.x - sz.x / 2f, spawnZone.transform.position.z - sz.y / 2f, sz.x, sz.y);
			Vector3 pos = new Vector3(zoneRect.x + Random.Range(0f, zoneRect.width), 0.24f, zoneRect.y + Random.Range(0f, zoneRect.height));
			float sum = _DistrSum();
			int weaponNumber;
			do
			{
				weaponNumber = 0;
				float val = Random.Range(0f, sum);
				float curSum = 0f;
				for (int i = 0; i < _weaponsProbDistr.Count; i++)
				{
					if (val < curSum + (float)(int)_weaponsProbDistr[i])
					{
						weaponNumber = i;
						break;
					}
					curSum += (float)(int)_weaponsProbDistr[i];
				}
			}
			while (weaponNumber == _lastWeapon || weaponPrefabs[weaponNumber].name.Equals(WeaponManager.PickWeaponName) || weaponPrefabs[weaponNumber].name.Equals(WeaponManager.SwordWeaponName));
			GameObject wp = (GameObject)weaponPrefabs[weaponNumber];
			wp.transform.rotation = Quaternion.identity;
			WeaponSounds ws = wp.GetComponent<WeaponSounds>();
			GameObject bonus = ws.bonusPrefab;
			GameObject newBonus = (GameObject)Object.Instantiate(bonus, pos, (!bonus.CompareTag("PistolRotate")) ? Quaternion.identity : Quaternion.Euler(270f, 90f, 0f));
			newBonus.AddComponent<WeaponBonus>();
			WeaponBonus wb = newBonus.GetComponent<WeaponBonus>();
			wb.weaponPrefab = wp;
			float weaponsScale = 2f;
			newBonus.transform.localScale = ((wp.CompareTag("M249MachinegunWeapon") || wp.CompareTag("AK47")) ? new Vector3(1f, 1f, 1f) : ((!wp.CompareTag("Colt45Weapon")) ? new Vector3(weaponsScale, weaponsScale, weaponsScale) : new Vector3(1f, 1f, 1f)));
			_weapons.Add(newBonus);
			if (_weapons.Count > 5)
			{
				Object.Destroy((GameObject)_weapons[0]);
				_weapons.RemoveAt(0);
			}
		}
	}
}
