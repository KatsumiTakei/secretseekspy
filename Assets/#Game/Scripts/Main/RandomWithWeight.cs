using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class RandomWithWeight
{
    /// <summary>
    /// 重み付き抽選を行う
    /// </summary>
    /// <param name="itemWeightPairs">Key: 抽選する対象, Value: 確率の重み</param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T Lotto<T>(IEnumerable<KeyValuePair<T, float>> itemWeightPairs)
    {
        // Weight降順でソート
        var sortedPairs = itemWeightPairs.OrderByDescending(x => x.Value).ToArray();

        // ドロップアイテムの抽選
        float total = sortedPairs.Select(x => x.Value).Sum();

        float randomPoint = Random.Range(0, total);

        // randomPointの位置に該当するキーを返す
        foreach (KeyValuePair<T, float> elem in sortedPairs)
        {
            if (randomPoint < elem.Value)
            {
                return elem.Key;
            }

            randomPoint -= elem.Value;
        }

        return sortedPairs[sortedPairs.Length - 1].Key;
    }
}