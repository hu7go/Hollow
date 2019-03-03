using UnityEngine;

public class Portal : MonoBehaviour
{
    public void Close ()
    {
        GetComponent<Animator>().SetBool("close", true);
        Destroy(gameObject, 1);
    }
}
