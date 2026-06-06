using UnityEngine;

public interface ICondition
{
    //the amount currently in the condition
    int total { get; }

    //how fast the condition deteriorates. leave at 0 to stay permanent
    int deterioratingSpeed { get; set; }

    //the tag that identifies this condition
    string acceptedTag { get; set; }

    //the visual for the condition
    GameObject statusVisual {  get; set; }

    //the condition that this condition influences
    ICondition influence {  get; set; }

    //update this condition
    //returns: total amount changed
    int Update();

    //remove a specific amount from this condition
    //returns: how much has been succesfully removed
    int Remove(int amount);

    //add to the condition
    //returns: if addition was succesful
    int Add(int amount, string tag);
}

//base implementation of ICondition
public class Condition : ICondition
{
    public int total { get; protected set; }
    public int deterioratingSpeed { get; set; }

    public string acceptedTag { get; set;}

    public ICondition influence { get; set; }

    private RectTransform statusRect;
    private GameObject statusvisual;
    public GameObject statusVisual
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

    //add to the condition
    //returns: if addition was succesful
    public virtual int Add(int amount, string tag)
    {
        if (acceptedTag == tag) 
        {
            if (influence != null) 
            {
                int influenced = influence.Remove(amount);
                total += (amount - influenced);
                amount = (amount - influenced) - influenced;
            }
            else
            {
                total += amount;
            }

            if (total != 0)
            {
                statusVisual.SetActive(true);
            }

            //update condition size
            statusRect.sizeDelta = new Vector2(StatusBar.tickSize * total, statusRect.sizeDelta.y);

            return amount;
        }
        return -1;
    }

    //remove a specific amount from this condition
    //returns: how much has been succesfully removed
    public virtual int Remove(int amount)
    {
        total -= amount;

        if (total <= 0)
        {
            int rest = -total;
            total = 0;
            statusVisual.SetActive(false);
            return amount - rest;
        }

        //update condition size
        statusRect.sizeDelta = new Vector2(StatusBar.tickSize * total, statusRect.sizeDelta.y);
        return amount;
    }

    //update this condition
    //returns: total amount changed
    public virtual int Update()
    {
        if (deterioratingSpeed == 0 || total <= 0)
        {
            return 0;
        }

        int deterioration = Mathf.RoundToInt(deterioratingSpeed);

        Remove(deterioration);
        return -deterioration;
    }
}