using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;
using System.Collections;


public class Ads : MonoBehaviour {

    public string zoneId;
    private Button _button;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void ShowAdPlacement()
    {
        if (string.IsNullOrEmpty(zoneId)) zoneId = null;

        ShowOptions options = new ShowOptions();
        options.resultCallback = HandleShowResult;
        if (Advertisement.IsReady())
        {
            Advertisement.Show(zoneId, options);
        }
    }

    private void HandleShowResult(ShowResult result)
    {
        switch (result)
        {
            case ShowResult.Finished:
                Debug.Log("Video completed. Offer a reward to the player.");
                break;
            case ShowResult.Skipped:
                Debug.LogWarning("Video was skipped.");
                break;
            case ShowResult.Failed:
                Debug.LogError("Video failed to show.");
                break;
        }
    }

}
