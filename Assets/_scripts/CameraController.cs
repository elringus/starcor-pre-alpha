using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
	public Transform Target;
	public float XSpeed;
	public float YSpeed;
	public float ZoomSpeed;
	public float MoveSpeed;

	public float YMinLimit;
	public float YMaxLimit;

	public float MinDistance;
	public float MaxDistance;

	public float EasingAmount;

	private Transform myTransform;
	private float distance;
	private float x;
	private float y;
	private float prevPinch;
	private Vector2 prevTouchPos;
	private Vector3 targetOffset;

	private float doubleTapTimer;

	private void Awake ()
	{
		// lock fps to prevent battery wasting
		//Application.targetFrameRate = 60;

		myTransform = transform;
		distance = MaxDistance;
	}

	private void Start ()
	{
		Vector3 angles = myTransform.eulerAngles;
		x = angles.y;
		y = angles.x;
	}

	private void LateUpdate ()
	{
		if (Target)
		{
			#region MOUSE_INPUT
			if (Application.isEditor)
			{
				if (Input.GetMouseButton(1))
				{
					Screen.lockCursor = true;
					x += Input.GetAxis("Mouse X") * XSpeed * distance;
					y -= Input.GetAxis("Mouse Y") * YSpeed;
				}
				else Screen.lockCursor = false;

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
			if (Input.touchCount == 1)
			{
				if (Time.time < doubleTapTimer + .3f)
				{
					targetOffset = Vector3.zero;
				}
				doubleTapTimer = Time.time;
			}
			if (Input.touchCount == 2)
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
			else if (Input.touchCount > 2)
			{
				Touch touch1 = Input.GetTouch(0), touch2 = Input.GetTouch(1);
				targetOffset = new Vector3(targetOffset.x - touch1.deltaPosition.x / MoveSpeed, 0, targetOffset.z - touch1.deltaPosition.y / MoveSpeed);
				targetOffset = new Vector3(Mathf.Clamp(targetOffset.x, -20 * myTransform.position.y / 30, 20 * myTransform.position.y / 30), 0, Mathf.Clamp(targetOffset.z, -15 * myTransform.position.y / 30, 15 * myTransform.position.y / 30));
			}
			#endregion

			y = ClampAngle(y, YMinLimit, YMaxLimit);
			Quaternion rotation = Quaternion.Euler(y, x, 0);

			Vector3 negDistance = new Vector3(0, 0, -distance);
			Vector3 position = Vector3.Lerp(myTransform.position, rotation * negDistance + Target.position + targetOffset, Time.deltaTime * EasingAmount);

			myTransform.rotation = rotation;
			myTransform.position = position;
		}
	}

	private float ClampAngle (float angle, float min, float max)
	{
		if (angle < -360F) angle += 360F;
		if (angle > 360F) angle -= 360F;
		return Mathf.Clamp(angle, min, max);
	}
}