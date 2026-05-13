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
    private int amount;
    public int Amount { get { return amount; } protected set { amount = value; } }

    public int deterioratingSpeed = 0;

    public string acceptedTag;
    public string AcceptedTag { get { return acceptedTag; } set { acceptedTag = value; } }

    public Sprite icon;
    public Sprite Icon { get { return icon; } set { icon = value; } }

    public Color color;
    public Color Color { get { return color; } set { color = value; } }

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
