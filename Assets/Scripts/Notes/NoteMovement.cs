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
        transform.position += new Vector3(0f, 0f, -speed * Time.deltaTime);
    }
}
