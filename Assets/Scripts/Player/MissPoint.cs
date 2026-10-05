using UnityEngine;

public class MissPoint : MonoBehaviour
{    

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Note"))
        {
            Debug.Log("Miss");
        }
    }
}
