using UnityEngine;
using TMPro;

public class Note : MonoBehaviour
{
    NoteData noteData;
    [SerializeField] private TMP_Text dir;

    void Start()
    {
        noteData = new NoteData();
        if(transform.position.x > 0f)
        {
            GetComponent<MeshRenderer>().material.color = Color.blue;
            noteData.color = COLOR.BLUE;

            int direction = Random.Range(2,4);
            noteData.direction = (DIRECTION)direction;
        }
        else
        {
            GetComponent<MeshRenderer>().material.color = Color.red;
            noteData.color = COLOR.RED;

            int direction = Random.Range(0, 2);
            noteData.direction = (DIRECTION)direction;
        }
        noteData.speed = 5f;
        noteData.score = 10;

        dir.text = noteData.direction.ToString();
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
}
