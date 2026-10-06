using UnityEditor;
using UnityEngine;
using RhythmEditor;

public static class ChartEditorInput
{
    public static void HandleInput(ChartEditorState state, Event evt, EditorWindow window)
    {
        if (evt.type != EventType.KeyDown) return;

        // Space: toggle play/pause
        if (evt.keyCode == KeyCode.Space)
        {
            if (state.isPlaying)
                ChartEditorPlayback.PausePlayback(state);
            else
                ChartEditorPlayback.StartPlayback(state);
            evt.Use();
            window.Repaint();
        }

        // Delete: delete selected note
        if (evt.keyCode == KeyCode.Delete && state.HasSelection)
        {
            state.chart.notes.RemoveAt(state.selectedNoteIndex);
            state.selectedNoteIndex = -1;
            state.isDirty = true;
            evt.Use();
            window.Repaint();
        }

        // Ctrl+C: copy
        if (evt.control && evt.keyCode == KeyCode.C && state.HasSelection)
        {
            state.copiedNote = JsonUtility.FromJson<ChartNoteData>(JsonUtility.ToJson(state.SelectedNote));
            evt.Use();
        }

        // Ctrl+V: paste at current time
        if (evt.control && evt.keyCode == KeyCode.V && state.copiedNote != null)
        {
            var newNote = JsonUtility.FromJson<ChartNoteData>(JsonUtility.ToJson(state.copiedNote));
            newNote.time = state.currentTime;
            state.chart.notes.Add(newNote);
            state.chart.SortNotesByTime();
            state.selectedNoteIndex = state.chart.notes.IndexOf(newNote);
            state.isDirty = true;
            evt.Use();
            window.Repaint();
        }

        // Ctrl+S: save
        if (evt.control && evt.keyCode == KeyCode.S)
        {
            // Trigger save - same as toolbar save
            if (!string.IsNullOrEmpty(state.currentFilePath))
                ChartSerializer.Save(state.chart, state.currentFilePath);
            evt.Use();
        }
    }
}
