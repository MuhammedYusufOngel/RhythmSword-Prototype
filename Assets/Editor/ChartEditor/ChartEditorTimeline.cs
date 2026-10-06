using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using RhythmEditor;

public static class ChartEditorTimeline
{
    private const float HEADER_HEIGHT = 20f;
    private const float NOTE_WIDTH = 20f;
    private const float NOTE_PADDING = 2f;
    private const float LANE_LABEL_WIDTH = 40f;
    
    private static bool isDragging = false;
    private static bool isScrolling = false;
    private static int dragNoteIndex = -1;
    
    private static readonly DIRECTION[] laneDirections = { DIRECTION.LEFT, DIRECTION.RIGHT, DIRECTION.UP, DIRECTION.DOWN };
    private static readonly string[] laneLabels = { "← L", "→ R", "↑ U", "↓ D" };
    private static readonly Color[] laneColors = { 
        new Color(0.15f, 0.15f, 0.15f),
        new Color(0.18f, 0.18f, 0.18f),
        new Color(0.15f, 0.15f, 0.15f),
        new Color(0.18f, 0.18f, 0.18f)
    };
    
    public static void Draw(ChartEditorState state, Rect rect, EditorWindow window)
    {
        Event e = Event.current;
        HandleInput(state, rect, window, e);

        // Background
        EditorGUI.DrawRect(rect, new Color(0.1f, 0.1f, 0.1f));

        float timelineWidth = rect.width - LANE_LABEL_WIDTH;
        float laneHeight = (rect.height - HEADER_HEIGHT) / 4f;

        // Clip timeline area
        Rect timelineArea = new Rect(rect.x + LANE_LABEL_WIDTH, rect.y + HEADER_HEIGHT, timelineWidth, rect.height - HEADER_HEIGHT);
        
        // Draw Lanes and Lane Labels
        for (int i = 0; i < 4; i++)
        {
            Rect laneLabelRect = new Rect(rect.x, rect.y + HEADER_HEIGHT + i * laneHeight, LANE_LABEL_WIDTH, laneHeight);
            EditorGUI.DrawRect(laneLabelRect, new Color(0.2f, 0.2f, 0.2f));
            GUI.Label(laneLabelRect, laneLabels[i], EditorStyles.whiteLabel);

            Rect laneRect = new Rect(timelineArea.x, timelineArea.y + i * laneHeight, timelineArea.width, laneHeight);
            EditorGUI.DrawRect(laneRect, laneColors[i]);
            
            // Lane Separator
            if (i > 0)
            {
                Handles.color = new Color(0.3f, 0.3f, 0.3f);
                float yPos = laneRect.y;
                Handles.DrawLine(new Vector3(rect.x, yPos, 0), new Vector3(rect.x + rect.width, yPos, 0));
            }
        }

        GUI.BeginClip(timelineArea);
        Rect clippedArea = new Rect(0, 0, timelineArea.width, timelineArea.height);

        // Time Grid (Seconds)
        float visibleTimeStart = state.scrollOffset;
        float visibleTimeEnd = visibleTimeStart + timelineArea.width / state.zoomLevel;

        Handles.color = new Color(0.4f, 0.4f, 0.4f);
        int startSec = Mathf.FloorToInt(visibleTimeStart);
        int endSec = Mathf.CeilToInt(visibleTimeEnd);
        for (int s = startSec; s <= endSec; s++)
        {
            float xPos = (s - state.scrollOffset) * state.zoomLevel;
            Handles.DrawLine(new Vector3(xPos, 0, 0), new Vector3(xPos, clippedArea.height, 0));
        }

        // BPM Grid
        if (state.chart.bpm > 0)
        {
            float beatInterval = 60f / state.chart.bpm;
            float subdivInterval = SnapUtility.GetSnapInterval(state.chart.bpm, state.snapMode);
            if (subdivInterval <= 0) subdivInterval = beatInterval;

            int startBeat = Mathf.FloorToInt(visibleTimeStart / subdivInterval);
            int endBeat = Mathf.CeilToInt(visibleTimeEnd / subdivInterval);

            for (int b = startBeat; b <= endBeat; b++)
            {
                float t = b * subdivInterval;
                float xPos = (t - state.scrollOffset) * state.zoomLevel;
                
                if (Mathf.Abs(t % beatInterval) < 0.001f)
                    Handles.color = new Color(1f, 1f, 1f, 0.3f); // Beat
                else
                    Handles.color = new Color(1f, 1f, 1f, 0.1f); // Sub
                
                Handles.DrawLine(new Vector3(xPos, 0, 0), new Vector3(xPos, clippedArea.height, 0));
            }
        }

        // Draw Notes
        for (int i = 0; i < state.chart.notes.Count; i++)
        {
            var note = state.chart.notes[i];
            if (note.time < visibleTimeStart - 2f || note.time > visibleTimeEnd + 2f) continue;

            Rect noteRect = GetNoteRectLocal(note, state, laneHeight);
            
            Color noteColor = note.color == "RED" ? new Color(0.8f, 0.2f, 0.2f) : new Color(0.2f, 0.4f, 0.8f);
            EditorGUI.DrawRect(noteRect, noteColor);
            
            // Highlight if selected
            if (i == state.selectedNoteIndex)
            {
                Handles.DrawSolidRectangleWithOutline(noteRect, Color.clear, Color.yellow);
            }
            
            // Direction text
            GUIStyle centeredStyle = new GUIStyle(EditorStyles.boldLabel) { alignment = TextAnchor.MiddleCenter, normal = { textColor = Color.white } };
            string dirChar = "•";
            if (note.direction == "LEFT") dirChar = "←";
            else if (note.direction == "RIGHT") dirChar = "→";
            else if (note.direction == "UP") dirChar = "↑";
            else if (note.direction == "DOWN") dirChar = "↓";
            
            GUI.Label(noteRect, dirChar, centeredStyle);
        }

        // Playback Cursor
        float cursorPos = (state.currentTime - state.scrollOffset) * state.zoomLevel;
        if (cursorPos >= 0 && cursorPos <= clippedArea.width)
        {
            Handles.color = Color.red;
            Handles.DrawLine(new Vector3(cursorPos, 0, 0), new Vector3(cursorPos, clippedArea.height, 0));
        }

        GUI.EndClip();

        // Header Labels (Seconds)
        GUI.BeginClip(new Rect(rect.x + LANE_LABEL_WIDTH, rect.y, timelineWidth, HEADER_HEIGHT));
        for (int s = startSec; s <= endSec; s++)
        {
            float xPos = (s - state.scrollOffset) * state.zoomLevel;
            GUI.Label(new Rect(xPos - 20, 0, 40, HEADER_HEIGHT), s.ToString() + "s", EditorStyles.whiteMiniLabel);
        }
        GUI.EndClip();
    }

