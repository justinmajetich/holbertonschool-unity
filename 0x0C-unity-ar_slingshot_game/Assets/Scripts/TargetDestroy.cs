using UnityEngine;

public class TargetDestroy : MonoBehaviour
{
    public GameObject shatterFX;
    [SerializeField] Transform m_Camera;

    private void Start() {
        m_Camera = GameObject.Find("AR Camera").transform;
    }

    private void OnDestroy() {
        Instantiate(shatterFX, transform.position, Quaternion.Euler(0f, m_Camera.localRotation.eulerAngles.y, 0f));
    }
}
