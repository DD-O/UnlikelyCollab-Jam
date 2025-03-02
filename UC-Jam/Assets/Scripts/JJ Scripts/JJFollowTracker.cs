using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JJFollowTracker : MonoBehaviour
{
    public GameObject Player;
    public List<GameObject> followerOrder;
    public int followerAmount;
    
    void Start()
    {
        followerOrder = new List<GameObject>();
        followerOrder.Add(Player); // Add Square Guy as Leader!
    }
}
