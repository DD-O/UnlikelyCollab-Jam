using UnityEngine;

public class BushActivator : MonoBehaviour
{
    private void Awake()
    {
        transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(1).gameObject.SetActive(false);
    }

    private void Start()
    {
        UpdateObjects();
    }

    private void Update()
    {
        UpdateObjects();
    }

    private void UpdateObjects()
    {
        if (transform.childCount < 2)
            return;

        bool state = GameManager.heartColorArea;

        //transform.GetChild(0).gameObject.SetActive(!state);
        transform.GetChild(1).gameObject.SetActive(state);
    }
}
