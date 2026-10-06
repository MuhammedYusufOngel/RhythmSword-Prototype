using UnityEditor;
using UnityEngine;
using RhythmEditor;

public static class ChartEditorInspector
{
    public static void Draw(ChartEditorState state, EditorWindow window)
    {
        if (!state.HasSelection)
        {
            EditorGUILayout.HelpBox("No note selected. Click on a note in the timeline or click in a lane to create one.", MessageType.Info);
            return;
        }

        var note = state.SelectedNote;
        EditorGUILayout.LabelField("Selected Note", EditorStyles.boldLabel);

        EditorGUILayout.BeginHorizontal();
        {
            EditorGUILayout.LabelField("Time:", GUILayout.Width(40));
            float newTime = EditorGUILayout.FloatField(note.time, GUILayout.Width(80));
            if (Mathf.Abs(newTime - note.time) > 0.0001f)
            {
                note.time = Mathf.Max(0f, newTime);
                state.chart.SortNotesByTime();
                state.selectedNoteIndex = state.chart.notes.IndexOf(note);
                state.isDirty = true;
            }

            EditorGUILayout.LabelField("Dir:", GUILayout.Width(25));
            DIRECTION dir = (DIRECTION)System.Enum.Parse(typeof(DIRECTION), note.direction);
            DIRECTION newDir = (DIRECTION)EditorGUILayout.EnumPopup(dir, GUILayout.Width(70));
            if (newDir != dir) { note.direction = newDir.ToString(); state.isDirty = true; }

            EditorGUILayout.LabelField("Color:", GUILayout.Width(35));
            COLOR col = (COLOR)System.Enum.Parse(typeof(COLOR), note.color);
            COLOR newCol = (COLOR)EditorGUILayout.EnumPopup(col, GUILayout.Width(60));
            if (newCol != col) { note.color = newCol.ToString(); state.isDirty = true; }

            EditorGUILayout.LabelField("Type:", GUILayout.Width(32));
            note.type = EditorGUILayout.TextField(note.type, GUILayout.Width(60));
        }
        EditorGUILayout.EndHorizontal();

        // Position row
        EditorGUILayout.BeginHorizontal();
        {
            note.hasCustomPosition = EditorGUILayout.Toggle("Custom Position", note.hasCustomPosition, GUILayout.Width(140));
            EditorGUI.BeginDisabledGroup(!note.hasCustomPosition);
            EditorGUILayout.LabelField("X:", GUILayout.Width(15));
            note.positionX = EditorGUILayout.FloatField(note.positionX, GUILayout.Width(50));
            EditorGUILayout.LabelField("Y:", GUILayout.Width(15));
            note.positionY = EditorGUILayout.FloatField(note.positionY, GUILayout.Width(50));
            EditorGUI.EndDisabledGroup();

            GUILayout.FlexibleSpace();

            if (GUILayout.Button("Copy", GUILayout.Width(45)))
            {
                state.copiedNote = JsonUtility.FromJson<ChartNoteData>(JsonUtility.ToJson(note));
            }
            if (GUILayout.Button("Delete", GUILayout.Width(50)))
            {
                state.chart.notes.RemoveAt(state.selectedNoteIndex);
                state.selectedNoteIndex = -1;
                state.isDirty = true;
                window.Repaint();
            }
        }
        EditorGUILayout.EndHorizontal();
    }
}
