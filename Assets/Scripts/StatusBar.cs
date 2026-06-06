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
    
    //other stuff no need to touch
    public List<ICondition> statusConditions = new List<ICondition>();
    public List<ScriptableCondition> statusTypes;
    public bool dead;
    public bool isSprinting;

    public DeathManager deathmanager = new DeathManager();

    public static float tickSize {get; private set;}

    private Slider privateStaminaSlider;
    private HorizontalLayoutGroup bar;
    private RectTransform staminaRect;
    public float hungerTimer;

    public Slider staminaSlider {
        get { return privateStaminaSlider; }
        set
        {
            privateStaminaSlider = value;
            staminaSlider.maxValue = health;
            staminaSlider.value = health;
            staminaRect = staminaSlider.GetComponent<RectTransform>();
            tickSize = staminaRect.sizeDelta.x/health;
            bar = staminaSlider.transform.parent.GetComponent<HorizontalLayoutGroup>();
        } }

    //update statusbar
    public void Update()
    {
        //add hunger
        hungerTimer += Time.deltaTime;
        if (hungerTimer > hungerTimerLength) 
        {
            AddAmount("Hunger", hungerAmount);
            hungerTimer = 0;
        }


        //update the conditions
        foreach (ICondition condition in statusConditions)
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
            dead = deathmanager.UpdateDeath();
        }
        else
        {
            deathmanager.ResetDeath();
        }

        //update sprintbar
        if (isSprinting)
        {
            health -= sprintDepletion;
        }
        else if (health < staminaSlider.maxValue)
        {
            health += sprintDepletion;
        }
        staminaSlider.value = health;

        //update bar
        staminaRect.sizeDelta = new Vector2(staminaSlider.maxValue * tickSize, staminaRect.sizeDelta.y);
        bar.SetLayoutHorizontal();
    }

    //add a specific amount to a condition with type
    //takes: the name of the condition (string) and the total amount that needs to be added (int)
    public void AddAmount(string type, int amount)
    {
        if (amount <= 0)
        {
            return;
        }

        foreach (ICondition condition in statusConditions)
        {
            int addTotal = condition.Add(amount, type);
            if (addTotal != -1)
            {
                staminaSlider.maxValue -= addTotal;
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

        statusConditions.Add(ConditionFactory.AddCondition(result, bar.transform, statusConditions));
        int remove = statusConditions[^1].Add(amount, type);
        if (remove != -1)
        {
            staminaSlider.maxValue -= remove;
        }
    }


    //add a velocity to a condition of a specific type
    //takes: the name of the condition (string) and the velocity (Vector3)
    public void AddVelocity(string type, Vector3 velocity)
    {         
        AddAmount(type, Mathf.RoundToInt(Vector3.Distance(Vector3.zero, velocity)*20));
    }
}
