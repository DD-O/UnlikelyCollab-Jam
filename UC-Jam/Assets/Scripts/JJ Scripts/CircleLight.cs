using UnityEngine;

public class CircleLight : MonoBehaviour
{
    private LevelChanger levelChanger;
    private SpriteMask perceptionBox;

    void Start()
    {
        levelChanger = GetComponentInParent<LevelChanger>();
        foreach (Transform child in transform)
        child.gameObject.SetActive(false);
    }
    void Update()
    {
        if (levelChanger.circleChange) {
            foreach (Transform child in transform)
            child.gameObject.SetActive(true);
        }
    }
}
