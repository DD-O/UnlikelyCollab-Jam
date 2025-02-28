using UnityEngine;

public class JJFollow : MonoBehaviour

{
    private JJFollowTracker followTracker;
    public GameObject perceptionBox;
    public Transform target;
    public Rigidbody2D rigidBody;
    public float distanceToStop;
    public float followSpeed;
    public bool goFollow = false;

    void Start()
    {
        followTracker = GetComponentInParent<JJFollowTracker>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (goFollow == false) {
            perceptionBox.SetActive(false);
            followTracker.followerOrder.Add(this.gameObject);
            goFollow = true;
        }
    }

    private void Update() {
        if (goFollow) {
            transform.position = Vector3.MoveTowards(transform.position, target.position, followSpeed * Time.deltaTime);
        }
    }
}
