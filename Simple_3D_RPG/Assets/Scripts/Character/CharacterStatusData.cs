using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStatusData
{
    Dictionary<string, StatusData> dicStatus = new Dictionary<string, StatusData>();

    bool bRefresh = false;  // true 변화가 있었다
    StatusData totalStauts = new StatusData();

    public void AddStatusData(string strKey, StatusData statusData)
    {
        dicStatus.Remove(strKey);           // 기존정보가 있다면 삭제
        dicStatus.Add(strKey, statusData);  // 새롭게 추가
        bRefresh = true;
    }

    public void RemoveStatusData(string strKey)
    {
        dicStatus.Remove(strKey);
        bRefresh = true;
    }

    public double GetStatusData(eStatusData statusType)
    {
        RefreshTotalStatus();
        return totalStauts.GetStatusData(statusType);
    }

    void RefreshTotalStatus()
    {
        if (bRefresh == false)
            return;

        totalStauts.InitData();

        foreach (KeyValuePair<string, StatusData> pair in dicStatus)
        {
            totalStauts.Add(pair.Value);
        }
        bRefresh = false;
    }
}
