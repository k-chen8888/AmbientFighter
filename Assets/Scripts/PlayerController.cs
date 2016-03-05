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
	private enum MoveInfo {UD, LR};
	private enum FightInfo { BASIC, STRONG, EVADE, GRAB, COMBO };
	public string[] moveCommands = { "Horizontal", "Vertical" };
	public string[] combatCommands = { "Basic", "Strong", "Evade", "Grab", "Combo" };

	// Information about the player, synchronized across all clients
	[SyncVar] PlayerState state;


	/* Instantiation through MonoBehaviour */

	void Awake () {
		InitState ();
	}

	void Start()
	{
		SyncState ();
	}
	
	// Update is called once per frame
	void Update () {
		if (isLocalPlayer) {
			foreach (string input in moveCommands) {
				float val = Input.GetAxis (input);
				CmdMove (input, val);
			}
		}

		SyncState ();
	}


	/* Helper methods for playing the game */

	// Handles movement on the server
	[Command] void CmdMove(string movement, float val)
	{
		state = Move (state, movement, val);
	}
	
	// Set the initial state of the player
	[Server] void InitState () {
		state = new PlayerState {
			x = 0.0f,
			z = 0.0f
		};
	}

	PlayerState Move(PlayerState curr, string movement, float val)
	{
		float deltaX = 0.0f,
			  deltaZ = 0.0f;

		switch (movement) {
			case "Horizontal":
				deltaX = val;
				break;

			case "Vertical":
				deltaZ = val;
				break;

			default:
				break;
		}

		return new PlayerState {
			x = deltaX + curr.x,
			z = deltaZ + curr.z
		};
	}

	// Synchronizes state across clients
	void SyncState()
	{
		transform.position = new Vector3 (state.x, 0, state.z);
	}
}
