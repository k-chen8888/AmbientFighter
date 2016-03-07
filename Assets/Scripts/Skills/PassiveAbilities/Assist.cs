using UnityEngine;
using System.Collections;
using System.Collections.Generic;


/* Adds some protection to help you when you're doing poorly
 */
public class Assist : Strip {
    // Called when the STRIP is created
    void Awake()
    {
        base.Init();
    }

    // Action that occurs when this STRIP's conditions are met
    public override void Action(GameObject caller)
    {
        // Check preconditions and state

        // Add the following conditions:
        //  Cannot trigger again for a some amount of time
        // 

        // Perform some action, maybe change state

        // Deferred-Add/Delete conditions from the BlackBoard
    }


    /* Preconditions
     */
    
}
