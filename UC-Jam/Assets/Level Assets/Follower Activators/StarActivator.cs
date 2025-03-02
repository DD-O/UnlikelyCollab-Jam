using UnityEngine;

public class StarActivator : MonoBehaviour
{
    private LevelChanger levelChanger;
    private bool isChanged = false;
    private MovingPlatform movingPlatform;

    void Start()
    {
        levelChanger = GetComponentInParent<LevelChanger>();
        movingPlatform = GetComponentInChildren<MovingPlatform>();
        movingPlatform.enabled = false;
    }
    void Update()
    {
        if (isChanged == false && levelChanger.starChange == true) {
            movingPlatform.enabled = true;
            isChanged = true;
        }
    }
}
