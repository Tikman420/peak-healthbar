using System;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

internal class StatusBar
{
    public List<ICondition> statusconditions = new List<ICondition>();
    public List<ScriptableCondition> statusTypes;
    public bool dead;
    public bool isSprinting;
    public int sprintDepletion = 2;

    public deathmanager deathmanager = new deathmanager();

    public int health = 1000;
    public float tickSize;

    private Slider staminaslider;
    public Slider StaminaSlider {
        get { return staminaslider; }
        set
        {
            staminaslider = value;
            staminaslider.maxValue = health;
            staminaslider.value = health;

            tickSize = staminaslider.GetComponent<RectTransform>().sizeDelta.x/health;
        } }

    public void update()
    {
        //Debug.Log(statusconditions.Count);
        int statusTotal = 0;

        foreach (ICondition condition in statusconditions)
        {
            Debug.Log(condition.Amount);
            condition.Update();
            statusTotal += condition.Amount;
        }

        if (staminaslider.maxValue == 0)
        {
            dead = deathmanager.updateDeath();
        }
        else
        {
            deathmanager.resetDeath();
        }

        if (isSprinting)
        {
            health -= sprintDepletion;
        }
        else if (health < StaminaSlider.maxValue)
        {
            health += sprintDepletion;
        }
        StaminaSlider.value = health;
    }

    public void AddAmount(string type, int amount)
    {
        if (amount <= 0)
        {
            return;
        }

        foreach (ICondition condition in statusconditions)
        {
            if (condition.Add(amount, type))
            {
                return;
            }
        }

        ScriptableCondition result = null;

        foreach (ScriptableCondition status in statusTypes)
        {
            if (status.acceptedTag != type)
            {
                continue;
            }
            result = status;
            break;
        }
        if (result == null)
        {
            return;
        }

        statusconditions.Add(ConditionFactory.AddCondition(result));
        statusconditions[^1].Add(amount, type);
    }

    public void Add(string type, Vector3 velocity)
    {
        //AddAmount("Damage", Mathf.RoundToInt(velocity.y * -1));
        Debug.Log(velocity);
        AddAmount(type, Mathf.RoundToInt(velocity.x*10));
    }
}
