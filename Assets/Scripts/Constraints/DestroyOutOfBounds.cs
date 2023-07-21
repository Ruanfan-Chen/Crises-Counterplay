using UnityEngine;

public class DestroyOutOfBounds : MonoBehaviour
{
    private float offset = 0.0f;

    public float GetOffset() { return offset; }

    public void SetOffset(float value) { offset = value; }

    // Update is called once per frame
    void Update()
    {
        if (!MapManager.GetBounds(offset).Contains(transform.position))
            Destroy(gameObject);
    }
}
