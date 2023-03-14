using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SimpleJSON;

public class CharacterTemplateData 
{
    string _strkey = string.Empty;
    // string _name = string.Empty;
    StatusData _status = new StatusData();

    public string Key { get { return _strkey; } }
    public StatusData Status { get { return _status; } }


    //"CHARACTER_1":        key
    //{                     nodeData
    //	"MAX_HP": 100,
    //	"ATTACK": 10,
    //	"DEFFENCE": 5,
    //	"SPEED": 3.5
    //},

    //public enum eStatusData
    //{
    //    MAX_HP,
    //    ATTACK,
    //    DEFFENCE,
    //    SPEED,
    //    MAX
    //}

    public CharacterTemplateData(string key, JSONNode nodeData)
    {
        _strkey = key;
        // _name = nodeData["NAME"];

        // Á¤¸®
        for (int i = 0; i < (int)eStatusData.MAX; i++)
        {
            eStatusData eStatus = (eStatusData)i;
            double valueData =
                nodeData[eStatus.ToString()].AsDouble;
            Status.IncreaseData(eStatus, valueData);
        }
    }

}
