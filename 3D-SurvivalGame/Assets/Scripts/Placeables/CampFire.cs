using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampFire : Building, IInteractable
{
    public GameObject particle;
    public GameObject light;
    private bool isOn = false;
    private Vector3 lightStartPos;

    [Header("Damage")]
    public int damage;
    public float damageRate;

    private List<IDamagable> thingsToDamage = new List<IDamagable>();

    private void Start()
    {
        lightStartPos = light.transform.localPosition;
        StartCoroutine(DealDamage());
    }

    private void Update()
    {
        if(isOn)
        {
            float x = Mathf.PerlinNoise(Time.time * 3.0f, 0.0f) / 5.0f;
            float z = Mathf.PerlinNoise(0.0f, Time.time * 3.0f) / 5.0f;

            light.transform.localPosition = lightStartPos + new Vector3(x, 0.0f, z);
        }
    }

    public string GetInteractPrompt()
    {
        return isOn ? "Turn Off" : "Turn On";
    }

    public void OnInteract()
    {
        isOn = !isOn;

        particle.SetActive(isOn);
        light.SetActive(isOn);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<IDamagable>() != null)
            thingsToDamage.Add(other.gameObject.GetComponent<IDamagable>());
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<IDamagable>() != null)
            thingsToDamage.Remove(other.gameObject.GetComponent<IDamagable>());
    }

    IEnumerator DealDamage()
    {
        while(true)
        {
            if(isOn)
            {
                for (int i = 0; i < thingsToDamage.Count; i++)
                    thingsToDamage[i].TakePhysicalDamage(damage);
            }
            yield return new WaitForSeconds(damageRate);
        }
    }

    public override string GetCustomProperties()
    {
        return isOn.ToString();
    }

    public override void ReceiveCustomProperties(string props)
    {
        isOn = props == "True" ? true : false;

        particle.SetActive(isOn);
        light.SetActive(isOn);
    }
}
