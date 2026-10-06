using System;
using System.Collections.Generic;
using UnityEngine;

public class ChartPlayer : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private TextAsset chartFile;
    [SerializeField] private GameObject notePrefab;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Transform target;
    [SerializeField] private float travelTime = 4.5f;

    private ChartData chart;
    private int nextNoteIndex = 0;
    private bool isPlaying = false;

    private List<float> gridX = new List<float> { -1.4f, -0.9f, -0.5f, 0f };
    private List<float> gridY = new List<float> { 0.25f, 1f };

    private void Start()
    {
        if (chartFile == null)
        {
            Debug.LogError("No chart file assigned!");
            return;
        }

        if (spawnPoint == null) spawnPoint = this.transform;

        chart = ChartSerializer.LoadFromTextAsset(chartFile);
        
        if (chart != null)
        {
            chart.SortNotesByTime();
            
            // Optionally load audio clip dynamically if path is provided, but typically assigned in inspector
            // audioSource.clip = Resources.Load<AudioClip>(chart.audioClipPath); // Simplified loading logic
            
            if (audioSource != null)
            {
                audioSource.Play();
                isPlaying = true;
            }
        }
    }

    private void Update()
    {
        if (isPlaying && audioSource != null && audioSource.isPlaying)
        {
            float currentTime = audioSource.time;

            while (nextNoteIndex < chart.notes.Count && currentTime >= chart.notes[nextNoteIndex].time - travelTime)
            {
                SpawnNote(chart.notes[nextNoteIndex]);
                nextNoteIndex++;
            }
        }
    }

    private void SpawnNote(ChartNoteData chartNote)
    {
        float x = 0f;
        float y = 0f;

        if (chartNote.hasCustomPosition)
        {
            x = gridX[Convert.ToInt32(chartNote.positionX)];
            y = 1.0f;
        }
        else
        {
            x = gridX[UnityEngine.Random.Range(0, gridX.Count)];
            y = gridY[UnityEngine.Random.Range(0, gridY.Count)];
        }

        Vector3 spawnPos = spawnPoint.position + new Vector3(x, y, 0f);
        GameObject newNote = Instantiate(notePrefab, spawnPos, Quaternion.identity);

        NoteData noteData = ConvertToNoteData(chartNote);
        
        NoteMovement movement = newNote.GetComponent<NoteMovement>();
        if (movement != null) movement.Initialize(target);
        
        Note noteComponent = newNote.GetComponent<Note>();
        if (noteComponent != null) noteComponent.Initialize(noteData);
    }

    private NoteData ConvertToNoteData(ChartNoteData chartNote)
    {
        DIRECTION dir = DIRECTION.LEFT;
        if (Enum.TryParse(chartNote.direction, true, out DIRECTION parsedDir))
        {
            dir = parsedDir;
        }

        COLOR col = COLOR.RED;
        if (Enum.TryParse(chartNote.color, true, out COLOR parsedCol))
        {
            col = parsedCol;
        }

        return new NoteData(dir, col, chartNote.time);
    }
}
