using System.Collections.Generic;
using UnityEngine;

public class CubeSpawner : MonoBehaviour
{
    [SerializeField] GameObject note;
    [SerializeField] Transform target;
    float elapsedTime;
    float randomX, randomY;
    private List<float> gridX = new List<float>(){ -1.1f, -0.3f, 0.3f, 1.1f };
    private List<float> gridY = new List<float>(){ 0.25f, 1f };

    void Start()
    {
        elapsedTime = 0f;
        randomX = 0f;
        randomY = 0f;
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;

        if(elapsedTime > 1.0f)
        {
            elapsedTime = 0f;
            randomX = gridX[Random.Range(0, 4)];
            randomY = gridY[Random.Range(0, 2)];

            Vector3 randomVector = new Vector3(randomX, randomY, 0f);

            GameObject newNote = Instantiate(note, transform.position + randomVector, transform.rotation, transform);
            newNote.GetComponent<NoteMovement>().Initialize(target);
        }
    }
}
