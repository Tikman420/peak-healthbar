using System;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

internal class StatusBar
{
    public List<ICondition> statusconditions = new List<ICondition>();
    public bool dead;

    public deathmanager deathmanager = new deathmanager();

    public static int health = 1000;

    public void update()
    {
        int statusTotal = 0;
        foreach (ICondition condition in statusconditions)
        {
            condition.Update();
            statusTotal += condition.Amount;
        }

        if (statusTotal >= health)
        {
            dead = deathmanager.updateDeath();
        }
        else
        {
            deathmanager.resetDeath();
        }
    }

    public void Add(string Type, int amount)
    {
        Debug.Log("adding " + Type);
    }
}
