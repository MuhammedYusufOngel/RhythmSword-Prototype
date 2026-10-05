using System.Collections.Generic;
using System;
using UnityEngine;

public class CubeSpawner : MonoBehaviour
{
    [SerializeField] GameObject note;
    [SerializeField] Transform target;
    [SerializeField] TextAsset textFile;

    string text;
    double elapsedTime;
    int counter = 0;
    float randomX, randomY;
    private List<float> gridX = new List<float>(){ -1.4f, -1f, -0.4f, 0f };
    private List<float> gridY = new List<float>(){ 0.25f, 1f };
    private List<NoteData> notes = new List<NoteData>();

    private void ReadFile()
    {
        if (textFile == null)
        {
            Debug.LogWarning("Dosya bulunamadý: myfile.txt");
            return;
        }

        string[] lines = textFile.text.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);

        for (int i = 0; i < lines.Length; i++)
        {
            text = lines[i];
            string[] parts = text.Split(new[] { "\t" }, StringSplitOptions.None);

            if (parts[0] != "")
            {
                NoteData noteData = new NoteData();
                noteData.hitTime = Convert.ToDouble(parts[0]);
                noteData.direction = (DIRECTION)Enum.Parse(typeof(DIRECTION), parts[1], true);
                noteData.color = (COLOR)Enum.Parse(typeof(COLOR), parts[2], true);

                notes.Add(noteData);

                Debug.Log(noteData.hitTime);
            }
        }
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
        
        if(elapsedTime > notes[counter].hitTime - 4)
        {
            randomX = gridX[UnityEngine.Random.Range(0, 4)];
            randomY = gridY[UnityEngine.Random.Range(0, 2)];

            Vector3 randomVector = new Vector3(randomX, 1f, 0f);

            GameObject newNote = Instantiate(note, transform.position + randomVector, transform.rotation, transform);
            newNote.GetComponent<NoteMovement>().Initialize(target);
            newNote.GetComponent<Note>().Initialize(notes[counter]);

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
