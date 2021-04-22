using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FixedSizeMetalList
{
    private List<bool> items = new List<bool>();

    private int limit { get; }
    private bool isMetal = false;

    public FixedSizeMetalList(int listLimit)
    {
        limit = listLimit;
    }

    public void Add(bool metalFrame)
    {
        items.Add(metalFrame);

        while (items.Count > limit)
        {
            items.RemoveAt(0);
        }

        isMetal = CheckIfMetal();
    }

    public bool IsMetalList()
    {
        return isMetal;
    }

    private bool CheckIfMetal()
    {
        if (items.Count < limit)
        {
            return false;
        }

        var allFalse = items.Where(b => b == false).Count();
        var allTrue = items.Where(b => b == true).Count();

        return allTrue > allFalse;
    }
}
