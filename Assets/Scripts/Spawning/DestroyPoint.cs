using UnityEngine;

public class DestroyPoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Note"))
        {
            Destroy(other.gameObject);
        }
    }
}
