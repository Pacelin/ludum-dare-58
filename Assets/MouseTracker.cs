using UnityEngine;

public class MouseTracker : MonoBehaviour
{
    private static readonly int MOUSE_POSITION_KEY = Shader.PropertyToID("_MousePosition");

    [SerializeField] private Camera _camera;
    [SerializeField] private Material _material;

    private void Update()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = 0f;

        Vector3 screenSize = new Vector3(Screen.width, Screen.height);

        Vector3 point = new Vector3(mousePosition.x / screenSize.x, mousePosition.y / screenSize.y, 0f);
        point.x = Mathf.Clamp01(point.x);
        point.y = Mathf.Clamp01(point.y);

        _material.SetVector(MOUSE_POSITION_KEY, point);
    }
}
