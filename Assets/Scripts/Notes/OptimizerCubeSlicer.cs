using EzySlice;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class OptimizerCubeSlicer : MonoBehaviour
{
    public GameObject realCubeMeshObject;

    public float force;
    [Range(0f, 1f)]
    public float centerShiftRatio = 0.5f; // 0 = edge, 1 = center

    private bool isSlice = false;

    // Alt collider'ların (Sol, Sağ, Üst, Alt) OnHoverEntered event'ine bu fonksiyonu bağla
    public void SliceFromDirection(HoverEnterEventArgs args)
    {
        if (isSlice) return;
        isSlice = true;

        Transform handTransform = args.interactorObject.transform;
        Transform directionObject = args.interactableObject.transform;

        // 1. ÇOK KÜÇÜK KESMEYİ ÖNLEME:
        // Kesim noktasını elin ilk değdiği dış kabuk yerine, küpün merkezine doğru yaklaştırıyoruz.
        Vector3 cubeCenter = realCubeMeshObject.transform.position;
        Vector3 sliceCenter = Vector3.Lerp(handTransform.position, cubeCenter, centerShiftRatio);

        // 2. TERS KESMEYİ ÖNLEME:
        // Elin küpün merkezine doğru giriş vektörünü buluyoruz
        Vector3 directionOfSlice = (cubeCenter - handTransform.position).normalized;

        // Vuruş yönü ile elin ileri yönünü çapraz çarpıp (Cross Product) tam kılıcın kesim düzlemini buluyoruz
        Vector3 normalizedOfCenter = Vector3.Cross(directionOfSlice, handTransform.forward).normalized;

        // Eğer el çok dik girdiyse ve Cross Product sıfıra yakınsa yedek olarak elin sağ/üst yönünü kullan
        if (normalizedOfCenter.sqrMagnitude < 0.1f)
        {
            normalizedOfCenter = handTransform.up;
        }

        // 3. DİLİMLEME İŞLEMİ
        SlicedHull parts = realCubeMeshObject.Slice(sliceCenter, normalizedOfCenter, realCubeMeshObject.GetComponent<MeshRenderer>().material);

        if (parts != null)
        {
            GameObject upperPart = parts.CreateUpperHull(realCubeMeshObject, realCubeMeshObject.GetComponent<MeshRenderer>().material);
            GameObject altParca = parts.CreateLowerHull(realCubeMeshObject, realCubeMeshObject.GetComponent<MeshRenderer>().material);

            // Parçaların pozisyon ve ölçeklerini ana küple eşitle
            SetAndThrowPart(upperPart, normalizedOfCenter * force);
            SetAndThrowPart(altParca, -normalizedOfCenter * force);

            // En üstteki ana küpü (ve altındaki 4 collider'ı) tamamen yok et
            Destroy(gameObject);
        }
        else
        {
            // Nadiren kesim açısı küpü ıskalarsa kilidi aç ki tekrar vurulabilsin
            isSlice = false;
        }
    }

    private void SetAndThrowPart(GameObject part, Vector3 addForce)
    {
        // EzySlice yeni parçayı dünya (0,0,0) noktasında oluşturabilir, konumunu ana küpe eşitliyoruz:
        part.transform.position = realCubeMeshObject.transform.position;
        part.transform.rotation = realCubeMeshObject.transform.rotation;
        part.transform.localScale = realCubeMeshObject.transform.lossyScale;

        MeshCollider col = part.AddComponent<MeshCollider>();
        col.convex = true;

        Rigidbody rb = part.AddComponent<Rigidbody>();

        // İki parçanın birbirine çarpıp patlamaması için hafifçe birbirinden uzağa ötele
        part.transform.position += addForce.normalized * 0.02f;

        rb.AddForce(addForce, ForceMode.Impulse);
        //rb.AddTorque(Random.insideUnitSphere * 2f, ForceMode.Impulse); // Havada dönsün

        Destroy(part, 2f);
    }
}
