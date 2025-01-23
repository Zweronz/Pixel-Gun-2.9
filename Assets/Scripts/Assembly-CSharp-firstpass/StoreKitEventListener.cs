using System.Collections.Generic;
using Prime31;
using UnityEngine;

public class StoreKitEventListener : MonoBehaviour
{
	public static string elixirSettName = "NumberOfElixirsSett";

	public static string bigAmmoPackID = "bigammopack";

	public static string fullHealthID = "fullhealth";

	public static string minerWeaponID = "minerweapon";

	public static string elixirID = "elixir";

	public static string crystalswordID = "crystalsword";

	public GameObject messagePrefab;

	public static string[] skus = new string[5] { bigAmmoPackID, fullHealthID, crystalswordID, minerWeaponID, elixirID };

	public static bool billingSupported = false;

	public void ProvideContent()
	{
		PlayerPrefs.SetInt("BigAmmoPackBought", 1);
		PlayerPrefs.Save();
	}

	private void Start()
	{
		GoogleIABManager.billingSupportedEvent += billingSupportedEvent;
		GoogleIABManager.billingNotSupportedEvent += billingNotSupportedEvent;
		GoogleIABManager.queryInventorySucceededEvent += queryInventorySucceededEvent;
		GoogleIABManager.queryInventoryFailedEvent += queryInventoryFailedEvent;
		GoogleIABManager.purchaseCompleteAwaitingVerificationEvent += purchaseCompleteAwaitingVerificationEvent;
		GoogleIABManager.purchaseSucceededEvent += purchaseSucceededEvent;
		GoogleIABManager.purchaseFailedEvent += purchaseFailedEvent;
		GoogleIABManager.consumePurchaseSucceededEvent += consumePurchaseSucceededEvent;
		GoogleIABManager.consumePurchaseFailedEvent += consumePurchaseFailedEvent;
		string text = "your public key from the Android developer portal here";
		text = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEA2fddncCpVwPU3m4ZzG8MfQTrxf3LBdjWwOV4LBRy2q4Kp/gPYi5QQaNJjsiQAbpIR51qSEJv9EomOi8+JZ4rWO52zOaLeumnKzpv++QVOllbGxaSwwSPDEZ0++eKmdsl5r+xzVvd20ey4n5tYotrRdYQfypZKYuHiMGvpsiIXf0rwv3yMNhVU7MDtbDgAs8zriBvPqCtkrRLnZdG+2dQEZ+hDPns0gO+N8y1V7odOHg4bDUceaK8al9DHcVKNItCMnOFyLHx++vKzHSLiXw2ojSUR1cfSbTkyyOTw9r9emHxxuGmc2/qWp7n/Qin1ksuAhyYFGOC9RClxxu1ygXKTQIDAQAB";
		GoogleIAB.init(text);
	}

	private void OnDestroy()
	{
		GoogleIABManager.billingSupportedEvent -= billingSupportedEvent;
		GoogleIABManager.billingNotSupportedEvent -= billingNotSupportedEvent;
		GoogleIABManager.queryInventorySucceededEvent -= queryInventorySucceededEvent;
		GoogleIABManager.queryInventoryFailedEvent -= queryInventoryFailedEvent;
		GoogleIABManager.purchaseCompleteAwaitingVerificationEvent += purchaseCompleteAwaitingVerificationEvent;
		GoogleIABManager.purchaseSucceededEvent -= purchaseSucceededEvent;
		GoogleIABManager.purchaseFailedEvent -= purchaseFailedEvent;
		GoogleIABManager.consumePurchaseSucceededEvent -= consumePurchaseSucceededEvent;
		GoogleIABManager.consumePurchaseFailedEvent -= consumePurchaseFailedEvent;
	}

	private void billingSupportedEvent()
	{
		billingSupported = true;
		Debug.Log("billingSupportedEvent");
		GoogleIAB.queryInventory(skus);
	}

	private void billingNotSupportedEvent(string error)
	{
		billingSupported = false;
		Debug.Log("billingNotSupportedEvent: " + error);
	}

	private void queryInventorySucceededEvent(List<GooglePurchase> purchases, List<GoogleSkuInfo> skus)
	{
		Debug.Log("queryInventorySucceededEvent");
		Utils.logObject(purchases);
		Utils.logObject(skus);
		foreach (GooglePurchase purchase in purchases)
		{
			if (purchase.productId.Equals(minerWeaponID))
			{
				GameObject gameObject = GameObject.FindGameObjectWithTag("WeaponManager");
				if ((bool)gameObject)
				{
					gameObject.SendMessage("AddMinerWeaponToInventoryAndSaveInApp");
				}
			}
			else if (purchase.productId.Equals(crystalswordID))
			{
				GameObject gameObject2 = GameObject.FindGameObjectWithTag("WeaponManager");
				if ((bool)gameObject2)
				{
					gameObject2.SendMessage("AddSwordToInventoryAndSaveInApp");
				}
			}
			else
			{
				GoogleIAB.consumeProduct(purchase.productId);
			}
		}
	}

	private void queryInventoryFailedEvent(string error)
	{
		Debug.Log("queryInventoryFailedEvent: " + error);
	}

	private void purchaseCompleteAwaitingVerificationEvent(string purchaseData, string signature)
	{
		Debug.Log("purchaseCompleteAwaitingVerificationEvent. purchaseData: " + purchaseData + ", signature: " + signature);
	}

	private void purchaseSucceededEvent(GooglePurchase purchase)
	{
		Debug.Log("purchaseSucceededEvent: " + purchase);
		if (purchase.productId.Equals(elixirID))
		{
			PlayerPrefs.SetInt(elixirSettName, PlayerPrefs.GetInt(elixirSettName, 1) + 1);
			PlayerPrefs.Save();
			GoogleIAB.consumeProduct(purchase.productId);
		}
	}

	private void purchaseFailedEvent(string error)
	{
		Debug.Log("purchaseFailedEvent: " + error);
	}

	private void consumePurchaseSucceededEvent(GooglePurchase purchase)
	{
		Debug.Log("consumePurchaseSucceededEvent: " + purchase);
	}

	private void consumePurchaseFailedEvent(string error)
	{
		Debug.Log("consumePurchaseFailedEvent: " + error);
	}
}