    private static void HandleInput(ChartEditorState state, Rect rect, EditorWindow window, Event e)
    {
        float timelineWidth = rect.width - LANE_LABEL_WIDTH;
        float laneHeight = (rect.height - HEADER_HEIGHT) / 4f;
        Rect timelineArea = new Rect(rect.x + LANE_LABEL_WIDTH, rect.y + HEADER_HEIGHT, timelineWidth, rect.height - HEADER_HEIGHT);
        Rect fullTimelineArea = new Rect(rect.x + LANE_LABEL_WIDTH, rect.y, timelineWidth, rect.height);

        if (e.type == EventType.ScrollWheel && fullTimelineArea.Contains(e.mousePosition))
        {
            if (e.control)
            {
                // Zoom
                float oldTime = XToTime(e.mousePosition.x, state, timelineArea);
                state.zoomLevel *= (e.delta.y > 0 ? 0.9f : 1.1f);
                state.zoomLevel = Mathf.Clamp(state.zoomLevel, 10f, 1000f);
                float newX = (oldTime - state.scrollOffset) * state.zoomLevel;
                state.scrollOffset += (newX - (e.mousePosition.x - timelineArea.x)) / state.zoomLevel;
            }
            else
            {
                // Horizontal scroll
                state.scrollOffset += e.delta.y * 10f / state.zoomLevel;
            }
            state.scrollOffset = Mathf.Max(0, state.scrollOffset);
            e.Use();
            window.Repaint();
        }
        
        if (e.type == EventType.MouseDown && e.button == 2 && fullTimelineArea.Contains(e.mousePosition))
        {
            isScrolling = true;
            e.Use();
        }
        if (e.type == EventType.MouseDrag && isScrolling)
        {
            state.scrollOffset -= e.delta.x / state.zoomLevel;
            state.scrollOffset = Mathf.Max(0, state.scrollOffset);
            e.Use();
            window.Repaint();
        }
        if (e.type == EventType.MouseUp && e.button == 2)
        {
            isScrolling = false;
            e.Use();
        }
        
        if (e.alt && e.type == EventType.MouseDrag && fullTimelineArea.Contains(e.mousePosition))
        {
             state.scrollOffset -= e.delta.x / state.zoomLevel;
             state.scrollOffset = Mathf.Max(0, state.scrollOffset);
             e.Use();
             window.Repaint();
        }

        if (!timelineArea.Contains(e.mousePosition) && !isDragging) return;

        if (e.type == EventType.MouseDown)
        {
            int clickedNoteIndex = GetNoteAtPosition(e.mousePosition, state, timelineArea, laneHeight);

            if (e.button == 0) // Left click
            {
                if (clickedNoteIndex >= 0)
                {
                    state.selectedNoteIndex = clickedNoteIndex;
                    isDragging = true;
                    dragNoteIndex = clickedNoteIndex;
                    e.Use();
                }
                else
                {
                    // Add Note
                    float rawTime = XToTime(e.mousePosition.x, state, timelineArea);
                    float snappedTime = SnapUtility.SnapTime(rawTime, state.chart.bpm, state.snapMode);
                    if (snappedTime < 0) snappedTime = 0;
                    
                    int lane = Mathf.FloorToInt((e.mousePosition.y - timelineArea.y) / laneHeight);
                    if (lane >= 0 && lane < 4)
                    {
                        DIRECTION dir = laneDirections[lane];
                        ChartNoteData newNote = new ChartNoteData
                        {
                            time = snappedTime,
                            direction = dir.ToString(),
                            color = state.currentColor.ToString()
                        };
                        state.chart.notes.Add(newNote);
                        state.chart.SortNotesByTime();
                        state.selectedNoteIndex = state.chart.notes.IndexOf(newNote);
                        state.isDirty = true;
                        e.Use();
                    }
                }
            }
            else if (e.button == 1) // Right click
            {
                if (clickedNoteIndex >= 0)
                {
                    state.selectedNoteIndex = clickedNoteIndex;
                    GenericMenu menu = new GenericMenu();
                    menu.AddItem(new GUIContent("Copy"), false, () => {
                        state.copiedNote = JsonUtility.FromJson<ChartNoteData>(JsonUtility.ToJson(state.chart.notes[state.selectedNoteIndex]));
                    });
                    menu.AddItem(new GUIContent("Delete"), false, () => {
                        state.chart.notes.RemoveAt(state.selectedNoteIndex);
                        state.selectedNoteIndex = -1;
                        state.isDirty = true;
                        window.Repaint();
                    });
                    menu.ShowAsContext();
                    e.Use();
                }
            }
        }
        else if (e.type == EventType.MouseDrag && isDragging && e.button == 0)
        {
            if (dragNoteIndex >= 0 && dragNoteIndex < state.chart.notes.Count)
            {
                float rawTime = XToTime(e.mousePosition.x, state, timelineArea);
                float snappedTime = SnapUtility.SnapTime(rawTime, state.chart.bpm, state.snapMode);
                if (snappedTime < 0) snappedTime = 0;
                
                int lane = Mathf.FloorToInt((e.mousePosition.y - timelineArea.y) / laneHeight);
                lane = Mathf.Clamp(lane, 0, 3);
                
                var note = state.chart.notes[dragNoteIndex];
                note.time = snappedTime;
                note.direction = laneDirections[lane].ToString();
                state.isDirty = true;
                e.Use();
                window.Repaint();
            }
        }
        else if (e.type == EventType.MouseUp && e.button == 0)
        {
            if (isDragging)
            {
                ChartNoteData draggedNote = null;
                if (dragNoteIndex >= 0 && dragNoteIndex < state.chart.notes.Count)
                    draggedNote = state.chart.notes[dragNoteIndex];

                isDragging = false;
                dragNoteIndex = -1;

                state.chart.SortNotesByTime();

                if (draggedNote != null)
                    state.selectedNoteIndex = state.chart.notes.IndexOf(draggedNote);

                e.Use();
                window.Repaint();
            }
        }
    }

