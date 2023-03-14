using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStatusData
{
    Dictionary<string, StatusData> dicStatus = new Dictionary<string, StatusData>();

    bool bRefresh = false;  // true ��ȭ�� �־���
    StatusData totalStauts = new StatusData();

    public void AddStatusData(string strKey, StatusData statusData)
    {
        dicStatus.Remove(strKey);           // ���������� �ִٸ� ����
        dicStatus.Add(strKey, statusData);  // ���Ӱ� �߰�
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
