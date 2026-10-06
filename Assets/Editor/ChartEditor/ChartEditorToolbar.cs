using UnityEditor;
using UnityEngine;
using RhythmEditor;

public static class ChartEditorToolbar
{
    public static void Draw(ChartEditorState state, EditorWindow window)
    {
        // Row 1: Song info + file ops
        EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
        {
            EditorGUILayout.LabelField("Song:", GUILayout.Width(35));
            state.chart.songName = EditorGUILayout.TextField(state.chart.songName, GUILayout.Width(150));

            EditorGUILayout.LabelField("Audio:", GUILayout.Width(40));
            var newClip = (AudioClip)EditorGUILayout.ObjectField(state.audioClip, typeof(AudioClip), false, GUILayout.Width(150));
            if (newClip != state.audioClip)
            {
                state.audioClip = newClip;
                if (newClip != null)
                {
                    state.chart.audioClipPath = AssetDatabase.GetAssetPath(newClip);
                    state.songDuration = newClip.length;
                }
                state.isDirty = true;
            }

            EditorGUILayout.LabelField("BPM:", GUILayout.Width(30));
            float newBpm = EditorGUILayout.FloatField(state.chart.bpm, GUILayout.Width(50));
            if (newBpm != state.chart.bpm) { state.chart.bpm = newBpm; state.isDirty = true; }

            GUILayout.FlexibleSpace();

            if (GUILayout.Button("New", EditorStyles.toolbarButton, GUILayout.Width(40)))
                HandleNew(state, window);
            if (GUILayout.Button("Load", EditorStyles.toolbarButton, GUILayout.Width(40)))
                HandleLoad(state, window);
            if (GUILayout.Button("Save", EditorStyles.toolbarButton, GUILayout.Width(40)))
                HandleSave(state);
            if (GUILayout.Button("Save As", EditorStyles.toolbarButton, GUILayout.Width(55)))
                HandleSaveAs(state);
        }
        EditorGUILayout.EndHorizontal();

        // Row 2: Direction, Color, Snap, Zoom
        EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
        {
            EditorGUILayout.LabelField("Dir:", GUILayout.Width(25));
            // 4 toggle-style buttons for direction
            if (GUILayout.Toggle(state.currentDirection == DIRECTION.LEFT, "←", EditorStyles.toolbarButton, GUILayout.Width(25)))
                state.currentDirection = DIRECTION.LEFT;
            if (GUILayout.Toggle(state.currentDirection == DIRECTION.RIGHT, "→", EditorStyles.toolbarButton, GUILayout.Width(25)))
                state.currentDirection = DIRECTION.RIGHT;
            if (GUILayout.Toggle(state.currentDirection == DIRECTION.UP, "↑", EditorStyles.toolbarButton, GUILayout.Width(25)))
                state.currentDirection = DIRECTION.UP;
            if (GUILayout.Toggle(state.currentDirection == DIRECTION.DOWN, "↓", EditorStyles.toolbarButton, GUILayout.Width(25)))
                state.currentDirection = DIRECTION.DOWN;

            EditorGUILayout.Space(10);
            EditorGUILayout.LabelField("Color:", GUILayout.Width(35));
            if (GUILayout.Toggle(state.currentColor == COLOR.RED, "RED", EditorStyles.toolbarButton, GUILayout.Width(35)))
                state.currentColor = COLOR.RED;
            if (GUILayout.Toggle(state.currentColor == COLOR.BLUE, "BLUE", EditorStyles.toolbarButton, GUILayout.Width(40)))
                state.currentColor = COLOR.BLUE;

            EditorGUILayout.Space(10);
            EditorGUILayout.LabelField("Snap:", GUILayout.Width(32));
            state.snapMode = (RhythmEditor.SnapMode)EditorGUILayout.EnumPopup(state.snapMode, GUILayout.Width(80));

            EditorGUILayout.Space(10);
            EditorGUILayout.LabelField("Zoom:", GUILayout.Width(35));
            state.zoomLevel = EditorGUILayout.Slider(state.zoomLevel, 20f, 500f, GUILayout.Width(150));
        }
        EditorGUILayout.EndHorizontal();
    }

    private static void HandleNew(ChartEditorState state, EditorWindow window)
    {
        if (state.isDirty)
        {
            if (!EditorUtility.DisplayDialog("Unsaved Changes", "You have unsaved changes. Are you sure you want to create a new chart?", "Yes", "No"))
                return;
        }
        state.NewChart();
        window.Repaint();
    }

    private static void HandleLoad(ChartEditorState state, EditorWindow window)
    {
        if (state.isDirty)
        {
            if (!EditorUtility.DisplayDialog("Unsaved Changes", "You have unsaved changes. Are you sure you want to load a new chart?", "Yes", "No"))
                return;
        }

        string path = EditorUtility.OpenFilePanel("Load Chart", Application.dataPath, "json");
        if (!string.IsNullOrEmpty(path))
        {
            ChartData loadedChart = ChartSerializer.Load(path);
            if (loadedChart != null)
            {
                state.NewChart();
                state.chart = loadedChart;
                state.currentFilePath = path;
                if (!string.IsNullOrEmpty(state.chart.audioClipPath))
                {
                    state.audioClip = AssetDatabase.LoadAssetAtPath<AudioClip>(state.chart.audioClipPath);
                    if (state.audioClip != null)
                    {
                        state.songDuration = state.audioClip.length;
                    }
                }
                window.Repaint();
            }
        }
    }

    private static void HandleSave(ChartEditorState state)
    {
        if (string.IsNullOrEmpty(state.currentFilePath))
        {
            HandleSaveAs(state);
        }
        else
        {
            ChartSerializer.Save(state.chart, state.currentFilePath);
            state.isDirty = false;
        }
    }

    private static void HandleSaveAs(ChartEditorState state)
    {
        string defaultName = string.IsNullOrEmpty(state.chart.songName) ? "NewChart" : state.chart.songName;
        string path = EditorUtility.SaveFilePanel("Save Chart", Application.dataPath, defaultName + ".chart.json", "json");
        if (!string.IsNullOrEmpty(path))
        {
            state.currentFilePath = path;
            ChartSerializer.Save(state.chart, path);
            state.isDirty = false;
            AssetDatabase.Refresh();
        }
    }
}
