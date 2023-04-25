using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class PlayerNeeds : MonoBehaviour, IDamagable
{
    public Need health;
    public Need hunger;
    public Need thirst;
    public Need sleep;

    public float noHungerHealthDecay;
    public float noThirstHealthDecay;

    public UnityEvent onTakeDamage;

    public static PlayerNeeds instance;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        health.curValue = health.startValue;
        hunger.curValue = hunger.startValue;
        thirst.curValue = thirst.startValue;
        sleep.curValue = sleep.startValue;
    }

    private void Update()
    {
        hunger.Substract(hunger.decayRate * Time.deltaTime);
        thirst.Substract(thirst.decayRate * Time.deltaTime);
        sleep.Add(sleep.regenRate * Time.deltaTime);

        if (hunger.curValue <= 0.0f)
            health.Substract(noHungerHealthDecay * Time.deltaTime);
        if (thirst.curValue <= 0.0f)
            health.Substract(noHungerHealthDecay * Time.deltaTime);

        if (health.curValue <= 0.0f)
            Die();

        //update UI bars
        health.uiBar.fillAmount = health.GetPercentage();
        hunger.uiBar.fillAmount = hunger.GetPercentage();
        thirst.uiBar.fillAmount = thirst.GetPercentage();
        sleep.uiBar.fillAmount = sleep.GetPercentage();
    }

    public void Heal(float amount)
    {
        health.Add(amount);
    }

    public void Eat(float amount)
    {
        hunger.Add(amount);
    }

    public void Drink(float amount)
    {
        thirst.Add(amount);
    }

    public void Sleep(float amount)
    {
        sleep.Substract(amount);
    }

    public void TakePhysicalDamage(int amount)
    {
        health.Substract(amount);
        onTakeDamage?.Invoke();
    }

    public void Die()
    {

    }
}

public interface IDamagable
{
    void TakePhysicalDamage(int damageAmount);
}