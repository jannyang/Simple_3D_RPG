using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Inventory inven;

    private void OnTriggerEnter(Collider other)
    {
        FieldItems item = other.GetComponent<FieldItems>();
        if(item != null)
        {
            inven.AddItem(item);
            Destroy(other.gameObject);
        }
    }
}
