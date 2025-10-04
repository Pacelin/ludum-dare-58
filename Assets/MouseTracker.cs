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
        Vector3 point = _camera.ScreenToWorldPoint(mousePosition);
        _material.SetVector(MOUSE_POSITION_KEY, point);
    }
}
