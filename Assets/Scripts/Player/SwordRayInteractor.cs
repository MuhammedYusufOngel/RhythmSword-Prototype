using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using TMPro;


public class SwordRayInteractor : XRSimpleInteractable
{
    [SerializeField] GameObject parent;
    [SerializeField] TMP_Text text;
    protected override void OnHoverEntered(HoverEnterEventArgs args)
    {
        base.OnHoverEntered(args);

        if (gameObject.name == "Right")
            Debug.Log("Sað");

        if (gameObject.name == "Left")
            Debug.Log("Sol");

        if (gameObject.name == "Up")
            Debug.Log("Yukarý");

        if (gameObject.name == "Down")
            Debug.Log("Aþaðý");

        Destroy(parent);
    }
}
