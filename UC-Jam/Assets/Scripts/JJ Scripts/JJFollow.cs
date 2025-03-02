using System.Text.RegularExpressions;
using Unity.VisualScripting;
using UnityEngine;

public class JJFollow : MonoBehaviour

{
    private JJFollowTracker followTracker;
    public GroupBoxSizer groupBoxSizer; 
    public GameObject perceptionBox;
    public Transform target;
    public float followSpeed;
    public bool goFollow = false;

    void Start()
    {
        followTracker = GetComponentInParent<JJFollowTracker>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player" && goFollow == false) {
            if (groupBoxSizer.Padding == 0) {
                groupBoxSizer.Padding = 5;
            }
            perceptionBox.SetActive(false);
            followTracker.followerOrder.Add(this.gameObject);
            print(other);
            goFollow = true;
        }
    }

    private void Update() {
        if (goFollow) {
            transform.position = Vector3.MoveTowards(transform.position, target.position, followSpeed * Time.deltaTime);
        }
    }
}
