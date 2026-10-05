using UnityEngine;
using TMPro;

public class Note : MonoBehaviour
{
    NoteData noteData;
    [SerializeField] private TMP_Text dir;
    float elapsedTime = 0f;

    public void Initialize(NoteData noteData)
    {
        this.noteData = noteData;
        dir.text = this.noteData.direction.ToString();

        if (this.noteData.color == COLOR.BLUE)
            GetComponent<MeshRenderer>().material.color = Color.blue;
        else if (this.noteData.color == COLOR.RED)
            GetComponent<MeshRenderer>().material.color = Color.red;
    }

    void Start()
    {
        //noteData = new NoteData();

        //int color = Random.Range(0, 2);

        //if (color == 0)
        //{
        //    GetComponent<MeshRenderer>().material.color = Color.blue;
        //    noteData.color = COLOR.BLUE;

        //    int direction = Random.Range(0, 4);
        //    noteData.direction = (DIRECTION)direction;
        //}
        //else if (color == 1)
        //{
        //    GetComponent<MeshRenderer>().material.color = Color.red;
        //    noteData.color = COLOR.RED;

        //    int direction = Random.Range(0, 4);
        //    noteData.direction = (DIRECTION)direction;
        //}
        //noteData.score = 10;

        //dir.text = noteData.direction.ToString();
    }

    public COLOR GetColor()
    {
        return noteData.color;
    }

    public DIRECTION GetDirection()
    {
        return noteData.direction;
    }
    
    public int GetScore()
    {
        return noteData.score;
    }

    public float GetElapsedTime()
    {
        return elapsedTime;
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;
    }
}
