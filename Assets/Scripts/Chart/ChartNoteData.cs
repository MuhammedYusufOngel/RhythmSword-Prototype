using System;

[System.Serializable]
public class ChartNoteData
{
    public float time; // hit time in seconds
    public string direction = "LEFT"; // "LEFT", "RIGHT", "UP", "DOWN"
    public string type = "Normal"; // extensible note type
    public string color = "RED"; // "RED", "BLUE"
    public float positionX;
    public float positionY;
    public bool hasCustomPosition; // if false, spawner uses random grid positions
}
