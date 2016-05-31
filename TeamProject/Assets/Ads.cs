using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;
using System.Collections;
using admob;

public class Ads : MonoBehaviour {

    public string zoneId;
    private Button _button;


	// Use this for initialization
	void Start () {
        Admob.Instance().initAdmob("ca-app-pub-3940256099942544/6300978111", "ca-app-pub-3940256099942544/1033173712");//admob id with format ca-app-pub-279xxxxxxxx/xxxxxxxx
        //Admob.Instance().showBannerRelative(AdSize.Banner, AdPosition.BOTTOM_CENTER, 0); 
        Admob.Instance().showBannerRelative(new AdSize(160, 50), AdPosition.BOTTOM_LEFT, 0);

      //  AdSize adSize = new AdSize(200, 50);
     //    Admob.Instance().showBannerAbsolute(adSize,0,30);
     //   Admob.Instance().showBannerRelative(AdSize.Banner, AdPosition.BOTTOM_LEFT, 0); 

    }

    // Update is called once per frame
    void Update () {
	
	}

    //public void ShowAdPlacement()
    //{
    //    if (string.IsNullOrEmpty(zoneId)) zoneId = null;

    //    ShowOptions options = new ShowOptions();
    //    options.resultCallback = HandleShowResult;
    //    if (Advertisement.IsReady())
    //    {
    //        Advertisement.Show(zoneId, options);
    //    }
    //}

    //private void HandleShowResult(ShowResult result)
    //{
    //    switch (result)
    //    {
    //        case ShowResult.Finished:
    //            Debug.Log("Video completed. Offer a reward to the player.");
    //            break;
    //        case ShowResult.Skipped:
    //            Debug.LogWarning("Video was skipped.");
    //            break;
    //        case ShowResult.Failed:
    //            Debug.LogError("Video failed to show.");
    //            break;
    //    }
    //}

}
