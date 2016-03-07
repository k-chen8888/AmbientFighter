using UnityEngine;
using System.Collections;

public class TreeEditor : MonoBehaviour {

    // Dragging and dropping skill tree nodes
    private Vector3 screenPoint;
    private Vector3 offset;
    public string nodeTag = "Node";
    private GameObject moving;

    // Objects past this point (y > line) are "in the tree" (and y < line is "not in the tree")
    public int skillBankLine = -5;

    // Make a line to show that there is a link
    // Otherwise, reset the object to where it started
    private Vector3 startPos;
    private bool drawLine = false;


    // Use this for initialization
    void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

    
    /* Override mouse clicks to move skill tree nodes when they are clicked on
     */
    void OnMouseDown()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100))
        {
            moving = (hit.transform.gameObject.tag == nodeTag ? hit.transform.gameObject : null);
            if (moving != null)
                startPos = moving.transform.position;
        }

        screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
        offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
    }

    void OnMouseDrag()
    {
        if (moving != null)
        {
            Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);

            Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
            moving.transform.position = curPosition;
        }
    }

    void OnMouseUp()
    {
        moving = null;
    }


    /* Draw lines between skill tree nodes to indicate links
     */
    void DrawLine(GameObject moving, GameObject target)
    {

    }
}
