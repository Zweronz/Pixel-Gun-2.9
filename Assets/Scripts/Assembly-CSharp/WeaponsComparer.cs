using System.Collections;

public class WeaponsComparer : IComparer
{
	int IComparer.Compare(object x, object y)
	{
		return ((Weapon)x).weaponPrefab.name.CompareTo(((Weapon)y).weaponPrefab.name);
	}
}
