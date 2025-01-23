using UnityEngine;

public class Message : MonoBehaviour
{
	public GUIStyle labelStyle;

	public Rect rect = Player_move_c.SuccessMessageRect();

	public string message = "Purchases have been restored";

	public int depth = -2;

	private void Start()
	{
		Object.DontDestroyOnLoad(base.gameObject);
		Invoke("Remove", 2f);
	}

	private void Remove()
	{
		Object.Destroy(base.gameObject);
	}

	private void OnGUI()
	{
		rect = Player_move_c.SuccessMessageRect();
		int num = GUI.depth;
		GUI.depth = depth;
		labelStyle.fontSize = Player_move_c.FontSizeForMessages;
		GUI.Label(rect, message, labelStyle);
		GUI.depth = num;
	}
}
