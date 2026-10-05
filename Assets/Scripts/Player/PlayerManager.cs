using UnityEngine;
using TMPro;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;

    [SerializeField] TMP_Text left;
    [SerializeField] TMP_Text right;
    [SerializeField] TMP_Text debuglog;
    public int sumScore = 0;

    void Start()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(instance);
    }

    public void AddScore(int score)
    {
        sumScore += score;
        left.text = sumScore.ToString();
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