    private static float TimeToX(float time, ChartEditorState state, Rect rect)
    {
        return rect.x + (time - state.scrollOffset) * state.zoomLevel;
    }

    private static float XToTime(float x, ChartEditorState state, Rect rect)
    {
        return (x - rect.x) / state.zoomLevel + state.scrollOffset;
    }

    private static Rect GetNoteRectLocal(ChartNoteData note, ChartEditorState state, float laneHeight)
    {
        float xPos = (note.time - state.scrollOffset) * state.zoomLevel;
        DIRECTION dir = (DIRECTION)System.Enum.Parse(typeof(DIRECTION), note.direction);
        int lane = System.Array.IndexOf(laneDirections, dir);
        if (lane < 0) lane = 0;
        
        float yPos = lane * laneHeight + NOTE_PADDING;
        return new Rect(xPos - NOTE_WIDTH / 2f, yPos, NOTE_WIDTH, laneHeight - NOTE_PADDING * 2f);
    }
    
    private static int GetNoteAtPosition(Vector2 mousePos, ChartEditorState state, Rect timelineArea, float laneHeight)
    {
        Vector2 localMouse = new Vector2(mousePos.x - timelineArea.x, mousePos.y - timelineArea.y);
        
        for (int i = 0; i < state.chart.notes.Count; i++)
        {
            Rect noteRect = GetNoteRectLocal(state.chart.notes[i], state, laneHeight);
            if (noteRect.Contains(localMouse))
            {
                return i;
            }
        }
        return -1;
    }
}
