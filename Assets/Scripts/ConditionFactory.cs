using System;
using System.Collections.Generic;
using UnityEngine;

internal static class ConditionFactory
{
    static ICondition AddCondition<T>(Sprite sprite, Color color, string AcceptedTag) where T : ICondition, new()
    {
        ICondition condition = new T(); 
        condition.Icon = sprite;
        condition.Color = color;
        condition.AcceptedTag = AcceptedTag;
        return condition;
    }
}