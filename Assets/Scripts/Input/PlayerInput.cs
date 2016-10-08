using UnityEngine;
using System.Collections;

public class PlayerInput : MannBehaviour
{
    // Abstract classes got from MannBehaviour
    protected override void CleanupReferences()
    {
        // Nothing
    }

    protected override void FetchReferences()
    {
        // Nothing
    }

    protected override void HandleNamedEvent(string eventName)
    {
        // Nothing
    }
    protected override void SetReferences()
    {
        node = this.gameObject.GetComponent<Node>();
    }
    // Variables
    private Node node;
    private float timer;
    private int seconds;
    public int captureEnemyNodeTime = 5;
    public int captureEmptyNodeTime = 2;
    private bool isCaptureAngled = false;
    private Node mouseEnterNode;

    // A function for tracking mouse position enter node objects
    void OnMouseEnter()
    {
        mouseEnterNode = node;
    }
    // A function for tracking mouse position over node objects
    void OnMouseOver()
    {
        // A timer to track how long the player is hovering over a node
        timer += Time.deltaTime;

        // Start to capture node - check node owner
        if (node.IsOwned && node.Owner != Player.Instance)
        {
            // Node is captured and does not belong to the player
            if (timer > captureEnemyNodeTime)
            {
                node.Owner = Player.Instance;
                Player.Instance.lastCapturedNode = node;
            }
        }
        else if (!node.IsOwned)
        {
            // Node is not captured
            if(timer > captureEmptyNodeTime)
            {
                node.Owner = Player.Instance;
                Player.Instance.lastCapturedNode = node;
            }

        }
    }
    // A function for tracking mouse position leaving node objects
    void OnMouseExit()
    {
        // Reset the hover over timer
        timer = 0;
    }

    // A function for checking if a chain can be set up
    void FindLineOfNodes()
    {
        /*
        Node[] nodesToCapture = new Node[3];
        for (int i = 0; i < 3; i++)
        {
            if (isCaptureAngled)
            {
                // Capture chain needs to loop through the correct nodes and capture them
            }
            else if(!isCaptureAngled)
            {
                // Capture chain is not angled so loop and capture nodes
            }
        }
        */
    }
    // A function for finding the distance between two nodes
    void CaptureNodesChain(Node start, Node end)
    {
        int xDiff, yDiff;
        int moveValue;
        xDiff = Mathf.Abs(start.Position.X - end.Position.X);
        yDiff = Mathf.Abs(start.Position.Y - end.Position.Y);
        if (xDiff == yDiff)
        {
            isCaptureAngled = true;
            moveValue = (xDiff > 0) ? xDiff : yDiff;
        }
        else if (xDiff == 0 || yDiff == 0)
        {
            isCaptureAngled = false;
            moveValue = (xDiff > 0) ? xDiff : yDiff;
        }
        // Grabbing direction of nodes
        int xDirection, yDirection;
        xDirection = start.Position.X - end.Position.X;
        yDirection = start.Position.Y - end.Position.Y;

        for (int i = start.Position.X; i < end.Position.X; i++)
        {

        }
    }
    // A function to capture nodes from a start node position and an end node position
}
