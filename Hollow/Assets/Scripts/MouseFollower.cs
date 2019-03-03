using UnityEngine;

public class MouseFollower : MonoBehaviour
{
    [SerializeField] private float zOffset = -10;
    [SerializeField] private float speed = 5f;

    private Vector3 offset = new Vector3();

    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), zOffset) + offset, Time.deltaTime * speed);
    }

    public void StartActive ()
    {
        offset = new Vector3(-1, -1.5f, 0);
    }

    public void StartDeactive ()
    {
        offset = Vector3.zero;
    }
}
