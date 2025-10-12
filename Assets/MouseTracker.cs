using UnityEngine;

public class MouseTracker : MonoBehaviour
{
    private static readonly int MOUSE_POSITION_KEY = Shader.PropertyToID("_MousePosition");

    [SerializeField] private Camera _camera;
    [SerializeField] private Material[] _materials;

    private void Update()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = 0f;
        mousePosition.y = Screen.height - mousePosition.y;
        //Vector3 point = _camera.ScreenToViewportPoint(mousePosition);

        foreach (Material material in _materials)
            material.SetVector(MOUSE_POSITION_KEY, mousePosition);
    }
}
