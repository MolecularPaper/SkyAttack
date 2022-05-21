using UnityEngine.Tilemaps;
using UnityEngine;
using Cinemachine;

[RequireComponent(typeof(CinemachineVirtualCamera))]
public class CameraInfo : MonoBehaviour
{
    [SerializeField] protected Vector2 xRange;
    [SerializeField] protected Vector2 yRange;
}

public class CameraCTRL : CameraInfo
{
    public void LateUpdate()
    {
        CameraPostionClamped();
    }

    private void CameraPostionClamped()
    {
        float xClamp = Mathf.Clamp(transform.position.x, xRange.x, xRange.y);
        float yClamp = Mathf.Clamp(transform.position.y, yRange.x, yRange.y);
        transform.position = new Vector3(xClamp, yClamp, transform.position.z);
    }
}
