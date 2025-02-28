using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JJFollowTracker : MonoBehaviour
{
    public GameObject Player;
    public List<GameObject> followerOrder;
    public int followerAmount;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        followerOrder = new List<GameObject>();
        followerOrder.Add(Player); // Add Square Guy as Leader!
        followerAmount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        followerAmount = followerOrder.Count;
    }
}
