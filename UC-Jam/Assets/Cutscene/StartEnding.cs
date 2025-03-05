using UnityEngine;

public class StartEnding : MonoBehaviour
{
    Animator animator;
    public float animationDelay = 4.2f;
    public Transform endBox;

    void Start()
    {
        //endBox = GetComponentInChildren<Transform>();
        animator = GetComponent<Animator>();
        Invoke("PlayEndAnimation", animationDelay);
    }

    void PlayEndAnimation()
    {
        animator.enabled = true;
    }

    void Update()
    {
        if (animator.enabled == true)
        {
            endBox.localScale += new Vector3(6*Time.deltaTime, 6*Time.deltaTime, 0);
        }
    }
}
