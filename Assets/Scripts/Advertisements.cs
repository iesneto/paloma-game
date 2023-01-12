using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.Networking;

namespace Gamob
{
    public class Advertisements : MonoBehaviour, IUnityAdsInitializationListener, IUnityAdsLoadListener, IUnityAdsShowListener
    {
    [SerializeField] string _androidGameId;
    [SerializeField] string _iOSGameId;
    [SerializeField] string _androidAdUnitInterstitialId = "Interstitial_Android";
    [SerializeField] string _iOsAdUnitInterstitialId = "Interstitial_iOS";
    [SerializeField] string _androidAdUnitRewardedId = "Rewarded_Android";
    [SerializeField] string _iOsAdUnitRewardedId = "Rewarded_iOS";
    [SerializeField] bool _testMode = true;
    private string _gameId;
    public string _adUnitInterstitialId;
    public string _adUnitRewardedId = null;
    public bool isReady = false;
    public bool interstitialLoaded = false;
    public bool rewardedLoaded = false;

        //void Start() => InitializeAds();
        void Start() => StartCoroutine(CheckInternetConnection());

        IEnumerator CheckInternetConnection()
        {
            while(!isReady)
            {                
                UnityWebRequest www = new UnityWebRequest("http://google.com");
                UnityWebRequestAsyncOperation request = www.SendWebRequest();              
                while (!www.isDone)
                {                    
                    yield return null;
                }
                if (www.result == UnityWebRequest.Result.Success)
                {
                    InitializeAds();
                    //break;
                }
                
                yield return new WaitForSeconds(60);
            }
        }

        public void InitializeAds()
        {
            _gameId = (Application.platform == RuntimePlatform.IPhonePlayer)
            ? _iOSGameId
            : _androidGameId;
            Advertisement.Initialize(_gameId, _testMode, this);

            _adUnitInterstitialId = (Application.platform == RuntimePlatform.IPhonePlayer)
            ? _iOsAdUnitInterstitialId
            : _androidAdUnitInterstitialId;



            // For some reason Preprocessor directives breaks namespace and monobehavior is not recognized in Unity

            //#if UNITY_IOS
            //        _adUnitRewardedId = _iOsAdUnitRewardedId;
            //#elif UNITY_ANDROID
            //            _adUnitRewardedId = _androidAdUnitRewardedId;
            //#endif

            _adUnitRewardedId = (Application.platform == RuntimePlatform.IPhonePlayer)
            ? _iOsAdUnitRewardedId
            : _androidAdUnitRewardedId;
            
        }

        public void OnInitializationComplete()
        {
            isReady = true;
            LoadInterstitialAd();
        }

        public void LoadInterstitialAd()
        {
            Advertisement.Load(_adUnitInterstitialId, this);
        }

        public void LoadRewardedAd()
        {

            Advertisement.Load(_adUnitRewardedId, this);
        }

        public void ShowInterstitialAd()
        {
            Advertisement.Show(_adUnitInterstitialId, this);
        }

        public void ShowRewardedlAd()
        {
            Advertisement.Show(_adUnitRewardedId, this);
        }

        public void OnInitializationFailed(UnityAdsInitializationError error, string message)
        {
            //Debug.Log($"Unity Ads Initialization Failed: {error.ToString()} - {message}");
            //StartCoroutine(RetryInitialization());
        }

        

        public void OnUnityAdsAdLoaded(string placementId)
        {
            
            if (placementId.Equals(_adUnitInterstitialId))
            {
                interstitialLoaded = true;
                gameObject.GetComponent<UIManager>().NotifyInterstitialAdsLoaded();
            }
            else if (placementId.Equals(_adUnitRewardedId))
            {

                rewardedLoaded = true;
                gameObject.GetComponent<UIManager>().NotifyRewardedAdsLoaded();
            }

        }

        public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
        {
            
            if (placementId.Equals(_adUnitInterstitialId))
            {
                interstitialLoaded = false;
            }
            else if (placementId.Equals(_adUnitRewardedId))
            {

                rewardedLoaded = false;
            }
        }

        public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
        {
            //TO DO: Show failure message to the player

        }

        public void OnUnityAdsShowStart(string placementId)
        {

        }

        public void OnUnityAdsShowClick(string placementId)
        {

        }

        public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
        {
            if (placementId.Equals(_adUnitInterstitialId))
            {
                interstitialLoaded = false;
                GameControl.Instance.LoadStage();
            }
            else if (placementId.Equals(_adUnitRewardedId) && showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED))
            {
                rewardedLoaded = false;
                gameObject.GetComponent<UIManager>().AdsDoneShowClaimButton();
            }


        }
    }
}