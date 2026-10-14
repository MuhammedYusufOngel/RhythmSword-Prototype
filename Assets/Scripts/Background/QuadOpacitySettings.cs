using UnityEngine;

public class QuadOpacitySettings : MonoBehaviour
{
    [SerializeField] private float timeAccordingToSong;
    private float timeChangingRotation;
    private float elapsedTime = 0f;
    void Start()
    {
        timeChangingRotation = 255 / timeAccordingToSong;
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;

        if(elapsedTime > 1f)
        {
            Renderer renderer = GetComponent<Renderer>();
            Color c = renderer.material.color;
            c.a -= timeChangingRotation * Time.deltaTime;
            
            if(c.a > 0)
                renderer.material.color = c;
            elapsedTime = 0f;
        }
    }
}
