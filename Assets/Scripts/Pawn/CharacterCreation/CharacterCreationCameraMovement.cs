using UnityEngine;

public class CharacterCreationCameraMovement : MonoBehaviour
{
    public Transform pivot;
    public Transform cameraTransform;
    public Transform target;

    public bool rotatingCamera = false;
    public bool translatingCamera = false;

    float cameraDistance = 2;

    private void Update()
    {
        if (rotatingCamera)
        {
            target.rotation *= Quaternion.AngleAxis(Input.GetAxis("Mouse X"), Vector3.up);
            pivot.rotation *= Quaternion.AngleAxis(Input.GetAxis("Mouse Y"), Vector3.right);
            float angleToForward = 45-Vector3.Angle(Vector3.forward, pivot.forward);
            if(angleToForward < 0)
            {
                float direction = Mathf.Sign(Input.GetAxis("Mouse Y"));
                pivot.rotation *= Quaternion.AngleAxis(angleToForward * direction, Vector3.right);
            }
        }
        if (translatingCamera)
        {
            pivot.position += Vector3.up * -Input.GetAxis("Mouse Y")/100;
            if (pivot.position.y <= 0) pivot.position = Vector3.zero;
            if (pivot.position.y >= 3) pivot.position = Vector3.up * 3;
        }

        cameraDistance -= Input.mouseScrollDelta.y /10;
        cameraDistance = Mathf.Clamp(cameraDistance, 0.75f, 5);
        cameraTransform.localPosition = new Vector3(0, 0, cameraDistance);

    }
}
