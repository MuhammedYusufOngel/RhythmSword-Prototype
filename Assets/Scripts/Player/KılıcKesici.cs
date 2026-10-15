using EzySlice;
using UnityEngine;

public class KılıcKesici : MonoBehaviour
{
    public Transform kilicUcu;
    public Transform kilicKabzasi;
    public Material kesilenIcYuzeyMateryali; // Kesilen yerin iç dolgu rengi

    private void OnTriggerEnter(Collider other)
    {
        // Sadece kesilebilir nesneleri hedef al
        if (other.CompareTag("KesilebilirKup"))
        {
            Kes(other.gameObject);
        }
    }

    void Kes(GameObject hedefKup)
    {
        // 1. Kılıcın pozisyonu ve hareket yönüyle kesme düzlemini hesapla
        Vector3 kesimNoktasi = kilicUcu.position;
        Vector3 kesimNormalligi = Vector3.Cross(kilicUcu.position - kilicKabzasi.position, transform.forward).normalized;

        // 2. Küpü tam bu düzlemden iki parçaya böl
        SlicedHull parcalar = hedefKup.Slice(kesimNoktasi, kesimNormalligi, kesilenIcYuzeyMateryali);

        if (parcalar != null)
        {
            // 3. Üst ve alt parçaları bağımsız 3D nesneler olarak üret
            GameObject ustParca = parcalar.CreateUpperHull(hedefKup, kesilenIcYuzeyMateryali);
            GameObject altParca = parcalar.CreateLowerHull(hedefKup, kesilenIcYuzeyMateryali);

            // 4. Parçalara fizik kazandır
            ParcayaFizikEkle(ustParca, kesimNormalligi * 2f);
            ParcayaFizikEkle(altParca, -kesimNormalligi * 2f);

            // 5. Orijinal küpü sahneden sil
            Destroy(hedefKup);
        }
    }

    void ParcayaFizikEkle(GameObject parca, Vector3 ayrilmaKuvveti)
    {
        // Parçalara fizik bileşenlerini ekle
        Rigidbody rb = parca.AddComponent<Rigidbody>();
        MeshCollider col = parca.AddComponent<MeshCollider>();
        col.convex = true;

        // İki parçanın birbirinden ayrılması için zıt yönlerde hafif itme kuvveti ver
        rb.AddForce(ayrilmaKuvveti, ForceMode.Impulse);

        // Bir süre sonra sahneden temizle
        Destroy(parca, 4f);
    }
}
