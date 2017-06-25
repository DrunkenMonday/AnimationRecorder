using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AnimationRecorderItem
{
	private const float MIN_DIFF_DISTANCE = 0.025f;
	private const float MIN_DIFF_SCALE = 0.025f;
	private const float MIN_DIFF_ANGLE = 5.0f;

	public Dictionary<string,AnimationCurve> Properties { get; private set; }

	public string PropertyName { get; private set; }

	private Transform _animationObject;
	private Vector3? _lastPosition;
	private Vector3? _lastScale;
	private Quaternion? _lastRotation;

	public AnimationRecorderItem ( string propertyName, Transform animatingObject )
	{
		Properties = new Dictionary<string, AnimationCurve> ();
		PropertyName = propertyName;
		_animationObject = animatingObject;

		Properties.Add ( "localPosition.x", new AnimationCurve () );
		Properties.Add ( "localPosition.y", new AnimationCurve () );
		Properties.Add ( "localPosition.z", new AnimationCurve () );

		Properties.Add ( "localRotation.x", new AnimationCurve () );
		Properties.Add ( "localRotation.y", new AnimationCurve () );
		Properties.Add ( "localRotation.z", new AnimationCurve () );
		Properties.Add ( "localRotation.w", new AnimationCurve () );

		Properties.Add ( "localScale.x", new AnimationCurve () );
		Properties.Add ( "localScale.y", new AnimationCurve () );
		Properties.Add ( "localScale.z", new AnimationCurve () );
	}

	public void AddFrame ( float time )
	{
		if ( _lastPosition == null || Vector3.Distance ( (Vector3) _lastPosition, _animationObject.localPosition ) > MIN_DIFF_DISTANCE )
		{
			Properties [ "localPosition.x" ].AddKey ( new Keyframe ( time, _animationObject.localPosition.x, 0.0f, 0.0f ) );
			Properties [ "localPosition.y" ].AddKey ( new Keyframe ( time, _animationObject.localPosition.y, 0.0f, 0.0f ) );
			Properties [ "localPosition.z" ].AddKey ( new Keyframe ( time, _animationObject.localPosition.z, 0.0f, 0.0f ) );
			_lastPosition = _animationObject.localPosition;
		}

		if ( _lastRotation == null || Quaternion.Angle ( (Quaternion) _lastRotation, _animationObject.localRotation ) > MIN_DIFF_ANGLE )
		{
			Properties [ "localRotation.x" ].AddKey ( new Keyframe ( time, _animationObject.localRotation.x, 0.0f, 0.0f ) );
			Properties [ "localRotation.y" ].AddKey ( new Keyframe ( time, _animationObject.localRotation.y, 0.0f, 0.0f ) );
			Properties [ "localRotation.z" ].AddKey ( new Keyframe ( time, _animationObject.localRotation.z, 0.0f, 0.0f ) );
			Properties [ "localRotation.w" ].AddKey ( new Keyframe ( time, _animationObject.localRotation.w, 0.0f, 0.0f ) );
			_lastRotation = _animationObject.localRotation;
		}

		if ( _lastScale == null || Vector3.Distance ( (Vector3) _lastScale, _animationObject.localScale ) > MIN_DIFF_SCALE )
		{
			Properties [ "localScale.x" ].AddKey ( new Keyframe ( time, _animationObject.localScale.x, 0.0f, 0.0f ) );
			Properties [ "localScale.y" ].AddKey ( new Keyframe ( time, _animationObject.localScale.y, 0.0f, 0.0f ) );
			Properties [ "localScale.z" ].AddKey ( new Keyframe ( time, _animationObject.localScale.z, 0.0f, 0.0f ) );
			_lastScale = _animationObject.localScale;
		}
	}
}