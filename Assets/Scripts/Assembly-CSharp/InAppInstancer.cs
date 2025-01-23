using UnityEngine;

public class InAppInstancer : MonoBehaviour
{
	public GameObject inAppGameObjectPrefab;

	private void Start()
	{
		if (!GameObject.FindGameObjectWithTag("InAppGameObject"))
		{
			Object.Instantiate(inAppGameObjectPrefab, Vector3.zero, Quaternion.identity);
		}
	}
}
