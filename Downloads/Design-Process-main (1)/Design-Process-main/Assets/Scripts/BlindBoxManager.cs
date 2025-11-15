using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlindBoxManager : MonoBehaviour
{
    public ShoppingCart cart;
    public BuyNowCart buyNowCart;
    public Transform resultsParent;
    public GameObject rewardUIPrefab;
    public ItemSelect select;

    public RewardViewer rewardViewer;

    public List<GameObject> rewards = new List<GameObject>();

    public void openAllBoxes()
    {
        if (!select.boughtNow)
        {
            foreach (GameObject cartItem in cart.cartItems)
            {
                ItemDetails details = cartItem.GetComponent<ItemDetails>();
                if (details.blindBoxData == null) continue;

                for (int i = 0; i < details.quantity; i++)
                {
                    var reward = details.blindBoxData.getRandomReward();
                    displayReward(reward);
                }
            }
        }
        else
        {
            foreach (GameObject buyNowItem in buyNowCart.buyNowItems)
            {
                ItemDetails details = buyNowItem.GetComponent<ItemDetails>();
                if (details.blindBoxData == null) continue;

                for (int i = 0; i < details.quantity; i++)
                {
                    var reward = details.blindBoxData.getRandomReward();
                    displayReward(reward);
                }
            }
        }
    }

    void displayReward(BlindBoxData.BlindBoxReward reward)
    {
        GameObject rewardUI = Instantiate(rewardUIPrefab, resultsParent);
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)resultsParent);
        rewards.Add(rewardUI);
        RewardUI ui = rewardUI.GetComponent<RewardUI>();
        ui.setup(reward, rewardViewer);
        BlindBoxAnimation anim = rewardUI.GetComponent<BlindBoxAnimation>();
        anim.playOpenAnimation(reward.rewardImage, reward.rewardName);
    }

    public void clearRewards()
    {
        for (int i = 0; i < rewards.Count; i++)
        {
            Destroy(rewards[i]);
        }
    }
}
