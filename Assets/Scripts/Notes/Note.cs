using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Note : MonoBehaviour
{
    NoteData noteData;
    [SerializeField] private TMP_Text dir;
    [SerializeField] private RawImage dir_r;
    [SerializeField] private RawImage dir_l;
    [SerializeField] private RawImage dir_u;
    [SerializeField] private RawImage dir_d;

    [SerializeField] private Material red;
    [SerializeField] private Material blue;
    float elapsedTime = 0f;

    public void Initialize(NoteData noteData)
    {
        this.noteData = noteData;
        dir.text = this.noteData.direction.ToString();

        if(this.noteData.direction == DIRECTION.RIGHT)
        {
            dir_l.gameObject.SetActive(false);
            dir_r.gameObject.SetActive(true);
            dir_u.gameObject.SetActive(false);
            dir_d.gameObject.SetActive(false);
        }
        else if (this.noteData.direction == DIRECTION.LEFT)
        {
            dir_l.gameObject.SetActive(true);
            dir_r.gameObject.SetActive(false);
            dir_u.gameObject.SetActive(false);
            dir_d.gameObject.SetActive(false);
        }
        else if (this.noteData.direction == DIRECTION.UP)
        {
            dir_l.gameObject.SetActive(false);
            dir_r.gameObject.SetActive(false);
            dir_u.gameObject.SetActive(true);
            dir_d.gameObject.SetActive(false);
        }
        else if (this.noteData.direction == DIRECTION.DOWN)
        {
            dir_l.gameObject.SetActive(false);
            dir_r.gameObject.SetActive(false);
            dir_u.gameObject.SetActive(false);
            dir_d.gameObject.SetActive(true);
        }

        if (this.noteData.color == COLOR.BLUE)
            GetComponent<MeshRenderer>().material = blue;
        else if (this.noteData.color == COLOR.RED)
            GetComponent<MeshRenderer>().material = red;
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
