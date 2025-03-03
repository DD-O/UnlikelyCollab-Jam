using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroupBox : MonoBehaviour
{
    public GameObject Player;
    public List<GameObject> followerGroup;
    public int padding;
    
    void Start()
    {
        padding = 0;
        followerGroup = new List<GameObject>();
        followerGroup.Add(Player); // Add Square Guy as Leader!
    }

    void Update()
    {
        var bound = new Bounds(followerGroup[0].transform.position, Vector3.zero);
        for (int i = 1; i < followerGroup.Count; i++)
        {
            bound.Encapsulate(followerGroup[i].transform.position);
        }
        transform.position = bound.center;
        transform.localScale = bound.size + new Vector3(padding, padding, 1);
    }
}
