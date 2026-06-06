using System;
using System.Collections.Generic;
using UnityEngine;


public interface ICondition
{
    int Amount { get; }

    int deterioratingSpeed { get; set; }
    string AcceptedTag { get; set; }

    GameObject StatusVisual {  get; set; }
    ICondition influence {  get; set; }

    //update this condition
    //returns: total amount changed
    int Update();

    //remove a specific amount from this condition
    int remove(int amount);

    //add to the condition
    //returns: if addition was succesful
    int Add(int amount, string tag);
}

public class Condition : ICondition
{
    public int Amount { get; protected set; }
    public int deterioratingSpeed { get; set; }

    public string AcceptedTag { get; set;}

    public ICondition influence { get; set; }

    private RectTransform statusRect;
    private GameObject statusvisual;
    public GameObject StatusVisual
    {
        get
        {
            return statusvisual;
        } 

        set
        {
            statusvisual = value;
            statusRect = statusvisual.GetComponent<RectTransform>();
        }
    }

    public int Add(int amount, string tag)
    {
        if (AcceptedTag == tag) 
        {
            if (influence != null) 
            {
                int influenced = influence.remove(amount);
                Debug.Log(influenced);
                Debug.Log(amount);
                Amount += (amount - influenced);
                amount = (amount - influenced) - influenced;
            }
            else
            {
                Amount += amount;
            }

            if (Amount != 0)
            {
                StatusVisual.SetActive(true);
            }

            //update condition size
            statusRect.sizeDelta = new Vector2(StatusBar.tickSize*Amount, statusRect.sizeDelta.y);

            return amount;
        }
        return -1;
    }

    //returns the amount succesfully removed
    public int remove(int amount)
    {
        Amount -= amount;

        if (Amount <= 0)
        {
            int rest = -Amount;
            Amount = 0;
            StatusVisual.SetActive(false);
            return amount - rest;
        }

        //update condition size
        statusRect.sizeDelta = new Vector2(StatusBar.tickSize * Amount, statusRect.sizeDelta.y);
        return amount;
    }

    public int Update()
    {
        if (deterioratingSpeed == 0 || Amount <= 0)
        {
            return 0;
        }

        int deterioration = Mathf.RoundToInt(deterioratingSpeed);

        remove(deterioration);
        return -deterioration;
    }
}
