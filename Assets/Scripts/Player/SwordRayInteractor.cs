using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;


public class SwordRayInteractor : XRSimpleInteractable
{
    [SerializeField] GameObject parent;
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

        Destroy(parent);
    }
}
