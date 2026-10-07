using UnityEngine;
using TMPro;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;

    [SerializeField] TMP_Text left;
    [SerializeField] TMP_Text right;
    [SerializeField] TMP_Text debuglog;

    [SerializeField] AudioSource hitSource;
    [SerializeField] AudioClip hitSound;
    [SerializeField] private GameObject hitEffectPrefab;

    public int sumScore = 0;

    void Start()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(instance);
    }

    public void AddScore(int score, Transform hitPoint)
    {
        sumScore += score;
        left.text = sumScore.ToString();

        //int sampleRate = hitSource.clip.frequency;
        //hitSource.pitch = Random.Range(0.9f, 1.2f);
        //hitSource.timeSamples = Mathf.FloorToInt(0.1f * sampleRate);
        //hitSource.Play();

        Instantiate(hitEffectPrefab, hitPoint.position, Quaternion.identity);
    }

    public void DebugLog(string msg)
    {
        right.text = msg;
    }

    public void DebugLogLong(string msg)
    {
        debuglog.text = msg;
    }

    void Update()
    {
        
    }
}
