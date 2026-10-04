using UnityEngine;

public class NoteMovement : MonoBehaviour
{
    [SerializeField] float speed;

    private Transform target;
    public void Initialize(Transform target)
    {
        this.target = target;
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
    }
}
