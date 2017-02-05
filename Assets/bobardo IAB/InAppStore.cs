using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/* Apache License. Copyright (C) Bobardo Studio - All Rights Reserved.
 * Unauthorized publishing the plugin with different name is strictly prohibited.
 * This plugin is free and no one has right to sell it to others.
 * http://bobardo.com
 * http://opensource.org/licenses/Apache-2.0
 */

[RequireComponent(typeof(StoreHandler))]
public class InAppStore : MonoBehaviour
{
    public Product[] products;

    public Text txtResult;
    public Text coinTxt;
    public GameObject successDialog, errorDialog;
    public Button btnDoubleCoin;
    public Text txtSuccessDialog;
    public Text txtErrorDialog;

    private int coin = 0;
    private bool doubleCoin;

    private int selectedProductIndex;

    void Start()
    {
        coin = PlayerPrefs.GetInt("coin", 0);
        coinTxt.text = coin + "$";
        doubleCoin = (PlayerPrefs.GetInt("doubleCoin", 0) == 1);
        btnDoubleCoin.interactable = !doubleCoin;
    }

    public void purchasedSuccessful(Purchase purchase)
    {
        // purchase was successful, give user the pruduct
        txtResult.text = "purchased successful. product: " + purchase.productId + " , token: " + purchase.orderId + "\n" + txtResult.text;

        switch (selectedProductIndex)
        {
            case 0: // 500 coin
                saveCoin(500);
                break;
            case 1: // enable double coin
                doubleCoin = true;
                PlayerPrefs.SetInt("doubleCoin", 1);
                btnDoubleCoin.interactable = false;
                break;
            default:
                throw new UnassignedReferenceException("you forgot to give user the product after purchase. product: " + purchase.productId);
        }

        txtSuccessDialog.text = "Thanks for purchasing.";
        successDialog.SetActive(true);
    }

    public void purchasedFailed(int errorCode, string info)
    {
        // purchase failed. show user the proper message
        switch (errorCode)
        {
            case 1: // error connecting cafeBazaar
            case 2: // error connecting cafeBazaar
            case 4: // error connecting cafeBazaar
            case 5: // error connecting cafeBazaar

                break;
            case 6: // user canceled the purchase

                break;
            case 7: // purchase failed

                break;
            case 8: // failed to consume product. but the purchase was successful.

                break;
            case 12: // error setup cafebazaar billing
            case 13: // error setup cafebazaar billing
            case 14: // error setup cafebazaar billing

                break;
            case 15: // you should enter your public key

                break;
            case 16: // unkown error happened

                break;
            case 17: // the result from cafeBazaar is not valid.

                break;
        }

        txtResult.text = "errorCode: " + errorCode + ", " + info + "\n" + txtResult.text;
        txtErrorDialog.text = "operation failed." + "\n" + info;
        errorDialog.SetActive(true);
    }

    public void userHasThisProduct(Purchase purchase)
    {
        // user already has this product
        txtResult.text = "user has product: " + purchase.productId + " , token: " + purchase.orderId + "\n" + txtResult.text;
        switch (selectedProductIndex)
        {
            case 0: // 500 coin
                saveCoin(500);
                break;
            case 1: // enable double coin
                doubleCoin = true;
                PlayerPrefs.SetInt("doubleCoin", 1);
                btnDoubleCoin.interactable = false;
                break;
            default:
                throw new UnassignedReferenceException("you forgot to give user the product after check inventory. product: " + purchase.productId);
        }

        txtSuccessDialog.text = "you had this product.";
        successDialog.SetActive(true);
    }

    public void failToGetUserInventory(int errorCode, string info)
    {
        // user has not this product or some error happened
        switch (errorCode)
        {
            case 3:  // error connecting cafeBazaar
            case 10: // error connecting cafeBazaar

                break;
            case 9: // user didn't login to cafeBazaar

                break;
            case 11: // user has not this product

                break;
            case 12: // error setup cafebazaar billing
            case 13: // error setup cafebazaar billing
            case 14: // error setup cafebazaar billing

                break;
            case 15: // you should enter your public key

                break;
            case 16: // unkown error happened

                break;
            case 17: // the result from cafeBazaar is not valid.

                break;
        }
        
        txtResult.text = "errorCode: " + errorCode + ", " + info + "\n" + txtResult.text;
        txtErrorDialog.text = "operation failed." + "\n" + info;
        errorDialog.SetActive(true);
    }

    public void purchaseProduct(int productIndex)
    {
        selectedProductIndex = productIndex;
        Product product = products[productIndex];
        if (product.type == Product.ProductType.Consumable)
        {
            GetComponent<StoreHandler>().BuyAndConsume(product.productId);
        }
        else if (product.type == Product.ProductType.NonConsumable)
        {
            GetComponent<StoreHandler>().BuyProduct(product.productId);
        }
    }

    public void checkIfUserHasProduct(int productIndex)
    {
        selectedProductIndex = productIndex;
        GetComponent<StoreHandler>().CheckInventory(products[productIndex].productId);
    }
    
    private void saveCoin(int value)
    {
        coin = PlayerPrefs.GetInt("coin", 0);
        PlayerPrefs.SetInt("coin", coin + value);

        StartCoroutine(animateCoinsText(coin, value));
    }

    private IEnumerator animateCoinsText(int coin, int value)
    {

        float part = (float)value / 20;

        for (int i = 0; i < 20; i++)
        {
            int extra = (int)((i + 1) * part);
            coinTxt.text = coin + extra + "$";
            yield return new WaitForSecondsRealtime(0.03f);
        }
    }

    public void addFreeCoin(int value)
    {
        if (doubleCoin) value *= 2;
        saveCoin(value);
    }
}

