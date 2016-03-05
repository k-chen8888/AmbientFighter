using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour {
	// Information about the player
	struct PlayerState {
		public float x;
		public float z;
	}


	/* Fields */

	// Controls
	string[] moveCommands = { "Horizontal", "Vertical" };
	string[] combatCommands = { "Basic", "Strong", "Evade", "Grab", "Combo" };

	// Information about the player, synchronized across all clients
	[SyncVar]
	private PlayerState state;


	/* Instantiation through MonoBehaviour */

	void Awake () {
		InitState ();
	}
	
	// Update is called once per frame
	void Update () {
		if (isLocalPlayer) {
			foreach (string input in moveCommands) {
				CmdMove (input);
			}
		}

		SyncState ();
	}


	/* Helper methods for playing the game */

	// Set the initial state of the player
	[Server]
	void InitState () {
		state = new PlayerState {
			x = 0.0f,
			z = 0.0f
		};
	}

	// Handles movement on the server
	[Command]
	void CmdMove(string movement)
	{
		float deltaX = 0.0f,
			  deltaZ = 0.0f;

		switch (movement) {
			case "Horizontal":
				deltaX = Input.GetAxis (movement);
				break;

			case "Vertical":
				deltaZ = Input.GetAxis (movement);
				break;

			default:
				break;
		}

		state = new PlayerState {
			x = deltaX + state.x,
			z = deltaZ + state.z
		};
	}

	// Synchronizes state across clients
	void SyncState()
	{
		transform.position = new Vector3 (state.x, 0, state.z);
	}
}
