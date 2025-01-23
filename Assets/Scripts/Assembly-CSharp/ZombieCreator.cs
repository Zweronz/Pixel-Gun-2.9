using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieCreator : MonoBehaviour
{
	private GameObject[] _teleports;

	public List<GameObject> zombiePrefabs = new List<GameObject>();

	private int _numOfLiveZombies;

	private int _numOfDeadZombies;

	private int _numOfDeadZombsSinceLastFast;

	public float curInterval = 10f;

	private GameObject[] _enemyCreationZones;

	private List<string[]> _enemies = new List<string[]>();

	public int NumOfLiveZombies
	{
		get
		{
			return _numOfLiveZombies;
		}
		set
		{
			_numOfLiveZombies = value;
		}
	}

	public int NumOfDeadZombies
	{
		get
		{
			return _numOfDeadZombies;
		}
		set
		{
			int num = value - _numOfDeadZombies;
			_numOfDeadZombies = value;
			NumOfLiveZombies -= num;
			_numOfDeadZombsSinceLastFast += num;
			if (_numOfDeadZombsSinceLastFast == 12)
			{
				if (curInterval > 5f)
				{
					curInterval -= 5f;
				}
				_numOfDeadZombsSinceLastFast = 0;
			}
			if (_numOfDeadZombies >= GlobalGameController.EnemiesToKill)
			{
				GameObject[] teleports = _teleports;
				foreach (GameObject gameObject in teleports)
				{
					gameObject.SetActive(true);
				}
			}
		}
	}

	private void Awake()
	{
		_enemies.Add(new string[4] { "1", "2", "1", "11" });
		_enemies.Add(new string[4] { "1", "2", "3", "10" });
		_enemies.Add(new string[5] { "1", "2", "3", "9", "10" });
		_enemies.Add(new string[5] { "1", "2", "4", "11", "9" });
		_enemies.Add(new string[6] { "1", "2", "4", "9", "11", "10" });
		_enemies.Add(new string[4] { "1", "2", "3", "9" });
		_enemies.Add(new string[3] { "6", "7", "7" });
		_enemies.Add(new string[5] { "1", "2", "8", "10", "11" });
		string[] array = _enemies[GlobalGameController.currentLevel - 1];
		foreach (string text in array)
		{
			GameObject item = Resources.Load("Enemies/Enemy" + text + "_go") as GameObject;
			zombiePrefabs.Add(item);
		}
	}

	private void Start()
	{
		_enemyCreationZones = GameObject.FindGameObjectsWithTag("EnemyCreationZone");
		_teleports = GameObject.FindGameObjectsWithTag("Portal");
		GameObject[] teleports = _teleports;
		foreach (GameObject gameObject in teleports)
		{
			gameObject.SetActive(false);
		}
		curInterval = Mathf.Max(1f, curInterval - (float)GlobalGameController.AllLevelsCompleted);
	}

	public void BeganCreateEnemies()
	{
		StartCoroutine(AddZombies());
	}

	private void Update()
	{
	}

	private IEnumerator AddZombies()
	{
		float halfLLength = 17f;
		float radius = 2.5f;
		do
		{
			int numOfZombsToAdd = GlobalGameController.ZombiesInWave;
			numOfZombsToAdd = Mathf.Min(numOfZombsToAdd, GlobalGameController.SimultaneousEnemiesOnLevelConstraint - NumOfLiveZombies);
			numOfZombsToAdd = Mathf.Min(numOfZombsToAdd, GlobalGameController.EnemiesToKill - (NumOfDeadZombies + NumOfLiveZombies));
			for (int i = 0; i < numOfZombsToAdd; i++)
			{
				int typeOfZomb = Random.Range(0, _enemies[GlobalGameController.currentLevel - 1].Length);
				GameObject spawnZone = _enemyCreationZones[Random.Range(0, _enemyCreationZones.Length)];
				BoxCollider spawnZoneCollider = spawnZone.GetComponent<BoxCollider>();
				Vector2 sz = new Vector2(spawnZoneCollider.size.x * spawnZone.transform.localScale.x, spawnZoneCollider.size.z * spawnZone.transform.localScale.z);
				Rect zoneRect = new Rect(spawnZone.transform.position.x - sz.x / 2f, spawnZone.transform.position.z - sz.y / 2f, sz.x, sz.y);
				Object.Instantiate(position: new Vector3(zoneRect.x + Random.Range(0f, zoneRect.width), (GlobalGameController.currentLevel != 8) ? 0f : spawnZone.transform.position.y, zoneRect.y + Random.Range(0f, zoneRect.height)), original: zombiePrefabs[typeOfZomb], rotation: Quaternion.identity);
			}
			yield return new WaitForSeconds(curInterval);
		}
		while (NumOfDeadZombies + NumOfLiveZombies < GlobalGameController.EnemiesToKill);
	}
}
