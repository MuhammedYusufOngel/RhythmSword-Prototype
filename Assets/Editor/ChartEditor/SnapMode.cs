using UnityEngine;

namespace RhythmEditor
{
    public enum SnapMode { Off, Whole, Half, Quarter, Eighth, Sixteenth }

    public static class SnapUtility
    {
        public static float GetSnapInterval(float bpm, SnapMode mode)
        {
            if (mode == SnapMode.Off || bpm <= 0) return 0f;
            float beatDuration = 60f / bpm;
            switch (mode)
            {
                case SnapMode.Whole: return beatDuration;
                case SnapMode.Half: return beatDuration / 2f;
                case SnapMode.Quarter: return beatDuration / 4f;
                case SnapMode.Eighth: return beatDuration / 8f;
                case SnapMode.Sixteenth: return beatDuration / 16f;
                default: return 0f;
            }
        }

        public static float SnapTime(float time, float bpm, SnapMode mode)
        {
            float interval = GetSnapInterval(bpm, mode);
            if (interval <= 0f) return time;
            return Mathf.Round(time / interval) * interval;
        }
    }
}
