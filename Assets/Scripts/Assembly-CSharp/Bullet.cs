using UnityEngine;

public class Bullet : MonoBehaviour
{
	private float LifeTime = 1f;

	private float RespawnTime;

	public float bulletSpeed = 200f;

	private void Start()
	{
		Invoke("RemoveSelf", LifeTime);
	}

	private void RemoveSelf()
	{
		Object.Destroy(base.gameObject);
	}

	private void Update()
	{
		base.transform.position += base.transform.forward * bulletSpeed * Time.deltaTime;
	}
}
