using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusData 
{
    private Dictionary<eStatusData, double> dicData = new Dictionary<eStatusData, double>();
    public void InitData()
    {
        dicData.Clear();
    }

    public void Add(StatusData data)
    {
        foreach (KeyValuePair<eStatusData, double> pair in data.dicData)
        {
            IncreaseData(pair.Key, pair.Value);
        }
    }

    public void Copy(StatusData data)
    {
        InitData();

        foreach (KeyValuePair<eStatusData, double> pair in data.dicData)
        {
            IncreaseData(pair.Key, pair.Value);
        }
    }

    public void IncreaseData(eStatusData statusData, double valueData)
    {
        double prevalue = 0.0;
        dicData.TryGetValue(statusData, out prevalue);

        dicData[statusData] = prevalue + valueData;
    }

    public void DecreaseData(eStatusData statusData, double valueData)
    {
        double prevalue = 0.0;
        dicData.TryGetValue(statusData, out prevalue);

        dicData[statusData] = prevalue - valueData;
    }

    public void SetData(eStatusData statusData, double valueData)
    {
        dicData[statusData] = valueData;
    }

    public void RemoveData(eStatusData statusData)
    {
        if (dicData.ContainsKey(statusData))
            dicData.Remove(statusData);
    }

    public double GetStatusData(eStatusData statusData)
    {
        double preValue = 0.0;
        dicData.TryGetValue(statusData, out preValue);

        return preValue;
    }

    public string StatusString()
    {
        string returnStr = string.Empty;

        foreach (var pair in dicData)
        {
            returnStr += pair.Key.ToString();
            returnStr += " " + pair.Value.ToString();
            returnStr += "\n";
        }
        return returnStr;
    }
}
