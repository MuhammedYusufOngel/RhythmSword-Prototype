using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;

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
    }

    void Update()
    {
        
    }
}
