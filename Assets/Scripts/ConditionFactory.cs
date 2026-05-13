using System;
using System.Collections.Generic;
using UnityEngine;

internal static class ConditionFactory
{
    static ICondition AddCondition<T>(int amount, Sprite sprite) where T : ICondition, new()
    {
        ICondition condition = new T();
        condition.Add(amount); 
        condition.Icon = sprite;
        return condition;
    }
}