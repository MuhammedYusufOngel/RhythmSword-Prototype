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
    public double hitTime;
    public int score;

    public NoteData()
    {
        direction = DIRECTION.LEFT;
        color = COLOR.RED;
        hitTime = 0;
        score = 0;
    }

    public NoteData(DIRECTION direction, COLOR color, float hitTime)
    {
        this.direction = direction;
        this.color = color;
        this.hitTime = hitTime;
        score = 10;
    }
}
