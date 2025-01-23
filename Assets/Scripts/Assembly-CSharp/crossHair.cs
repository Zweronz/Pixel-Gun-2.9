using UnityEngine;

public class crossHair : MonoBehaviour
{
	public Texture2D crossHairTexture;

	private Rect crossHairPosition;

	private Pauser pauser;

	private Player_move_c playerMoveC;

	private void Start()
	{
		crossHairPosition = new Rect((Screen.width - crossHairTexture.width * Screen.height / 640) / 2, (Screen.height - crossHairTexture.height * Screen.height / 640) / 2, crossHairTexture.width * Screen.height / 640, crossHairTexture.height * Screen.height / 640);
		pauser = GameObject.FindGameObjectWithTag("GameController").GetComponent<Pauser>();
		playerMoveC = GameObject.FindGameObjectWithTag("PlayerGun").GetComponent<Player_move_c>();
	}

	private void OnGUI()
	{
		if (!pauser.paused)
		{
			GUI.DrawTexture(crossHairPosition, crossHairTexture);
		}
	}
}
