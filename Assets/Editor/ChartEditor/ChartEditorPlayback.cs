using System.Reflection;
using UnityEditor;
using UnityEngine;

public static class ChartEditorPlayback
{
    private static System.Type audioUtilType;
    private static bool audioUtilInitialized;
    private static double playbackStartEditorTime;
    private static float playbackStartSongTime;

    static void EnsureInit()
    {
        if (!audioUtilInitialized)
        {
            audioUtilType = typeof(AudioImporter).Assembly.GetType("UnityEditor.AudioUtil");
            audioUtilInitialized = true;
        }
    }

    public static void Draw(ChartEditorState state, EditorWindow window)
    {
        EditorGUILayout.BeginHorizontal();
        {
            // Play button
            if (GUILayout.Button(state.isPlaying ? "⏸ Pause" : "▶ Play", GUILayout.Width(70)))
            {
                if (state.isPlaying)
                    PausePlayback(state);
                else
                    StartPlayback(state);
            }

            // Stop button
            if (GUILayout.Button("⏹ Stop", GUILayout.Width(60)))
                StopPlayback(state);

            EditorGUILayout.Space(10);

            // Current time (editable for seeking)
            EditorGUILayout.LabelField("Time:", GUILayout.Width(35));
            float newTime = EditorGUILayout.FloatField(state.currentTime, GUILayout.Width(70));
            if (Mathf.Abs(newTime - state.currentTime) > 0.001f)
                SeekTo(state, newTime);

            // Duration
            EditorGUILayout.LabelField("/", GUILayout.Width(10));
            EditorGUILayout.LabelField(FormatTime(state.songDuration), GUILayout.Width(60));

            GUILayout.FlexibleSpace();

            // Speed
            EditorGUILayout.LabelField("Speed:", GUILayout.Width(42));
            float[] speeds = { 0.25f, 0.5f, 1f, 2f };
            string[] speedLabels = { "0.25x", "0.5x", "1x", "2x" };
            int currentSpeedIdx = System.Array.IndexOf(speeds, state.playbackSpeed);
            if (currentSpeedIdx < 0) currentSpeedIdx = 2;
            int newIdx = EditorGUILayout.Popup(currentSpeedIdx, speedLabels, GUILayout.Width(60));
            if (newIdx != currentSpeedIdx)
            {
                state.playbackSpeed = speeds[newIdx];
                if (state.isPlaying) { StopPlayback(state); StartPlayback(state); } // restart with new speed
            }
        }
        EditorGUILayout.EndHorizontal();
    }

    public static void StartPlayback(ChartEditorState state)
    {
        if (state.audioClip == null) return;
        EnsureInit();
        state.isPlaying = true;
        playbackStartEditorTime = EditorApplication.timeSinceStartup;
        playbackStartSongTime = state.currentTime;

        // Play preview clip from current position
        int startSample = (int)(state.currentTime * state.audioClip.frequency);
        PlayPreviewClip(state.audioClip, startSample, false);
    }

    public static void PausePlayback(ChartEditorState state)
    {
        state.isPlaying = false;
        StopPreviewClip();
    }

    public static void StopPlayback(ChartEditorState state)
    {
        state.isPlaying = false;
        state.currentTime = 0f;
        StopPreviewClip();
    }

    public static void SeekTo(ChartEditorState state, float time)
    {
        state.currentTime = Mathf.Clamp(time, 0f, state.songDuration);
        if (state.isPlaying)
        {
            StopPreviewClip();
            StartPlayback(state);
        }
    }

    public static void UpdatePlayback(ChartEditorState state)
    {
        if (!state.isPlaying) return;
        double elapsed = EditorApplication.timeSinceStartup - playbackStartEditorTime;
        state.currentTime = playbackStartSongTime + (float)(elapsed * state.playbackSpeed);
        if (state.currentTime >= state.songDuration)
        {
            StopPlayback(state);
        }
    }

    // Reflection-based AudioUtil wrappers
    private static void PlayPreviewClip(AudioClip clip, int startSample, bool loop)
    {
        EnsureInit();
        if (audioUtilType == null) return;
        
        var method = audioUtilType.GetMethod("PlayPreviewClip", BindingFlags.Static | BindingFlags.Public, null,
            new System.Type[] { typeof(AudioClip), typeof(int), typeof(bool) }, null);
        if (method != null)
        {
            method.Invoke(null, new object[] { clip, startSample, loop });
            return;
        }
        
        method = audioUtilType.GetMethod("PlayPreviewClip", BindingFlags.Static | BindingFlags.Public, null,
            new System.Type[] { typeof(AudioClip) }, null);
        method?.Invoke(null, new object[] { clip });
    }

    private static void StopPreviewClip()
    {
        EnsureInit();
        if (audioUtilType == null) return;
        var method = audioUtilType.GetMethod("StopAllPreviewClips", BindingFlags.Static | BindingFlags.Public);
        method?.Invoke(null, null);
    }

    private static string FormatTime(float seconds)
    {
        int min = (int)(seconds / 60);
        float sec = seconds % 60;
        return string.Format("{0}:{1:00.000}", min, sec);
    }
}
