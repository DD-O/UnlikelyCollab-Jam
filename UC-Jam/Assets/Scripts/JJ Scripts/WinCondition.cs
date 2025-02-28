using UnityEngine;

public class WinCondition : MonoBehaviour
{
    private LevelChanger levelChanger;
    public bool almostWin = false;
    public bool gameWon = false;
    public GameObject voidObject;
    public GameObject cameraObject;

    void Start()
    {
        levelChanger = GetComponentInParent<LevelChanger>();
        foreach (Transform child in transform)
        child.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (levelChanger.circleChange && levelChanger.triangleChange && levelChanger.heartChange && levelChanger.starChange) {
            if (almostWin == false) {
            foreach (Transform child in transform)
            child.gameObject.SetActive(true);
                almostWin = true;
            }
        }
        
    }
    
    private void OnTriggerEnter2D(Collider2D other) {
        if (almostWin == true) {
            gameWon = true;
            voidObject.SetActive(false);
            cameraObject.GetComponent<Camera>().backgroundColor = Color.cyan;
        }
    }
}