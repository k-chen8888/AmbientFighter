using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PlayerMoveController : NetworkBehaviour
{
	public Vector3 startPosition;

	// Use this for initialization
	void Start () {
		transform.Translate(startPosition);
	}
	
	// Update is called once per frame
	void Update () {
		if (!isLocalPlayer)
			return;
		
		var x = Input.GetAxis("Horizontal")*0.1f;
		var z = Input.GetAxis("Vertical")*0.1f;

		transform.Translate(x, 0, z);
	}
}
