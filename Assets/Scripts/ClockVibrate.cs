using UnityEngine;

public class ClockVibrate : MonoBehaviour
{
    public bool canVibrate = false;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float intensity;
    [SerializeField] private float rotSpeed = 8f;
    [SerializeField] private float rotIntensity = 10f; // degrees
    private Quaternion _startRot;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _startRot = transform.localRotation;
    }

    void Update()
    {
        if(canVibrate)
        {
            float t = Time.time;

            // transform.localPosition = intensity * new Vector3(
            //     Mathf.PerlinNoise(speed * t, 1),
            //     Mathf.PerlinNoise(speed * t, 2),
            //     Mathf.PerlinNoise(speed * t, 3));

            float rotNoise = Mathf.PerlinNoise(rotSpeed * t, 4f);
            float angle = (rotNoise - 0.5f) * 2f * rotIntensity;

            Quaternion rotOffset = Quaternion.Euler(0f, 0f, angle);
            transform.localRotation = _startRot * rotOffset;
        }
    }
}
