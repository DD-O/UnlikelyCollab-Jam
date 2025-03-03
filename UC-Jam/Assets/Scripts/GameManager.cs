using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    // Example variables that ObjectToggler can check
    public static bool circleColorArea = false;
    public static bool triangleColorArea = false;
    public static bool heartColorArea = false;
    public static bool starColorArea = false;
    public static bool squareColorArea = false;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Keep the manager between scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
