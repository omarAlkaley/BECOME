using System.Collections;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance;

    [SerializeField] private CinemachineCamera[] _allVirtualCameras;

    [Header("Controls for lerping the y Daming during player jump/fall")]
    [SerializeField] private float _fallPanAmount = 0.25f;
	[SerializeField] private float _fallYPanTime = 0.35f;
	public float _fallSpeedYDampingChangeThreshold = -0.15f;

    public bool IsLerpingYDamping {  get; private set; }
    public bool LerpedFromPlayerFalling {  get; set; }

    private Coroutine _lerpYPanCoroutine;
    private Coroutine _panCameraCoroutine;
	private CinemachineCamera _currentActiveCamera;
	private CinemachinePositionComposer positionComposer;

    private float _normYPanAmount;

    private Vector2 _startingTrackedObjectOffset;

	void Awake()
	{
        if(instance == null)
        {
            instance = this;
        }
		else
		{
			Destroy(gameObject);
		}

		for (int i = 0; i < _allVirtualCameras.Length; i++) 
        {
            if (_allVirtualCameras[i].enabled)
            {
                _currentActiveCamera = _allVirtualCameras[i];

                positionComposer = _currentActiveCamera.GetComponent<CinemachinePositionComposer>();
			}
        }

        _normYPanAmount = positionComposer.Damping.y;

		_startingTrackedObjectOffset = positionComposer.TargetOffset;
	}

	#region Lerping the Y Damping
	public void LerpYDamping(bool isPlayerFalling )
    {
        _lerpYPanCoroutine = StartCoroutine(LerpYAction(isPlayerFalling));
    }

    private IEnumerator LerpYAction(bool isPlayerFalling )
    {
        IsLerpingYDamping = true;
        //control the damping amount
        float startDampingAmount = positionComposer.Damping.y;
        float endDmapingAmount = 0f;

        if (isPlayerFalling)
        {
            endDmapingAmount = _fallPanAmount;
			LerpedFromPlayerFalling = true;
		}
		else
        {
			endDmapingAmount = _normYPanAmount;
		}

        float elapsedTime = 0f;
        while (elapsedTime < _fallYPanTime)
        {
            elapsedTime += Time.deltaTime;
            float lerpedPanAmount = Mathf.Lerp(startDampingAmount, endDmapingAmount, (elapsedTime/ _fallYPanTime));
			positionComposer.Damping.y = lerpedPanAmount;
			yield return null;
        }
		IsLerpingYDamping = false;
    }
	#endregion

	#region Pan Camera
	public void PanCameraOnContact(float panDistance, float panTime, PanDirection panDirection ,bool panToStartingPos )
    {
        _panCameraCoroutine = StartCoroutine(PanCamera(panDistance , panTime , panDirection , panToStartingPos));
    }
    private IEnumerator PanCamera( float panDistance , float panTime , PanDirection panDirection, bool panToStartingPos )
    {
        Vector2 endPos = Vector2.zero;
        Vector2 startingPos = Vector2.zero;

        if (!panToStartingPos)
        {
            switch (panDirection)
            {
                case PanDirection.Up:
                    endPos = Vector2.up;
                    break;
                case PanDirection.Down:
					endPos = Vector2.down;
					break;
                case PanDirection.Left:
                    endPos = Vector2.left;
                    break;
                case PanDirection.Right:
                    endPos = Vector2.right;
                    break;
                default:
                    break;
            }
            endPos *= panDistance;
            startingPos = _startingTrackedObjectOffset;
            endPos += startingPos;
        }
        else
        {
            startingPos = positionComposer.TargetOffset;
            endPos = _startingTrackedObjectOffset;
        }

        float elapsedTime = 0f;
        while (elapsedTime < panTime)
        {
            elapsedTime += Time.deltaTime;
            Vector3 panLerp = Vector3.Lerp(startingPos, endPos, (elapsedTime/panTime));
            positionComposer.TargetOffset = panLerp;

            yield return null;
        }
    }
	#endregion

	#region Swap Cameras
    public void SwapCamera(CinemachineCamera cameraFromLeft, CinemachineCamera cameraFromRight, Vector2 triggerExitDirection)
    {
        if(_currentActiveCamera == cameraFromLeft && triggerExitDirection.x > 0)
        {
            cameraFromRight.enabled = true;
            cameraFromLeft.enabled = false;

            _currentActiveCamera = cameraFromRight;

            positionComposer = _currentActiveCamera.GetComponent<CinemachinePositionComposer>();
        }
        else if(_currentActiveCamera == cameraFromRight && triggerExitDirection.x < 0) 
        {
			cameraFromLeft.enabled = true;
			cameraFromRight.enabled = false;

			_currentActiveCamera = cameraFromLeft;

			positionComposer = _currentActiveCamera.GetComponent<CinemachinePositionComposer>();
		}
    }
	#endregion
}
