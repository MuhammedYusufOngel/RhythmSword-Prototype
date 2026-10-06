using UnityEditor;
using UnityEngine;

public class ChartEditorWindow : EditorWindow
{
    private ChartEditorState state = new ChartEditorState();
    private Vector2 timelineScroll;

    [MenuItem("Tools/VR Rhythm/Chart Editor")]
    public static void ShowWindow() { GetWindow<ChartEditorWindow>("Chart Editor"); }

    void OnEnable()
    {
        EditorApplication.update += OnEditorUpdate;
    }

    void OnDisable()
    {
        EditorApplication.update -= OnEditorUpdate;
        ChartEditorPlayback.StopPlayback(state);
    }

    void OnEditorUpdate()
    {
        if (state.isPlaying)
        {
            ChartEditorPlayback.UpdatePlayback(state);
            Repaint();
        }
    }

    void OnGUI()
    {
        // Handle keyboard input first
        ChartEditorInput.HandleInput(state, Event.current, this);

        // Draw panels top to bottom
        ChartEditorToolbar.Draw(state, this);
        EditorGUILayout.Space(4);
        ChartEditorPlayback.Draw(state, this);
        EditorGUILayout.Space(4);

        // Timeline gets remaining space
        Rect timelineRect = GUILayoutUtility.GetRect(position.width, position.height * 0.5f, GUILayout.ExpandHeight(true));
        if (timelineRect.width > 1 && timelineRect.height > 1)
            ChartEditorTimeline.Draw(state, timelineRect, this);

        EditorGUILayout.Space(4);
        ChartEditorInspector.Draw(state, this);
    }
}
