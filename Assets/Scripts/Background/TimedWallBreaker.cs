using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class TimedWallBreaker : MonoBehaviour
{
    [System.Serializable]
    public class WallSection
    {
        public string sectionName = "Bölüm";
        [HideInInspector] public List<Rigidbody> pieces = new List<Rigidbody>();
    }
    [Header("Wall_Parts")]
    [SerializeField] private WallSection[] wallSections = new WallSection[4];

    [Header("Physics")]
    [SerializeField] private float forwardForce = 1.2f;

    [Header("Particle Prefab")]
    [SerializeField] private ParticleSystem dustEffectPrefab;

    private float[] timeThatWallWillBroken = new float[5] { 85f, 61f, 103f, 21f, 10000f };
    //private float[] timeThatWallWillBroken = new float[5] { 1f, 5f, 5f, 5f, 10000f };

    public void CreateWallPartition()
    {
        Rigidbody[] bricks = GetComponentsInChildren<Rigidbody>();
        if (bricks.Length == 0) return;

        float minX = float.MaxValue;
        float maxX = float.MinValue;
        float minY = float.MaxValue;
        float maxY = float.MinValue;

        foreach (var t in bricks)
        {
            float x = t.transform.position.x;
            float y = t.transform.position.y;

            if (x < minX) minX = x;
            if (x > maxX) maxX = x;
            if (y < minY) minY = y;
            if (y > maxY) maxY = y;
        }

        float width = maxX - minX;
        float height = maxY - minY;

        foreach (var t in bricks)
        {
            float normalizeX = (t.transform.position.x - minX) / width;
            float normalizeY = (t.transform.position.y - minY) / height;

            float randomX = Mathf.Clamp01(normalizeX + Random.Range(-0.04f, 0.04f));
            float randomY = Mathf.Clamp01(normalizeY + Random.Range(-0.04f, 0.04f));

            t.isKinematic = true;
            t.useGravity = true;

            if (randomX < 0.25f)
                wallSections[0].pieces.Add(t);
            else if (randomX < 0.75f)
            {
                if(randomY < 0.25f)
                    wallSections[2].pieces.Add(t);
                else if(randomY < 0.5f)
                    wallSections[3].pieces.Add(t);
                else if(randomY < 0.75f)
                {
                    if(randomX < 0.65f)
                        wallSections[3].pieces.Add(t);
                    else
                        wallSections[1].pieces.Add(t);
                }
                else
                    wallSections[2].pieces.Add(t);
            }
            else
                wallSections[1].pieces.Add(t);
        }
    }

    void Start()
    {
        //for (int i = 0; i < wallSections.Length; i++)
        //{
        //    if (wallSections[i].sectionParent != null)
        //    {
        //        Rigidbody[] rbs = wallSections[i].sectionParent.GetComponentsInChildren<Rigidbody>();
        //        foreach (Rigidbody rb in rbs)
        //        {
        //            rb.isKinematic = true;
        //            rb.useGravity = true;

        //            wallSections[i].pieces.Add(rb);
        //        }
        //    }
        //}

        CreateWallPartition();

        StartCoroutine(BreakWallRoutine());
    }

    private IEnumerator BreakWallRoutine()
    {
        yield return new WaitForSeconds(timeThatWallWillBroken[0]);

        for (int i = 0; i < wallSections.Length; i++)
        {
            BreakSection(i);
            yield return new WaitForSeconds(timeThatWallWillBroken[i+1]);
        }
    }

    private void BreakSection(int sectionIndex)
    {
        if (sectionIndex < 0 || sectionIndex >= wallSections.Length) return;

        WallSection section = wallSections[sectionIndex];

        foreach (Rigidbody rb in section.pieces)
        {
            if (rb == null) continue;

            Debug.Log("Buraya girdi...");
            rb.isKinematic = false;

            rb.AddForce(Vector3.left * forwardForce, ForceMode.Impulse);
            rb.AddTorque(Random.insideUnitSphere * 1.5f, ForceMode.Impulse);
            
            if (dustEffectPrefab != null)
            {
                Instantiate(dustEffectPrefab, rb.transform.position, Quaternion.identity);
            }
        }
    }
}
