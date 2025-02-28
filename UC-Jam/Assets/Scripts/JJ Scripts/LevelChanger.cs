using UnityEngine;

public class LevelChanger : MonoBehaviour
{
    public JJFollowTracker followTracker;
    public bool circleChange;
    public bool triangleChange;
    public bool heartChange;
    public bool starChange;
    // public bool squareChange; // Might not need! Only changes in final cutscene.
    
    // Update is called once per frame
    void Update()
    {
        for (int i = 1; i < followTracker.followerOrder.Count; i++)
        {
            var loopFollower = followTracker.followerOrder[i];
            if (loopFollower.name == "Circle") {
                circleChange = true;
            }
            if (loopFollower.name == "Triangle") {
                triangleChange = true;
            }
            if (loopFollower.name == "Heart") {
                heartChange = true;
            }
            if (loopFollower.name == "Star") {
                starChange = true;
            }
            // Might not need! Only changes in final cutscene.
            // if (loopFollower.name == "Player") {
            //     squareChange = true;
            // }
        }
    }
}
