using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PlayerMoveController : NetworkBehaviour
{
	public Vector3 startPosition;

    // Movement
    [SyncVar]
    protected float deltaX = 0.0f,
                    deltaZ = 0.0f;
    
    // Use this for initialization
    public override void OnStartLocalPlayer()
    {
        transform.Translate(startPosition);
        GetComponent<MeshRenderer>().material.color = Color.red;
    }

    // Update is called once per frame
    void Update () {
        if (isLocalPlayer)
        {
            deltaX = Input.GetAxis("Horizontal") * 0.1f;
            deltaZ = Input.GetAxis("Vertical") * 0.1f;
        }

		transform.Translate(deltaX, 0, deltaZ);
	}
}
