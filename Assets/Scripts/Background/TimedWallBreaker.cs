using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedWallBreaker : MonoBehaviour
{
    [System.Serializable]
    public class WallSection
    {
        public string sectionName = "Bölüm";
        [HideInInspector] public List<GameObject> pieces = new List<GameObject>();
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
        GameObject[] bricks = new GameObject[672];
        
        for(int i = 0; i < transform.childCount; i++)
        {
            bricks[i] = transform.GetChild(i).gameObject;
        }

        if (bricks.Length == 0)
        {
            Debug.Log("Burada hiçbir şey yok..");
            return;
        }

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

            t.GetComponent<Rigidbody>().isKinematic = true;
            t.GetComponent<Rigidbody>().useGravity = true;

            if (randomX < 0.25f)
                wallSections[0].pieces.Add(t);
            else if (randomX < 0.5f)
                wallSections[2].pieces.Add(t);
            else if (randomX < 0.75f)
                wallSections[3].pieces.Add(t);
            else
                wallSections[1].pieces.Add(t);
        }
    }

    void Start()
    {
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

        foreach (GameObject @object in section.pieces)
        {
            if (@object == null) continue;

            @object.GetComponent<Rigidbody>().isKinematic = false;

            @object.GetComponent<Rigidbody>().AddForce(Vector3.left * forwardForce, ForceMode.Impulse);
            @object.GetComponent<Rigidbody>().AddTorque(Random.insideUnitSphere * 1.5f, ForceMode.Impulse);

            //Destroy(@object, 5.0f);
            
            if (dustEffectPrefab != null)
            {
                Instantiate(dustEffectPrefab, @object.transform.position, Quaternion.identity);
            }
        }
    }
}
