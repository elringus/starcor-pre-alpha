using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
	#region SINGLETON
	private static CameraController _instance;
	public static CameraController I
	{
		get
		{
			if (_instance == null) _instance = FindObjectOfType(typeof(CameraController)) as CameraController;
			return _instance;
		}
	}
	private void OnApplicationQuit () { _instance = null; }
	#endregion

	public Transform Target;
	public float XSpeed;
	public float YSpeed;
	public float ZoomSpeed;
	public float MoveSpeed;

	public float YMinLimit;
	public float YMaxLimit;

	public Vector2 FrameLimit;

	public float MinDistance;
	public float MaxDistance;
	public float DefaultDistance;

	public float EasingAmount;

	public float ShakeAmount;

	private Transform myTransform;
	private float distance;
	private float x;
	private float y;
	private float prevPinch;
	private Vector2 prevTouchPos;
	private Vector3 targetOffset;
	private float doubleTapTimer;
	private Transform innerCamera;
	private float shakeFactor;

	private void Awake ()
	{
		// lock fps to prevent battery wasting
		//Application.targetFrameRate = 60;

		myTransform = transform.parent;
		innerCamera = transform;
		distance = DefaultDistance;
	}

	private void Start ()
	{
		Vector3 angles = myTransform.eulerAngles;
		x = angles.y;
		y = angles.x;
	}

	private void Update ()
	{
		if (ShakeAmount > 0)
		{
			innerCamera.localPosition = Random.insideUnitSphere * shakeFactor;
			innerCamera.localPosition = new Vector3(innerCamera.localPosition.x, 0, innerCamera.localPosition.z);
			ShakeAmount -= Time.deltaTime * 1.5f;
		}
		else ShakeAmount = 0;
	}

	private void LateUpdate ()
	{
		if (!Target) return;

		bool inputting = ColliderRelay.IsInputting();

		#region MOUSE_INPUT
		if (Application.isEditor)
		{
			if (Input.GetMouseButton(1) && !inputting)
			{
				if (!dfGUIManager.HitTestAll(Input.mousePosition))
				{
					targetOffset = new Vector3(targetOffset.x + Input.GetAxis("Mouse X"), 0, targetOffset.z + Input.GetAxis("Mouse Y"));
					targetOffset = new Vector3(Mathf.Clamp(targetOffset.x, -FrameLimit.x * Camera.main.orthographicSize / 10, FrameLimit.x * Camera.main.orthographicSize / 10), 0,
						Mathf.Clamp(targetOffset.z, -FrameLimit.y * Camera.main.orthographicSize / 10, FrameLimit.y * Camera.main.orthographicSize / 10));
				}
			}

			if (Input.GetMouseButtonDown(1) && !inputting) DoubleTapHandler();

			distance = Mathf.Clamp(distance - Input.GetAxis("Mouse ScrollWheel") * ZoomSpeed, MinDistance, MaxDistance);
		}
		#endregion

		#region TOUCH_INPUT
		//if (!Application.isEditor)
		//{
		// swipe moving
		//if (Input.touchCount == 1)
		//{
		//	Touch touch = Input.GetTouch(0);
		//	if (touch.phase == TouchPhase.Moved)
		//	{
		//		if (prevTouchPos == Vector2.zero) prevTouchPos = touch.position;
		//		x += (touch.position.x - prevTouchPos.x) * XSpeed / 10 * distance;
		//		y -= (touch.position.y - prevTouchPos.y) * YSpeed / 10;
		//		prevTouchPos = touch.position;
		//	}
		//	else prevTouchPos = Vector2.zero;
		//}
		else
		{
			if (Input.touchCount == 1 && !inputting)
			{
				Touch touch = Input.GetTouch(0);

				if (touch.phase == TouchPhase.Ended) DoubleTapHandler();

				if (touch.phase == TouchPhase.Moved && !dfGUIManager.HitTestAll(touch.rawPosition))
				{
					targetOffset = new Vector3(targetOffset.x - touch.deltaPosition.x / MoveSpeed, 0, targetOffset.z - touch.deltaPosition.y / MoveSpeed);
					targetOffset = new Vector3(Mathf.Clamp(targetOffset.x, -FrameLimit.x * Camera.main.orthographicSize / 10, FrameLimit.x * Camera.main.orthographicSize / 10), 0,
						Mathf.Clamp(targetOffset.z, -FrameLimit.y * Camera.main.orthographicSize / 10, FrameLimit.y * Camera.main.orthographicSize / 10));
				}
			}
			else if (Input.touchCount == 2 && !inputting)
			{
				Touch touch1 = Input.GetTouch(0), touch2 = Input.GetTouch(1);
				if (touch1.phase == TouchPhase.Moved && touch2.phase == TouchPhase.Moved)
				{
					float pinch = Vector2.Distance(touch1.position, touch2.position);
					if (prevPinch == 0) prevPinch = pinch;

					if (prevPinch != pinch)
					{
						distance -= (pinch - prevPinch) * ZoomSpeed / 100;
						distance = Mathf.Clamp(distance, MinDistance, MaxDistance);
					}

					prevPinch = pinch;
				}
				else prevPinch = 0;
			}
		}
		#endregion

		y = ClampAngle(y, YMinLimit, YMaxLimit);
		Quaternion rotation = Quaternion.Euler(y, x, 0);

		Vector3 negDistance = new Vector3(0, 0, -10);
		Vector3 position = Vector3.Lerp(myTransform.position, rotation * negDistance + Target.position + targetOffset, Time.deltaTime * EasingAmount);

		Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, distance, Time.deltaTime * EasingAmount);

		myTransform.rotation = rotation;
		myTransform.position = position;
	}

	private float ClampAngle (float angle, float min, float max)
	{
		if (angle < -360F) angle += 360F;
		if (angle > 360F) angle -= 360F;
		return Mathf.Clamp(angle, min, max);
	}

	public void Shake (float amount, float length)
	{
		ShakeAmount = length;
		shakeFactor = amount;
	}

	private void DoubleTapHandler ()
	{
		if (Time.time < doubleTapTimer + .3f)
		{
			targetOffset = Vector3.zero;
			distance = DefaultDistance;
		}
		doubleTapTimer = Time.time;
	}
}