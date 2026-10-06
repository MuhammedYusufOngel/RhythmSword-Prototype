using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;


public class SwordRayInteractor : XRSimpleInteractable
{
    [SerializeField] GameObject parent;
    [SerializeField] GameObject cubeBroken;

    protected override void OnHoverEntered(HoverEnterEventArgs args)
    {
        base.OnHoverEntered(args);

        string interactorName = args.interactorObject.transform.name;

        if (interactorName.Contains("Left") && parent.GetComponent<Note>().GetColor() == COLOR.RED)
        {
            PlayerManager.instance.DebugLogLong(gameObject.name);

            if (gameObject.name == parent.GetComponent<Note>().GetDirection().ToString())
            {
                PlayerManager.instance.AddScore(10);
                Debug.Log("10-points");
                PlayerManager.instance.DebugLog("NP::");
            }
            else
            {
                Debug.Log("Wrong direction::");
                PlayerManager.instance.DebugLog("Wrong direction::");
            }
        }
        else if(interactorName.Contains("Right") && parent.GetComponent<Note>().GetColor() == COLOR.BLUE)
        {
            PlayerManager.instance.DebugLogLong(gameObject.name);

            if (gameObject.name == parent.GetComponent<Note>().GetDirection().ToString())
            {
                PlayerManager.instance.AddScore(10);
                Debug.Log("10-points");
                PlayerManager.instance.DebugLog("NP::");
            }
            else
            {
                Debug.Log("Wrong direction::");
                PlayerManager.instance.DebugLog("Wrong direction::");
            }
        }
        else
        {
            Debug.Log("Wrong color::");
            PlayerManager.instance.DebugLog("Wrong color::");
        }

        if(gameObject.name == "LEFT" || gameObject.name == "RIGHT")
        {
            GameObject newCubeBroken = Instantiate(cubeBroken, parent.transform.position, Quaternion.Euler(0f, 0f, 90f));
            newCubeBroken.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material.color = parent.GetComponent<MeshRenderer>().material.color;
            newCubeBroken.transform.GetChild(1).gameObject.GetComponent<MeshRenderer>().material.color = parent.GetComponent<MeshRenderer>().material.color;
            Destroy(newCubeBroken, 0.5f);
        }

        else if (gameObject.name == "UP" || gameObject.name == "DOWN")
        {
            GameObject newCubeBroken = Instantiate(cubeBroken, parent.transform.position, Quaternion.Euler(0f, 0f, 0f));
            newCubeBroken.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material.color = parent.GetComponent<MeshRenderer>().material.color;
            newCubeBroken.transform.GetChild(1).gameObject.GetComponent<MeshRenderer>().material.color = parent.GetComponent<MeshRenderer>().material.color;
            Destroy(newCubeBroken, 0.5f);
        }

        Destroy(parent);
    }
}
