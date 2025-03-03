using UnityEngine;

public class SpeakerActivation : MonoBehaviour
{

    private void Awake()
    {
        transform.GetChild(0).gameObject.SetActive(true);
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

        bool state = GameManager.starColorArea;

        transform.GetChild(0).gameObject.SetActive(!state);
        transform.GetChild(1).gameObject.SetActive(state);
    }
}
