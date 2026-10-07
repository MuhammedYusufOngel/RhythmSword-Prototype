using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class HapticFeedbackController : MonoBehaviour
{
    public static void TriggerHaptic(IXRInteractor interactor, float amplitude = 0.5f, float duration = 0.1f)
    {
        if (interactor is XRBaseInputInteractor inputInteractor)
        {
            inputInteractor.SendHapticImpulse(amplitude, duration);
        }
    }
}
