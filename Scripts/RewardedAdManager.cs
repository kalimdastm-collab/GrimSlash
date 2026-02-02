using UnityEngine;
using TapsellPlusSDK;

public class RewardedAdManager : MonoBehaviour
{
    public static RewardedAdManager Instance;

    [Header("Tapsell Zone ID")]
    public string rewardedZoneId = "REWARDED_ZONE_ID";

    private string responseId;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        RequestRewardedAd();
    }

    public void RequestRewardedAd()
    {
        TapsellPlus.RequestRewardedVideoAd(
            rewardedZoneId,

            response =>
            {
                Debug.Log("Rewarded Ad Requested");
                responseId = response.responseId;
            },

            error =>
            {
                Debug.LogError("Ad Request Error: " + error.ToString());
            }
        );
    }

   public void ShowRewardedAd()
    {
        if (string.IsNullOrEmpty(responseId))
        {
            Debug.Log("Ad not ready yet");
            return;
        }

        TapsellPlus.ShowRewardedVideoAd(
            responseId,

            // onOpen
            ad =>
            {
                Debug.Log("Ad Opened");
            },

            // onReward
            ad =>
            {
                Debug.Log("REWARD GIVEN");
                GiveReward();
            },

            // onClose
            ad =>
            {
                Debug.Log("Ad Closed");
                RequestRewardedAd();
            },

            error =>
            {
                Debug.LogError("🚫 Show Error: " + error.ToString());
            }
        );
    }


    void GiveReward()
    {
        Debug.Log("Player rewarded");
    }
}
