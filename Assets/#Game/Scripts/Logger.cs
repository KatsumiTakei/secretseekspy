//  @note   http://nanoappli.com/blog/archives/6511

using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEngine;
using TMPro;

public class Logger : MonoBehaviour
{
    [SerializeField]
    TextMeshPro drawMsg = null;

    // ログの記録
    static List<string> logMsg_ = new List<string>();
    GUIStyle style_ = new GUIStyle();

    const float EraseTime = 5.0f;
    static List<string> logMsgErase_ = new List<string>();
    float eraseCnt = 0.0f;
    const int MaxLogNum = 2;

    public static void Log(string msg)
    {
        logMsg_.Add(msg);
        if (logMsg_.Count > MaxLogNum)
        {// 直近のみ保存する
            logMsg_.RemoveAt(0);
        }
    }

    void PushLog()
    {

    }

    void PopLog(int index)
    {
        logMsg_.RemoveAt(0);
    }

    private void Update()
    {
        if (logMsg_.Count == 0)
            return;

        // 出力された文字列を改行でつなぐ
        string outMessage = string.Empty;
        foreach (string msg in logMsg_)
        {
            outMessage += msg + System.Environment.NewLine;
        }

        eraseCnt += Time.deltaTime;
        if (eraseCnt > EraseTime)
        {
            eraseCnt = 0;
            logMsg_.RemoveAt(0);
        }

        drawMsg.text = outMessage;
    }

}
