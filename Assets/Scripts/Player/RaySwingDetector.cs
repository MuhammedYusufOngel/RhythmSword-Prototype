using UnityEngine;
using TMPro;


public enum SwingDirection
{
    None,
    Up,
    Down,
    Left,
    Right,
    Forward,
    Backward
}

public class RaySwingDetector : MonoBehaviour
{
    [SerializeField] private float minSwingSpeed = 1.8f;

    [SerializeField] private float detectionCooldown = 0.35f;

    [SerializeField] private bool useLocalToCamera = true;

    [SerializeField] private Transform controller;
    [SerializeField] TMP_Text text;

    private Vector3 lastPosition;
    private float nextDetectionTime;

    void Start()
    {
        lastPosition = controller.position;
    }

    void Update()
    {
        if (Time.deltaTime <= 0f) return;

        Vector3 deltaPosition = controller.position - lastPosition;
        Vector3 velocity = deltaPosition / Time.deltaTime;
        float currentSpeed = velocity.magnitude;

        if (currentSpeed >= minSwingSpeed && Time.time >= nextDetectionTime)
        {
            Vector3 swingDirection = velocity.normalized;
            SwingDirection detected = EvaluateDirection(swingDirection);

            if (detected != SwingDirection.None)
            {
                OnSwingDetected(detected, currentSpeed);
                nextDetectionTime = Time.time + detectionCooldown;
            }
        }

        lastPosition = controller.position;
    }

    private SwingDirection EvaluateDirection(Vector3 direction)
    {
        Transform refTransform = (useLocalToCamera && Camera.main != null) ? Camera.main.transform : null;

        Vector3 upVector = refTransform != null ? refTransform.up : Vector3.up;
        Vector3 rightVector = refTransform != null ? refTransform.right : Vector3.right;
        Vector3 forwardVector = refTransform != null ? refTransform.forward : Vector3.forward;

        float dotUp = Vector3.Dot(direction, upVector);
        float dotRight = Vector3.Dot(direction, rightVector);
        float dotForward = Vector3.Dot(direction, forwardVector);

        float maxDot = Mathf.Max(Mathf.Abs(dotUp), Mathf.Abs(dotRight), Mathf.Abs(dotForward));

        // Belirgin bir ana eksen yönü yoksa (çapraz ve kararsýzsa) pas geç
        if (maxDot < 0.65f) return SwingDirection.None;

        if (maxDot == Mathf.Abs(dotUp))
            return dotUp > 0 ? SwingDirection.Up : SwingDirection.Down;

        if (maxDot == Mathf.Abs(dotRight))
            return dotRight > 0 ? SwingDirection.Right : SwingDirection.Left;

        return dotForward > 0 ? SwingDirection.Forward : SwingDirection.Backward;
    }

    private void OnSwingDetected(SwingDirection direction, float speed)
    {
        //Debug.Log($"Sallama Algýlandý: {direction} (Hýz: {speed:F2} m/s)");

        // Kendi mekaniklerini yönlere göre buradan tetikleyebilirsin
        switch (direction)
        {
            case SwingDirection.Up:
                text.text = "Yukarý doðru sallandý!";
                Debug.Log("Yukarý doðru sallandý!");
                break;
            case SwingDirection.Down:
                text.text = "Aþaðý doðru sallandý!";
                Debug.Log("Aþaðý doðru sallandý!");
                break;
            case SwingDirection.Left:
                text.text = "Sola doðru sallandý!";
                Debug.Log("Sola sallandý!");
                break;
            case SwingDirection.Right:
                text.text = "Saða doðru sallandý!";
                Debug.Log("Saða sallandý!");
                break;
        }
    }
}