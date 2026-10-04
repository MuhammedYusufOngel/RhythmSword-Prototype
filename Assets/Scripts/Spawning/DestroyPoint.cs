using UnityEngine;

public class DestroyPoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Theres a trigger");
        if (other.CompareTag("Note"))
        {
            Destroy(other.gameObject);
            Debug.Log("Destroyed note.");
        }
    }
}
