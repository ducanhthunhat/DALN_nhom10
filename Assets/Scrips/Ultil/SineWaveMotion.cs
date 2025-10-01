using UnityEngine;

public class SineWaveMotion : MonoBehaviour
{
    [SerializeField] private float amplitude = 0.4f;
    [SerializeField] private float frequency = 4f;
    [SerializeField] private Vector3 axis = Vector3.up;

    private Vector3 _startLocalPos;

    void Start()
    {
        _startLocalPos = transform.localPosition;
    }


    void Update()
    {
        float offset = Mathf.Sin(Time.time * frequency) * amplitude;
        transform.localPosition = _startLocalPos + axis.normalized * offset;
    }
}
