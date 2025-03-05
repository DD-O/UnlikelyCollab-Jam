using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartIntro : MonoBehaviour
{
    Animator animator;
    Rigidbody2D rb;
    private bool switchScene = false;
    public Object levelScene;
    public float sceneDelay = 0.5f;
    public Image mainMenuImage;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        mainMenuImage = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if(animator.enabled == false && Input.GetKeyDown(KeyCode.Space)) {
            rb.AddForce(Vector2.up * 5, ForceMode2D.Impulse);
            animator.enabled = true;
            switchScene = true;
        }
        if (switchScene) {
            //mainMenuImage.color = new Color32(255, 255, 255, 50);
            Invoke("SceneSwitch", sceneDelay);
        }
    }

    void SceneSwitch()
    {
        SceneManager.LoadScene(levelScene.name);
    }
}
