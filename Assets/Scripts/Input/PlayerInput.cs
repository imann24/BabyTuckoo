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

    // A function for tracking mouse position entering node objects
    void OnMouseEnter()
    {
        Debug.Log("Hello Node");
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
                Debug.Log("You captured an enemy node");
            }
        }
        else if (!node.IsOwned)
        {
            // Node is not captured
            if(timer > captureEmptyNodeTime)
            {
                node.Owner = Player.Instance;
                Debug.Log("You captured an empty node");
            }

        }
        else
        {
            // Player owns the node
            Debug.Log("You own this node");
        }
    }
    // A function for tracking mouse position leaving node objects
    void OnMouseExit()
    {
        // Reset the hover over timer
        timer = 0;
    }
}
