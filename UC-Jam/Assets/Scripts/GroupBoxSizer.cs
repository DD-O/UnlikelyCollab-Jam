using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class GroupBoxSizer : MonoBehaviour
{
    public List<Transform> points;
    public int Padding;

    public void PointBetweenPoints()
    {
        var bound = new Bounds(points[0].position, Vector3.zero);
        for (int i = 1; i < points.Count; i++)
        {
            bound.Encapsulate(points[i].position);
        }
        transform.position = bound.center;
        transform.localScale = bound.size + new Vector3(Padding, Padding, 1);
    }

    void Update()
    {
        PointBetweenPoints();
    }
}
