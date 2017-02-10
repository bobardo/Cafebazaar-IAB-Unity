# Bobardo Unity plugin for Cafebazaar IAB
This is an easy to use unity plugin for cafebazaar IAB system.

**برای اطلاعات بیشتر و راهنمای فارسی به [اینجا][1] مراجعه کنید**

Setup
--------
Open your project in unity, then open the UnityPackage file and click import. In `bobardo IAB` folder drag `com.bobardo IAB` prefab to your scene and drop it. This gameObject will handle IAB system and events. Please note that you should not change its name.

Click the `com.bobardo IAB` and you can see it has 2 scripts as component. In `StoreHandler` component enter your public key (provided by cafebazaar in your developer panel) and payload (optional).

In `InAppStore` component you should define your products in `products` array. For each product define its ID and choose it is consumable or not.

In your project go to `Android -> Plugins` directory and open `AndroidManifest.xml` file. If you don't have `AndroidManifest.xml` just copy the sample of this plugin.

Make sure you have this permission before your application **open** tag in your manifest.
```xml
<uses-permission android:name="com.farsitel.bazaar.permission.PAY_THROUGH_BAZAAR" />
```
And this activity before your application **close** tag.
```xml
<activity
android:name="com.bobardo.bazaar.iab.ServiceBillingBazaar$IabActivity"
android:theme="@android:style/Theme.Translucent.NoTitleBar.Fullscreen" />
```
Purchase Security
--------
We also provided a sample for [purchase security][2] using cafebazaar developers API.

[bobardo.com][3]

[1]: http://bobardo.com/blog/%d9%be%d9%84%d8%a7%da%af%db%8c%d9%86-%d9%be%d8%b1%d8%af%d8%a7%d8%ae%d8%aa-%da%a9%d8%a7%d9%81%d9%87-%d8%a8%d8%a7%d8%b2%d8%a7%d8%b1-%d8%a8%d8%b1%d8%a7%db%8c-%db%8c%d9%88%d9%86%db%8c%d8%aa%db%8c/
[2]: https://github.com/bobardo/Cafebazaar-IAB-Unity-Security
[3]: http://bobardo.com
