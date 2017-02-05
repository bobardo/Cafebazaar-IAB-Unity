using UnityEngine;
using SimpleJSON;
using System;

/* Apache License. Copyright (C) Bobardo Studio - All Rights Reserved.
 * Unauthorized publishing the plugin with different name is strictly prohibited.
 * This plugin is free and no one has right to sell it to others.
 * http://bobardo.com
 * http://opensource.org/licenses/Apache-2.0
 */

[RequireComponent (typeof(InAppStore))]
public class StoreHandler : MonoBehaviour {

    public string publicKey;
    public string payload;

    private AndroidJavaObject pluginUtilsClass = null;
    private AndroidJavaObject activityContext = null;
    
    private void initiateBilling()
    {
        if (pluginUtilsClass == null)
        {
            using (AndroidJavaClass activityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            {
                activityContext = activityClass.GetStatic<AndroidJavaObject>("currentActivity");
            }

            using (AndroidJavaClass pluginClass = new AndroidJavaClass("com.bobardo.bazaar.iab.ServiceBillingBazaar"))
            {
                if (pluginClass != null)
                {

                    pluginUtilsClass = pluginClass.CallStatic<AndroidJavaObject>("instance");
                    pluginUtilsClass.CallStatic("setContext", activityContext);
                    pluginUtilsClass.Call("setPublicKey", publicKey);
                    pluginUtilsClass.Call("startIabServiceInBg");

                }
            }
        }
    }

    public void BuyAndConsume(string produc_sku)
    {
        initiateBilling();

        if (pluginUtilsClass != null)
        {
            pluginUtilsClass.Call("PurchaseAndConsume", produc_sku, payload);
        }
    }

    public void BuyProduct(string produc_sku)
    {
        initiateBilling();

        if (pluginUtilsClass != null)
        {
            pluginUtilsClass.Call("launchPurchaseFlow", produc_sku, payload);
        }
    }

    public void CheckInventory(string produc_sku)
    {
        initiateBilling();

        if (pluginUtilsClass != null)
        {
            pluginUtilsClass.Call("checkHasPurchase", produc_sku);
        }
    }

    public void getPurchaseResult(string result)
    {
        if (result.Length == 0 || result == "" || result == null)
        {
            GetComponent<InAppStore>().purchasedFailed(16, "unknown error!!!");
            return;
        }
        try
        {
            JSONNode purchaseResult = JSON.Parse(result);
            int errorCode = purchaseResult["errorCode"].AsInt;
            string data = purchaseResult["data"].Value.ToString();
            if (errorCode == 0)
            {
                GetComponent<InAppStore>().purchasedSuccessful(getPurchaseData(data));
            }
            else
            {
                GetComponent<InAppStore>().purchasedFailed(errorCode, data);
            }

        }
        catch
        {
            GetComponent<InAppStore>().purchasedFailed(17, "the result from cafeBazaar is not valid.");
        }
    }

    public void getInventoryResult(string result)
    {
        if (result.Length == 0 || result == "" || result == null)
        {
            GetComponent<InAppStore>().purchasedFailed(16, "unknown error!!!");
            return;
        }
        try
        {
            JSONNode purchaseResult = JSON.Parse(result);
            int errorCode = purchaseResult["errorCode"].AsInt;
            string data = purchaseResult["data"].Value.ToString();
            if (errorCode == 0)
            {
                GetComponent<InAppStore>().purchasedSuccessful(getPurchaseData(data));
            }
            else
            {
                GetComponent<InAppStore>().purchasedFailed(errorCode, data);
            }

        }
        catch
        {
            GetComponent<InAppStore>().purchasedFailed(17, "the result from cafeBazaar is not valid.");
        }
    }

    private Purchase getPurchaseData(string data)
    {
        JSONNode info = JSONNode.Parse(data);
        Purchase purchase = new Purchase();
        purchase.orderId = info["orderId"].Value.ToString();
        purchase.purchaseToken = info["purchaseToken"].Value.ToString();
        purchase.payload = info["developerPayload"].Value.ToString();
        purchase.packageName = info["packageName"].Value.ToString();
        purchase.purchaseState = info["purchaseState"].AsInt;
        purchase.purchaseTime = info["purchaseTime"].Value.ToString();
        purchase.productId = info["productId"].Value.ToString();
        purchase.json = data;
        return purchase;
    }

    public void DebugLog(string msg)
    {
        Debug.Log(msg);
    }

    void OnApplicationQuit()
    {
        if (pluginUtilsClass != null)
        {
            pluginUtilsClass.Call("stopIabHelper");
        }
    }
}

public class Purchase
{
    public string orderId;
    public string purchaseToken;
    public string payload;
    public string packageName;
    public int purchaseState;
    public string purchaseTime;
    public string productId;
    public string json;
}

[Serializable]
public class Product
{
    public enum ProductType { Consumable, NonConsumable };
    public string productId;
    public ProductType type;
}
