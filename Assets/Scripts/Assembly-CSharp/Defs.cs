using UnityEngine;

public class Defs : MonoBehaviour
{
	public static float HalfLength
	{
		get
		{
			return 17f;
		}
	}

	public static string BestScoreSett
	{
		get
		{
			return "BestScoreSett";
		}
	}

	public static string InAppBoughtSett
	{
		get
		{
			return "BigAmmoPackBought";
		}
	}

	public static string CurrentWeaponSett
	{
		get
		{
			return "CurrentWeapon";
		}
	}

	public static string MinerWeaponSett
	{
		get
		{
			return "MinerWeaponSett";
		}
	}

	public static string SwordSett
	{
		get
		{
			return "SwordSett";
		}
	}

	public static int ScoreForSurplusAmmo
	{
		get
		{
			return 50;
		}
	}

	public static string CurrentHealthSett
	{
		get
		{
			return "CurrentHealthSett";
		}
	}

	public static string NumberOfElixirsSett
	{
		get
		{
			return "NumberOfElixirsSett";
		}
	}

	public static float Coef
	{
		get
		{
			return (float)Screen.height / 768f;
		}
	}
}
