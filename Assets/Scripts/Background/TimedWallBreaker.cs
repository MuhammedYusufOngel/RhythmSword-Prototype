using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedWallBreaker : MonoBehaviour
{
    [System.Serializable]
    public class WallSection
    {
        public string sectionName = "Bölüm";
        public Transform sectionParent;
        [HideInInspector] public List<Rigidbody> pieces = new List<Rigidbody>();
    }
    [Header("Wall_Parts")]
    [SerializeField] private WallSection[] wallSections = new WallSection[4];

    [Header("Physics")]
    [SerializeField] private float forwardForce = 1.2f;
    [SerializeField] private float downwardForce = 0.8f;

    [Header("Time between two stage")]
    [SerializeField] private float timeBetweenStages = 3.0f;

    [Header("Particle Prefab")]
    [SerializeField] private ParticleSystem dustEffectPrefab;

    private float[] timeThatWallWillBroken = new float[5] { 85f, 61f, 103f, 21f, 10000f };

    void Start()
    {
        for (int i = 0; i < wallSections.Length; i++)
        {
            if (wallSections[i].sectionParent != null)
            {
                Rigidbody[] rbs = wallSections[i].sectionParent.GetComponentsInChildren<Rigidbody>();
                foreach (Rigidbody rb in rbs)
                {
                    rb.isKinematic = true;
                    rb.useGravity = true;

                    wallSections[i].pieces.Add(rb);
                }
            }
        }

        StartCoroutine(BreakWallRoutine());
    }

    private IEnumerator BreakWallRoutine()
    {
        yield return new WaitForSeconds(timeThatWallWillBroken[0]); // Başlangıç gecikmesi

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
            
            if (dustEffectPrefab != null && section.sectionParent != null)
            {
                Instantiate(dustEffectPrefab, rb.transform.position, Quaternion.identity);
            }
        }
    }
}
