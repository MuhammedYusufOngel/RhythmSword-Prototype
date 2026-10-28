using UnityEngine;

public class AtmosphereManager : MonoBehaviour
{
    private float elapsedTime = 0f;
    private int counter = 0;
    private bool isParticleDone = false;
    private bool isWhiteDotsDone = false;
    private float timeChangingRotation;

    [SerializeField] private ParticleSystem bloodRainEffect;
    [SerializeField] private ParticleSystem whiteDotsEffect;
    [SerializeField] private Transform sun;
    void Start()
    {
        timeChangingRotation = 30 / 60f;
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;

        if (elapsedTime > counter)
        {
            counter++;
            Debug.Log(counter);
        }

        sun.Rotate(new Vector3(timeChangingRotation * Time.deltaTime, 0f, 0f));

        if(elapsedTime > 205f && !isWhiteDotsDone)
        {
            whiteDotsEffect.Play();
            isWhiteDotsDone = true;
        }

        else if(elapsedTime > 205f)
        {
            timeChangingRotation = 0f;
            sun.eulerAngles = Vector3.right * -120f;
        }
        
        else if(elapsedTime > 176f)
        {
            timeChangingRotation = 50f;
        }

        else if (elapsedTime > 165f && !isParticleDone)
        {
            bloodRainEffect.Play();
            isParticleDone = true;
        }

        else if (elapsedTime > 144f && isWhiteDotsDone)
        {
            whiteDotsEffect.Stop();
        }

        else if(elapsedTime > 126f && !isWhiteDotsDone)
        {
            whiteDotsEffect.Play();
            isWhiteDotsDone = true;
        }

        else if(elapsedTime > 126f)
        {
            timeChangingRotation = 0f;
            sun.eulerAngles = Vector3.right * -120f;
        }

        else if (elapsedTime > 112f)
            timeChangingRotation = 50f;

        else if (elapsedTime > 98f)
        {
            timeChangingRotation = 0f;
            sun.eulerAngles = Vector3.right * -120f;
        }

        else if (elapsedTime > 85f)
            timeChangingRotation = -50f;

        else if (elapsedTime > 68f)
            timeChangingRotation = 10f;

        else if (elapsedTime > 61f)
            timeChangingRotation = 5f;

        else if (elapsedTime > 59f)
            timeChangingRotation = -30f;

        else if (elapsedTime > 56f)
            timeChangingRotation = 15f;

    }
}
