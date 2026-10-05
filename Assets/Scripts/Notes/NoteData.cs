using UnityEngine;

public enum DIRECTION
{
    RIGHT,  //0
    LEFT,   //1
    UP,     //2
    DOWN    //3
}

public enum COLOR
{
    RED,
    BLUE
}

public class NoteData
{
    public DIRECTION direction;
    public COLOR color;
    public float speed;
    public int score;

    public NoteData()
    {
        direction = DIRECTION.LEFT;
        color = COLOR.RED;
        speed = 5f;
        score = 0;
    }
}
