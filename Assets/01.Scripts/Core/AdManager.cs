using GoogleMobileAds.Api;
using System;
using UnityEngine;

public class AdManager : MonoBehaviour
{
    public static AdManager Instance { get; private set; }

    private string _adUnitId = "ca-app-pub-3940256099942544/5224354917";


    private RewardedAd _rewardedAd;

    public void Start()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize((InitializationStatus initStatus) => { });

        LoadAd();
    }

    public void ShowRewardedAd()
    {
        if (_rewardedAd != null && _rewardedAd.IsLoaded())
        {
            _rewardedAd.Show();
            _rewardedAd.Destroy();
            LoadAd(); // 광고 재로드
        }
        else
        {
            Debug.Log("Rewarded ad is not loaded yet.");
        }
    }

    private void LoadAd()
    {
        // Clean up the old ad before loading a new one.
        if (_rewardedAd != null)
        {
            _rewardedAd.Destroy();
            _rewardedAd = null;
        }

        Debug.Log("Loading the rewarded ad.");

        // Create our request used to load the ad.
        var adRequest = new AdRequest.Builder().Build();
        _rewardedAd = new RewardedAd(_adUnitId);

        // Load the ad with a callback
        _rewardedAd.LoadAd(adRequest);

        // Subscribe to ad events
        _rewardedAd.OnAdLoaded += HandleRewardedAdLoaded;
        _rewardedAd.OnAdFailedToLoad += HandleRewardedAdFailedToLoad;
        _rewardedAd.OnAdClosed += HandleRewardedAdClosed;
    }

    private void HandleRewardedAdLoaded(object sender, EventArgs args)
    {
        Debug.Log("Rewarded ad loaded successfully.");
    }

    private void HandleRewardedAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        Debug.LogError("Failed to load rewarded ad: " + args.ToString());
    }

    private void HandleRewardedAdClosed(object sender, EventArgs args)
    {
        Debug.Log("Rewarded ad closed. Reloading...");
        LoadAd(); // 광고 종료 후 재로드
    }
}
