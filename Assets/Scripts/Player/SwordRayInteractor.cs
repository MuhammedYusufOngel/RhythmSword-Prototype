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
            if (gameObject.name == parent.GetComponent<Note>().GetDirection().ToString())
            {
                PlayerManager.instance.AddScore(parent.GetComponent<Note>().GetScore());
                Debug.Log("10-points");
            }
            else
            {
                Debug.Log("Wrong direction::");
            }
        }
        else if(interactorName.Contains("Right") && parent.GetComponent<Note>().GetColor() == COLOR.BLUE)
        {
            if (gameObject.name == parent.GetComponent<Note>().GetDirection().ToString())
            {
                PlayerManager.instance.AddScore(parent.GetComponent<Note>().GetScore());
                Debug.Log("10-points");
            }
            else
            {
                Debug.Log("Wrong direction::");
            }
        }
        else
        {
            Debug.Log("Wrong color::");
        }

        Destroy(parent);
    }
}
