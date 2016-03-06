using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour {
	// Information about the player
	struct PlayerState {
		public int moveNum;
		public int x;
		public int z;
	}


	/* Fields */
	[SyncVar(hook="OnServerStateChanged")] PlayerState serverState;
	// Controls
	private enum MoveInfo { UD, LR };
	private enum FightInfo { BASIC, STRONG, EVADE, GRAB, COMBO };
	public string[] moveCommands = { "Horizontal", "Vertical" };
	public string[] combatCommands = { "Basic", "Strong", "Evade", "Grab", "Combo" };

	// Information about the player, synchronized across all clients
	[SyncVar] PlayerState state;

	// Queue up predicted states to reduce lag
	private Queue<KeyValuePair<int, int>> pendingMoves = new Queue<KeyValuePair<int, int>>();
	private PlayerState predictedState;


	/* Instantiation through MonoBehaviour */

	void Awake () {
		InitState ();
	}

	void Start()
	{
		if (isLocalPlayer) {
			pendingMoves = new Queue<KeyValuePair<int, int>> ();
			UpdatePredictedState ();
		}
		SyncState (true);
	}
	
	// Update is called once per frame
	void Update () {
		if (isLocalPlayer) {
			for (int input = 0; input < moveCommands.Length; input++) {
				float val = Input.GetAxis (moveCommands[input]);
				if (val > 0) {
					pendingMoves.Enqueue (new KeyValuePair<int, int> (input, 1));
					UpdatePredictedState ();
					CmdMove (input, 1);
				} else if (val < 0) {
					pendingMoves.Enqueue (new KeyValuePair<int, int> (input, -1));
					UpdatePredictedState ();
					CmdMove (input, -1);
				}
			}
		}
			
		SyncState (false);
	}


	/* Helper methods for playing the game */

	// Handles movement on the server
	[Command] void CmdMove(int movement, int val)
	{
		state = Move (state, movement, val);
	}

	PlayerState Move(PlayerState curr, int movement, int val)
	{
		int deltaX = 0,
		deltaZ = 0;

		switch (movement) {
			case (int)MoveInfo.UD:
				deltaX = val;
				break;

			case (int)MoveInfo.LR:
				deltaZ = val;
				break;

			default:
				break;
		}

		int newX = deltaX + curr.x;
		int newZ = deltaZ + curr.z;

		return new PlayerState {
			//x = deltaX + curr.x,
			//z = deltaZ + curr.z
			//checks newZ to see if it is greater than 0 and less than the maximum
			moveNum = 1 + curr.moveNum,
			x = newX < 0 ? 0 : newX >= maxX ? (maxX - 1) : newX,
			z = newZ < 0 ? 0 : newZ >= maxZ ? (maxZ - 1) : newZ
		};
	}
	
	// Set the initial state of the player
	[Server] void InitState () {
		state = new PlayerState {
			x = 0,
			z = 0
		};
	}

	//
	void OnServerStateChanged (PlayerState newState) {
		serverState = newState;
		if (pendingMoves != null) {
			while (pendingMoves.Count > (predictedState.moveNum - serverState.moveNum)) {
				pendingMoves.Dequeue ();
			}
			UpdatePredictedState ();
		}
	}

	// Synchronizes state across clients
	void SyncState(bool init)
	{
		PlayerState render = isLocalPlayer ? predictedState : state;
		Vector3 target = spacing * (render.x * Vector3.right + render.z * Vector3.forward);
		transform.position = new Vector3 (render.x, 0, render.z);
		transform.position = init ? target : Vector3.Lerp (transform.position, target, easing);
	}

	// Update the state prediction
	void UpdatePredictedState ()
	{
		predictedState = state;
		foreach (KeyValuePair<int, int> pending in pendingMoves) {
			predictedState = Move (predictedState, pending.Key, pending.Value);
		}
	}

	int maxX = 5;
	int maxZ = 5;
	float easing = 0.1f;
	float spacing = 1.0f;
}