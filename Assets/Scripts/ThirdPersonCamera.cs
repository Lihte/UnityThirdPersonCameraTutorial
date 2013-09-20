using UnityEngine;

using System.Collections;


/// <summary>
/// #DESCRIPTION OF CLASS#
/// </summary>


public class ThirdPersonCamera : MonoBehaviour 
{

	#region Variables (private)
	
	[SerializeField]
	private float distanceAway;
	[SerializeField]
	private float distanceUp;
	[SerializeField]
	private float smooth;
	[SerializeField]
	private Transform follow;
	private Vector3 targetPosition;

	#endregion

	#region Properties (public)

	#endregion

	#region Unity event functions

	/// <summary>
	/// Use this for initialization
	
	/// </summary>
	void Start () 
	{
		follow = GameObject.FindWithTag ("Player").transform;
	}
	
	

	/// <summary>
	/// Update is called once per frame

	/// </summary>	
	void Update () 
	{

		
	
	}


	/// <summary>
	/// Debugging information should be put here
	/// </summary>
	void OnDrawGizmos()
	{

	}
	
	void LateUpdate ()
	{
		// Setting the target position to be the correct offset from the player
		targetPosition = follow.position + follow.up * distanceUp - follow.forward * distanceAway;
		Debug.DrawRay(follow.position, Vector3.up * distanceUp, Color.red);
		Debug.DrawRay(follow.position, -1f * follow.forward * distanceAway, Color.blue);
		Debug.DrawLine(follow.position, targetPosition, Color.magenta);
		
		// Making a smooth transition between its current position and the position it wants to be in
		transform.position = Vector3.Lerp (transform.position, targetPosition, Time.deltaTime * smooth);
		
		// Make sure the camera is looking the right way!
		transform.LookAt(follow);
	}

	#endregion

	#region Methods

	#endregion Methods
}
