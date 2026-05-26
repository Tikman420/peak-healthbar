using System;
using System.Collections.Generic;
using UnityEngine;


public interface ICondition
{
    int Amount { get; }

    string AcceptedTag { get; set; }

    Sprite Icon { get; set; }
    Color Color { get; set; }

    void Update();
    void Remove(int amount);
    void Add(int amount, string tag);
}

internal class Condition : ICondition
{
    public int Amount { get; protected set; }

    public int deterioratingSpeed = 0;

    public string AcceptedTag { get; set;}

    public Sprite Icon { get; set; }

    public Color Color { get; set; }

    public void Add(int amount, string tag)
    {
        if (AcceptedTag == tag) {
            Amount += amount;
        }
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
