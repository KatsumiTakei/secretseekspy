using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
    , ISurveyTarget
{
    public void Surveyed(Player player)
    {
        AudioManager.PlaySE("Clear");
        EventManager.BroadcastAddScore(10000);
        EventManager.BroadcastGameFinish(true);
        LogMessage.OutputLog("Bounus Get!");
    }
}
