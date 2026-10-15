using UnityEngine;

public class MissPoint : MonoBehaviour
{
    [SerializeField] private AudioSource missSource;
    [SerializeField] private AudioClip missClip;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Note"))
        {
            Debug.Log("Miss");

            missSource.PlayOneShot(missClip);
        }
    }
}
