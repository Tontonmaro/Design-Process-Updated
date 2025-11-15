using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewBlindBox", menuName = "Shop/BlindBox")]
public class BlindBoxData : ScriptableObject
{
    public string boxName;
    public List<BlindBoxReward> rewards;

    [System.Serializable]
    public class BlindBoxReward
    {
        public string rewardName;
        public Sprite rewardImage;
        public GameObject rewardPrefab;
        [Range(0f, 1f)] public float chance;
    }

    public BlindBoxReward getRandomReward()
    {
        float roll = Random.value;
        float cumulative = 0f;

        foreach (var reward in rewards)
        {
            cumulative += reward.chance;
            if (roll <= cumulative)
                return reward;
        }

        return rewards[rewards.Count - 1];
    }
}
