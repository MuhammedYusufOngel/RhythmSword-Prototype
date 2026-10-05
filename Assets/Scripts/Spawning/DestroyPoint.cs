using UnityEngine;

public class DestroyPoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Note"))
        {
            float elapsedTime = other.gameObject.GetComponent<Note>().GetElapsedTime();
            Debug.Log("Yaþam süresi: " + elapsedTime);
            Destroy(other.gameObject);
        }
    }
}
