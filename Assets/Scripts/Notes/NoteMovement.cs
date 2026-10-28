using UnityEngine;

public class NoteMovement : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "SlowDown")
        {
            speed = 2.5f;
        }
    }
    [SerializeField] float speed;
    private Transform target;
    public void Initialize(Transform target)
    {
        this.target = target;
    }

    void Update()
    {
        transform.position += new Vector3(0f, 0f, -speed * Time.deltaTime);
    }
}
