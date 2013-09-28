using UnityEngine;

using System.Collections;


/// <summary>
/// #DESCRIPTION OF CLASS#
/// </summary>


public class ThirdPersonCamera : MonoBehaviour 
{

	#region Variables (private)
	
    // Inspector serialized
	[SerializeField]
	private float distanceAway;
	[SerializeField]
	private float distanceUp;
	[SerializeField]
	private float smooth;
	[SerializeField]
	private Transform followXForm;
    [SerializeField]
    private float targetingTime = 0.5f;

    // Smoothing and damping
    private Vector3 velocityCamSmooth = Vector3.zero;
    [SerializeField]
    private float camSmoothDampTime = 0.1f;

    //Private global only
    private Vector3 lookDir;
    private Vector3 targetPosition;
    private CamStates camState = CamStates.Behind;

	#endregion

	#region Properties (public)

    public enum CamStates
    {
        Behind,
        FirstPerson,
        Target,
        Free
    }

	#endregion

	#region Unity event functions

	/// <summary>
	/// Use this for initialization
	
	/// </summary>
	void Start () 
	{
		followXForm = GameObject.FindWithTag ("Player").transform;
        lookDir = followXForm.forward;
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
        // Debug.DrawRay(follow.position, Vector3.up * distanceUp, Color.red);
        // Debug.DrawRay(follow.position, -1f * follow.forward * distanceAway, Color.blue);
        Debug.DrawLine(followXForm.position, targetPosition, Color.magenta);
	}
	
	void LateUpdate ()
	{
        Vector3 characterOffset = followXForm.position + new Vector3(0f, distanceUp, 0f);

        // Determine camera state
        if (Input.GetAxis("Target") > 0.01f)
        {
            camState = CamStates.Target;
        }
        else
        {
            camState = CamStates.Behind;
        }

        // Execute camera state
        switch (camState)
        {
            case CamStates.Behind:
                // Calculate direction from camera to player, kill Y, and normalize to give a valid direction with unit magnitude
                lookDir = characterOffset - this.transform.position;
                lookDir.y = 0;
                lookDir.Normalize();
                Debug.DrawRay(this.transform.position, lookDir, Color.white);

		        // Setting the target position to be the correct offset from the player
                targetPosition = characterOffset + followXForm.up * distanceUp - lookDir * distanceAway;
                break;
            case CamStates.Target:
                lookDir = followXForm.forward;

                targetPosition = characterOffset + followXForm.up * distanceUp - followXForm.forward * distanceAway;
                break;
        }

        CompensateForWalls(characterOffset, ref targetPosition);

		// Making a smooth transition between its current position and the position it wants to be in
		smoothPosition(this.transform.position, targetPosition);
		
		// Make sure the camera is looking the right way!
		transform.LookAt(characterOffset);
	}

	#endregion

	#region Methods

    private void smoothPosition(Vector3 fromPos, Vector3 toPos)
    {
        // Making a smooth transition between camera's current position and the position it wants to be in
        this.transform.position = Vector3.SmoothDamp(fromPos, toPos, ref velocityCamSmooth, camSmoothDampTime);
    }

    private void CompensateForWalls(Vector3 fromObject, ref Vector3 toTarget)
    {

        Debug.DrawLine(fromObject, toTarget, Color.cyan);
        // Compensate for walls between camera
        RaycastHit wallHit = new RaycastHit();

        if (Physics.Linecast(fromObject, toTarget, out wallHit))
        {
            Debug.DrawRay(wallHit.point, Vector3.left, Color.red);
            toTarget = new Vector3(wallHit.point.x, toTarget.y, wallHit.point.z);
        }
    }

	#endregion Methods
}
