using System.Collections.Generic;
using UnityEngine;

public class WallPartition : MonoBehaviour
{
    // Unity Inspector'da göreceðin 4 bölge listesi
    public List<Rigidbody> partLeft = new List<Rigidbody>();
    public List<Rigidbody> partMiddleLeft = new List<Rigidbody>();
    public List<Rigidbody> partMiddleRight = new List<Rigidbody>();
    public List<Rigidbody> partRight = new List<Rigidbody>();

    [ContextMenu("Tuðlalarý Bölgelere Ayýr")]
    public void CreateWallPartition()
    {
        partLeft.Clear();
        partMiddleLeft.Clear();
        partMiddleRight.Clear();
        partRight.Clear();

        Rigidbody[] bricks = GetComponentsInChildren<Rigidbody>();
        if (bricks.Length == 0) return;

        float minX = float.MaxValue;
        float maxX = float.MinValue;

        foreach (var t in bricks)
        {
            float x = t.transform.position.x;
            if (x < minX) minX = x;
            if (x > maxX) maxX = x;
        }

        float width = maxX - minX;

        foreach (var t in bricks)
        {
            float normalizeX = (t.transform.position.x - minX) / width;

            float randomX = Mathf.Clamp01(normalizeX + Random.Range(-0.04f, 0.04f));

            if (randomX < 0.25f)
                partLeft.Add(t);
            else if (randomX < 0.50f)
                partMiddleLeft.Add(t);
            else if (randomX < 0.75f)
                partMiddleRight.Add(t);
            else
                partRight.Add(t);
        }

        Debug.Log($"Ayrým Tamamlandý! Sol: {partLeft.Count}, Orta-Sol: {partMiddleLeft.Count}, Orta-Sað: {partMiddleRight.Count}, Sað: {partRight.Count}");
    }

    public void BreakPart(List<Rigidbody> part, Vector3 darbeNoktasi, float force, float yaricap)
    {
        foreach (var rb in part)
        {
            rb.isKinematic = false;
            rb.AddExplosionForce(force, darbeNoktasi, yaricap, 0.4f, ForceMode.Impulse);
        }
    }
}
