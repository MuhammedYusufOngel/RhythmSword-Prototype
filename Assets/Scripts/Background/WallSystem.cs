using UnityEngine;

public class WallSystem : MonoBehaviour
{
    private float elapsedTime = 0f;
    [SerializeField] GameObject particleEffect;
    void Start()
    {
        
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;

        if(elapsedTime > 3f)
        {
            int random = Random.Range(0, 768);

            Instantiate(particleEffect, transform.GetChild(random).gameObject.transform.position, Quaternion.identity);
            transform.GetChild(random).gameObject.GetComponent<Rigidbody>().AddForce(Vector3.right * -5.0f, ForceMode.Impulse);
            transform.GetChild(random).gameObject.GetComponent<Rigidbody>().useGravity = true;

            Debug.Log(transform.GetChild(random) + " sayılı küp hareketlendirildi.");

            elapsedTime = 0f;
        }
    }
}
