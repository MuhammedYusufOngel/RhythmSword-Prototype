using UnityEngine;

public class SunRotation : MonoBehaviour
{
    [SerializeField] private float timeAccordingToSong;
    private float timeChangingRotation;

    void Start()
    {
        timeChangingRotation = 90f / timeAccordingToSong;
    }

    void Update()
    {
        transform.Rotate(new Vector3(timeChangingRotation * Time.deltaTime, 0f, 0f));
    }
}
