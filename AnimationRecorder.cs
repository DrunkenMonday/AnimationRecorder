#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class AnimationRecorder : MonoBehaviour
{
	private const float CAPTURING_INTERVAL = 1.0f / 30.0f;

	private float _lastCapturedTime;
	private float _recordingTimer;
	private bool _recording = false;

	private List<AnimationRecorderItem> _recorders;

	void Start ()
	{
		Configurate ();
	}

	void Configurate ()
	{
		_recorders = new List<AnimationRecorderItem> ();
		_recordingTimer = 0.0f;

		var allTransforms = gameObject.GetComponentsInChildren< Transform > ();
		for ( int i = 0; i < allTransforms.Length; ++i )
		{
			string path = CreateRelativePathForObject ( transform, allTransforms [ i ] );
			_recorders.Add ( new AnimationRecorderItem ( path, allTransforms [ i ] ) );
		}
	}

	public void StartRecording ()
	{
		Debug.Log ( "AnimationRecorder recording started" );
		_recording = true;
	}

	public void StopRecording ()
	{
		Debug.Log ( "AnimationRecorder recording stopped" );
		_recording = false;
		ExportAnimationClip ();
		Configurate ();
	}

	void Update ()
	{
		if ( Input.GetKeyDown ( KeyCode.Space ) && !_recording )
		{
			StartRecording ();
			return;
		}

		if ( Input.GetKeyDown ( KeyCode.Space ) && _recording )
		{
			StopRecording ();
			return;
		}

		if ( _recording )
		{
			if ( _recordingTimer == 0.0f || _recordingTimer - _lastCapturedTime >= CAPTURING_INTERVAL )
			{
				for ( int i = 0; i < _recorders.Count; ++i )
				{
					_recorders [ i ].AddFrame ( _recordingTimer );
				}
				_lastCapturedTime = _recordingTimer;
			}
			_recordingTimer += Time.deltaTime;
		}
	}

	private void ExportAnimationClip ()
	{
		AnimationClip clip = new AnimationClip ();
		for ( int i = 0; i < _recorders.Count; ++i )
		{
			Dictionary<string,AnimationCurve> propertiles = _recorders [ i ].Properties;
			for ( int j = 0; j < propertiles.Count; ++j )
			{
				clip.SetCurve ( _recorders [ i ].PropertyName, typeof(Transform), propertiles.ElementAt ( j ).Key, propertiles.ElementAt ( j ).Value );
			}
		}
		clip.EnsureQuaternionContinuity ();

		string path = "Assets/" + gameObject.name + ".anim";
		AssetDatabase.CreateAsset ( clip, path );
		Debug.Log ( "AnimationRecorder saved to = " + path );
	}

	private string CreateRelativePathForObject ( Transform root, Transform target )
	{
		if ( target == root )
		{
			return string.Empty;
		}

		string name = target.name;
		Transform bufferTransform = target;

		while ( bufferTransform.parent != root )
		{
			name = string.Format ( "{0}/{1}", bufferTransform.parent.name, name );
			bufferTransform = bufferTransform.parent;
		}
		return name;
	}
}
#endif