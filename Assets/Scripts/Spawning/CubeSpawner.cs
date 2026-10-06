using System.Collections.Generic;
using System;
using UnityEngine;

public class CubeSpawner : MonoBehaviour
{
    [SerializeField] GameObject note;
    [SerializeField] Transform target;
    [SerializeField] TextAsset jsonFile;

    string text;
    double elapsedTime;
    int counter = 0;
    float randomX, randomY;
    private List<float> gridX = new List<float>(){ -1.4f, -1f, -0.4f, 0f };
    private List<float> gridY = new List<float>(){ 0.25f, 1f };
    private List<ChartNoteData> notes = new List<ChartNoteData>();

    private void ReadFile()
    {
        //if (jsonFile == null)
        //{
        //    Debug.LogWarning("Dosya bulunamadý: jsonFile.txt");
        //    return;
        //}

        //ChartData chartData = JsonUtility.FromJson<ChartData>(jsonFile.text);

        //foreach (var note in chartData.notes)
        //{
        //    notes.add(note);
        //}
    }

    void Start()
    {
        elapsedTime = 0f;
        randomX = 0f;
        randomY = 0f;
        ReadFile();
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;
        
        if(elapsedTime > notes[counter].time - 4)
        {
            randomX = gridX[Convert.ToInt32(notes[counter].positionX)];
            randomY = gridY[1];

            Vector3 randomVector = new Vector3(randomX, 1f, 0f);

            GameObject newNote = Instantiate(note, transform.position + randomVector, transform.rotation, transform);
            newNote.GetComponent<NoteMovement>().Initialize(target);
            //newNote.GetComponent<Note>().Initialize(notes[counter]);

            if (counter < notes.Count - 1)
                counter++;
            else
            {
                counter = 0;
                elapsedTime = 0;
            }

            Debug.Log("Nota üretildi::Sure: " + elapsedTime);
        }
    }
}
