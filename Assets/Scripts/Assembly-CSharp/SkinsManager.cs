using System;
using System.IO;
using UnityEngine;

public class SkinsManager
{
	private static string _PathBase
	{
		get
		{
			return Application.persistentDataPath;
		}
	}

	public static bool SaveTextureWithName(Texture2D t, string nm)
	{
		string path = Path.Combine(_PathBase, nm);
		try
		{
			byte[] bytes = t.EncodeToPNG();
			File.WriteAllBytes(path, bytes);
		}
		catch (Exception message)
		{
			Debug.Log(message);
		}
		try
		{
		}
		catch (Exception ex)
		{
			Debug.Log("Exception in _ScreenshotWriteToAlbum: " + ex);
		}
		return true;
	}

	public static Texture2D TextureForName(string nm)
	{
		Texture2D texture2D = new Texture2D(64, 32);
		try
		{
			byte[] data = File.ReadAllBytes(Path.Combine(_PathBase, nm));
			texture2D.LoadImage(data);
		}
		catch (Exception message)
		{
			Debug.Log(message);
		}
		return texture2D;
	}

	public static bool DeleteTexture(string nm)
	{
		try
		{
			File.Delete(Path.Combine(_PathBase, nm));
		}
		catch (Exception message)
		{
			Debug.Log(message);
		}
		return true;
	}
}
