using UnityEngine;

public class Circle : MonoBehaviour
{
    private LevelChanger levelChanger;
    private bool isChanged = false;

    void Start()
    {
        levelChanger = GetComponentInParent<LevelChanger>();
        foreach (Transform child in transform)
        child.gameObject.SetActive(false);
    }
    void Update()
    {
        if (isChanged == false && levelChanger.circleChange == true) {
            foreach (Transform child in transform)
            child.gameObject.SetActive(true);
            isChanged = true;
        }
    }
}
