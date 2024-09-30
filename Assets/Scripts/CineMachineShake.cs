using Cinemachine;
using UnityEngine;

public class CineMachineShake : MonoBehaviour
{
    private static CineMachineShake instance;
    private CinemachineVirtualCamera cinemachine;

    private CinemachineBasicMultiChannelPerlin cinemachinePerlin;

    private float time_movement;

    private float time_movement_total;

    private float intensity_start;

    public void Awake()
    {
        instance = this;
        cinemachine = GetComponent<CinemachineVirtualCamera>();
        cinemachinePerlin = cinemachine.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }
    public void Move_Camera(float intensity, float frecuency, float time)
    {
        cinemachinePerlin.m_AmplitudeGain = intensity;
        cinemachinePerlin.m_FrequencyGain = frecuency;
        intensity_start = intensity;
        time_movement_total = time;
        time_movement = time;
    }
    // Update is called once per frame
    void Update()
    {
        if(time_movement > 0)
        {
            time_movement -= Time.deltaTime;
            cinemachinePerlin.m_AmplitudeGain = Mathf.Lerp(intensity_start, 0, 1 -(time_movement / time_movement_total));
        }
    }
}
