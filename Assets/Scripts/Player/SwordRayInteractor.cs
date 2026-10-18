using EzySlice;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;


public class SwordRayInteractor : XRSimpleInteractable
{
    [SerializeField] GameObject parent;
    [SerializeField] GameObject cubeBroken;

    void ParcayaFizikEkle(GameObject parca, Vector3 ayrilmaKuvveti)
    {
        // Parçalara fizik bileşenlerini ekle
        Rigidbody rb = parca.AddComponent<Rigidbody>();
        MeshCollider col = parca.AddComponent<MeshCollider>();
        col.convex = true;

        // İki parçanın birbirinden ayrılması için zıt yönlerde hafif itme kuvveti ver
        rb.AddForce(ayrilmaKuvveti, ForceMode.Impulse);
        parca.transform.position = parent.transform.position;
        parca.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);

        // Bir süre sonra sahneden temizle
        Destroy(parca, 4f);
    }


    protected override void OnHoverEntered(HoverEnterEventArgs args)
    {
        base.OnHoverEntered(args);

        string interactorName = args.interactorObject.transform.name;

        if (interactorName.Contains("Left") && parent.GetComponent<Note>().GetColor() == COLOR.RED)
        {
            PlayerManager.instance.DebugLogLong(gameObject.name);

            if (gameObject.name == parent.GetComponent<Note>().GetDirection().ToString())
            {
                PlayerManager.instance.AddScore(10, gameObject.transform);
                Debug.Log("10-points");
                PlayerManager.instance.DebugLog("NP::");
            }
            else
            {
                Debug.Log("Wrong direction::");
                PlayerManager.instance.DebugLog("Wrong direction::");
                PlayerManager.instance.WrongDirOrColor();
            }
        }
        
        else if(interactorName.Contains("Right") && parent.GetComponent<Note>().GetColor() == COLOR.BLUE)
        {
            PlayerManager.instance.DebugLogLong(gameObject.name);

            if (gameObject.name == parent.GetComponent<Note>().GetDirection().ToString())
            {
                PlayerManager.instance.AddScore(10, gameObject.transform);
                Debug.Log("10-points");
                PlayerManager.instance.DebugLog("NP::");
            }
            else
            {
                Debug.Log("Wrong direction::");
                PlayerManager.instance.DebugLog("Wrong direction::");
                PlayerManager.instance.WrongDirOrColor();
            }
        }
        
        else
        {
            Debug.Log("Wrong color::");
            PlayerManager.instance.DebugLog("Wrong color::");
            PlayerManager.instance.WrongDirOrColor();
        }

        float randomForce = Random.Range(3.0f, 5.0f);

        //// 1. Küpün üzerine gelen interactor'ı (el/lazer/kontrolcü) alıyoruz
        //Transform interactorTransform = args.interactorObject.transform;

        if (gameObject.name == "LEFT" || gameObject.name == "RIGHT")
        {
            //interactorTransform.Rotate(0f, 0f, 90f);
            GameObject newCubeBroken = Instantiate(cubeBroken, parent.transform.position, Quaternion.Euler(0f, 0f, 90f));
            newCubeBroken.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material = parent.transform.GetChild(5).transform.GetChild(0).GetComponent<MeshRenderer>().material;
            newCubeBroken.transform.GetChild(1).gameObject.GetComponent<MeshRenderer>().material = parent.transform.GetChild(5).transform.GetChild(0).GetComponent<MeshRenderer>().material;

            newCubeBroken.transform.GetChild(0).gameObject.GetComponent<Rigidbody>().AddForce(Vector3.up * randomForce, ForceMode.Impulse);
            newCubeBroken.transform.GetChild(1).gameObject.GetComponent<Rigidbody>().AddForce(Vector3.down * randomForce, ForceMode.Impulse);
            Destroy(newCubeBroken, 0.5f);
        }

        else if (gameObject.name == "UP" || gameObject.name == "DOWN")
        {
            GameObject newCubeBroken = Instantiate(cubeBroken, parent.transform.position, Quaternion.Euler(0f, 0f, 0f));
            newCubeBroken.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material = parent.transform.GetChild(5).transform.GetChild(0).GetComponent<MeshRenderer>().material;
            newCubeBroken.transform.GetChild(1).gameObject.GetComponent<MeshRenderer>().material = parent.transform.GetChild(5).transform.GetChild(0).GetComponent<MeshRenderer>().material;

            newCubeBroken.transform.GetChild(0).gameObject.GetComponent<Rigidbody>().AddForce(Vector3.up * randomForce + (Vector3.left * randomForce), ForceMode.Impulse);
            newCubeBroken.transform.GetChild(1).gameObject.GetComponent<Rigidbody>().AddForce(Vector3.down * randomForce + (Vector3.right * randomForce), ForceMode.Impulse);

            Destroy(newCubeBroken, 0.5f);
        }

        //GameObject cube = parent.transform.GetChild(5).transform.GetChild(0).gameObject;
        //Material material = cube.GetComponent<MeshRenderer>().material;

        //// 2. Kesme noktasını ve kesim açısını (düzlemini) belirle
        //Vector3 kesimNoktasi = interactorTransform.position;
        //// Dilimleme düzlemi olarak kontrolcünün yukarı veya ileri yönünü kullanabilirsin:
        //Vector3 kesimNormalligi = interactorTransform.up;

        //// 3. Küpü dilimle
        //SlicedHull parcalar = cube.Slice(kesimNoktasi, kesimNormalligi, material);

        //if (parcalar != null)
        //{
        //    // İki yeni fiziksel parçayı oluştur
        //    GameObject ustParca = parcalar.CreateUpperHull(cube, material);
        //    GameObject altParca = parcalar.CreateLowerHull(cube, material);

        //    ParcayaFizikEkle(ustParca, kesimNormalligi * 2f);
        //    ParcayaFizikEkle(altParca, -kesimNormalligi * 2f);
        //}
        HapticFeedbackController.TriggerHaptic(args.interactorObject, 0.4f, 0.1f);

        Destroy(parent);
    }
}
