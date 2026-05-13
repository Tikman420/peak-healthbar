using System;
using System.Collections.Generic;
using UnityEngine;


public interface ICondition
{
    int Amount { get; }

    void Update();
    void Remove(int amount);
    void Add(int amount);
}

internal class Condition : ICondition
{
    private int amount;
    public int Amount { get { return amount; } protected set { amount = value; } }

    public int deterioratingSpeed = 0;

    public Sprite Icon;
    public Color color;

    public void Add(int amount)
    {
        Amount += amount;
    }

    public void Remove(int amount)
    {
        Debug.Log("removed a total of " + amount + " from total: " + Amount);
    }

    public void Update()
    {
        Debug.Log("Deteriorating");
        return;
    }
}
