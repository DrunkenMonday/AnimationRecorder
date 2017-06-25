using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationRecorderRagdollHelper : MonoBehaviour
{
	[SerializeField] private Rigidbody _rigidBody;
	[SerializeField] private Vector3 _applyingForce;
	[SerializeField] private float _startingDelay;
	[SerializeField] public bool _recordAnimation;

	void Start ()
	{
		StartCoroutine ( Handle () );
	}

	private IEnumerator Handle ()
	{
		yield return new WaitForSeconds ( _startingDelay );

		Animator animator = GetComponent<Animator> ();
		if ( animator != null )
		{
			animator.enabled = false;
		}

		Rigidbody[] rBodies = GetComponentsInChildren<Rigidbody> ();
		for ( int i = 0; i < rBodies.Length; ++i )
		{ 
			rBodies [ i ].isKinematic = false;
		}

		BoxCollider[] bColliders = GetComponentsInChildren<BoxCollider> ();
		for ( int i = 0; i < bColliders.Length; ++i )
		{
			bColliders [ i ].isTrigger = false;
		}

		CapsuleCollider[] cColliders = GetComponentsInChildren<CapsuleCollider> ();
		for ( int i = 0; i < cColliders.Length; ++i )
		{
			cColliders [ i ].isTrigger = false;
		}

		SphereCollider[] sColliders = GetComponentsInChildren<SphereCollider> ();
		for ( int i = 0; i < sColliders.Length; ++i )
		{
			sColliders [ i ].isTrigger = false;
		} 

		_rigidBody.AddForce ( _applyingForce, mode: ForceMode.VelocityChange );

		if ( _recordAnimation )
		{
			gameObject.AddComponent<AnimationRecorder> ().StartRecording ();
		}
	}
}
