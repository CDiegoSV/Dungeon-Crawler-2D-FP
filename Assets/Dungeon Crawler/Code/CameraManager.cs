using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    #region Singleton

    public static CameraManager instance;

    private void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(instance);
        }
        else
        {
            instance = this;
        }
    }

    #endregion

    #region References

    [SerializeField] private CinemachineImpulseSource _cinemachineImpulseSource;
    [SerializeField] private CinemachineVirtualCameraBase _targetGroupCamera;
    [SerializeField] private CinemachineVirtualCameraBase _fakeLakeCamera;

    #endregion

    #region Runtime Variables

    private CinemachineVirtualCameraBase _currentCamera;

    #endregion

    #region Unity Methods

    private void Start()
    {
        _currentCamera = _targetGroupCamera;
    }

    #endregion

    #region Public Methods

    public void ChangeCameraTo(CinemachineVirtualCameraBase nextCamera)
    {
        if(nextCamera != _currentCamera)
        {
            _currentCamera.Priority = 1;
            _currentCamera = nextCamera;
            _currentCamera.Priority = 10;
        }
    }

    public void ChangeCameraToLake()
    {
        ChangeCameraTo(_fakeLakeCamera);
    }

    public void ChangeCameraToPlayers()
    {
        ChangeCameraTo(_targetGroupCamera);
    }

    public void CameraShake()
    {
        _cinemachineImpulseSource.GenerateImpulse(0.5f);
    }

    public void ShakeCameraForSeconds(float seconds)
    {
        StartCoroutine(ShakeCameraForSecondsCoroutine(seconds));
    }

    #endregion

    #region Coroutines

    private IEnumerator ShakeCameraForSecondsCoroutine(float seconds)
    {
        CinemachineVirtualCamera vc = _currentCamera as CinemachineVirtualCamera;
        vc.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = 1;
        vc.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain = 1;

        yield return new WaitForSeconds(seconds);

        vc.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = 0;
        vc.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain = 0;
    }

    #endregion
}
