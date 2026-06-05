using System;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

//manger for the statusbar
internal class StatusBar
{
    //settings
    public int health = 1000;
    private const int sprintDepletion = 2;
    private const float hungerTimerLength = 10;
    private const int hungerAmount = 20;

    public List<ICondition> statusconditions = new List<ICondition>();
    public List<ScriptableCondition> statusTypes;
    public bool dead;
    public bool isSprinting;

    public deathmanager deathmanager = new deathmanager();

    public static float tickSize {get; private set;}

    private Slider staminaSlider;
    private HorizontalLayoutGroup bar;
    private RectTransform staminaRect;
    public float hungerTimer;

    public Slider StaminaSlider {
        get { return staminaSlider; }
        set
        {
            staminaSlider = value;
            staminaSlider.maxValue = health;
            staminaSlider.value = health;
            staminaRect = StaminaSlider.GetComponent<RectTransform>();
            tickSize = staminaRect.sizeDelta.x/health;
            bar = staminaSlider.transform.parent.GetComponent<HorizontalLayoutGroup>();
        } }

    public void update()
    {
        //add hunger
        hungerTimer += Time.deltaTime;
        if (hungerTimer > hungerTimerLength) 
        {
            AddAmount("Hunger", hungerAmount);
            hungerTimer = 0;
        }


        //update the conditions
        foreach (ICondition condition in statusconditions)
        {
            int updatedTotal = condition.Update();
            if (updatedTotal == 0)
            {
                continue;
            }
            staminaSlider.maxValue -= updatedTotal;
        }

        //check for death
        if (staminaSlider.maxValue <= 0)
        {
            dead = deathmanager.updateDeath();
        }
        else
        {
            deathmanager.resetDeath();
        }

        //update sprintbar
        if (isSprinting)
        {
            health -= sprintDepletion;
        }
        else if (health < StaminaSlider.maxValue)
        {
            health += sprintDepletion;
        }
        StaminaSlider.value = health;

        //update bar
        staminaRect.sizeDelta = new Vector2(staminaSlider.maxValue * tickSize, staminaRect.sizeDelta.y);
        bar.SetLayoutHorizontal();
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
                staminaRect.sizeDelta -= new Vector2(amount * tickSize, 0);
                StaminaSlider.maxValue -= amount;
                return;
            }
        }

        //generate a new status
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

        statusconditions.Add(ConditionFactory.AddCondition(result, bar.transform));
        if(statusconditions[^1].Add(amount, type))
        {
            StaminaSlider.maxValue -= amount;
        }
    }

    public void AddVelocity(string type, Vector3 velocity)
    {
        //AddAmount("Damage", Mathf.RoundToInt(velocity.y * -1));
        AddAmount(type, Mathf.RoundToInt(Vector3.Distance(Vector3.zero, velocity)*20));
    }
}
