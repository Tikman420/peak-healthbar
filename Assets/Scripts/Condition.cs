using System;
using System.Collections.Generic;
using UnityEngine;


public interface ICondition
{
    int Amount { get; }

    int deterioratingSpeed { get; set; }
    string AcceptedTag { get; set; }

    GameObject StatusVisual {  get; set; }

    //update this condition
    //returns: total amount changed
    int Update();

    //remove a specific amount from this condition
    void Remove(int amount);

    //add to the condition
    //returns: if addition was succesful
    bool Add(int amount, string tag);
}

public class Condition : ICondition
{
    public int Amount { get; protected set; }

    public int deterioratingSpeed { get; set; }

    public string AcceptedTag { get; set;}

    private RectTransform statusRect;

    private GameObject statusVisual;
    public GameObject StatusVisual
    {
        get
        {
            return statusVisual;
        } 

        set
        {
            statusVisual = value;
            statusRect = statusVisual.GetComponent<RectTransform>();
        }
    }

    public bool Add(int amount, string tag)
    {
        Debug.Log(tag);
        if (AcceptedTag == tag) {
            Amount += amount;
            statusVisual.SetActive(true);

            //update condition size
            statusRect.sizeDelta = new Vector2(StatusBar.tickSize*Amount, statusRect.sizeDelta.y);

            return true;
        }
        return false;
    }

    public void Remove(int amount)
    {
        Amount -= amount;

        if (Amount <= 0)
        {
            Amount = 0;
            statusVisual.SetActive(false);
        }

        //update condition size
        statusRect.sizeDelta = new Vector2(StatusBar.tickSize * Amount, statusRect.sizeDelta.y);

        //Debug.Log("removed a total of " + amount + " from total: " + Amount);
    }

    public int Update()
    {
        if (deterioratingSpeed == 0 || Amount <= 0)
        {
            return 0;
        }

        int deterioration = Mathf.RoundToInt(deterioratingSpeed);

        Remove(deterioration);
        return -deterioration;
    }
}
