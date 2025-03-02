using UnityEngine;

public class TriangleActivator : MonoBehaviour
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
        if (isChanged == false && levelChanger.triangleChange == true) {
            foreach (Transform child in transform)
            child.gameObject.SetActive(true);
            isChanged = true;
        }
    }
}
