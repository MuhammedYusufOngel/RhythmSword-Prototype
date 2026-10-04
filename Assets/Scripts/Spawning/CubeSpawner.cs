using System.Linq.Expressions;
using UnityEngine;

public class CubeSpawner : MonoBehaviour
{
    [SerializeField] GameObject note;
    [SerializeField] Transform target;
    float elapsedTime;
    float randomX, randomY;

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
            randomX = Random.Range(-3f, 4f);
            randomY = Random.Range(0f, 1f);

            Vector3 randomVector = new Vector3(randomX, randomY, 0f);

            GameObject newNote = Instantiate(note, transform.position + randomVector, transform.rotation, transform);
            newNote.GetComponent<NoteMovement>().Initialize(target);
        }
    }
}
