using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class GroupBoxSizer : MonoBehaviour
{
    private JJFollowTracker followTracker;
    public int Padding;

    void Start()
    {
        Padding = 0;
        followTracker = GetComponentInParent<JJFollowTracker>();
    }
    public void PointBetweenPoints()
    {
        var bound = new Bounds(followTracker.followerOrder[0].transform.position, Vector3.zero);
        for (int i = 1; i < followTracker.followerOrder.Count; i++)
        {
            bound.Encapsulate(followTracker.followerOrder[i].transform.position);
        }
        transform.position = bound.center;
        transform.localScale = bound.size + new Vector3(Padding, Padding, 1);
    }

    void Update()
    {
        PointBetweenPoints();
    }
}
