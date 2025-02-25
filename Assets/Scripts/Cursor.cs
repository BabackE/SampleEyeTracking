using UnityEngine;
using UnityEngine.InputSystem.Interactions;

public class Cursor : MonoBehaviour
{
    private GameObject currentSelection;
    private const float SELECTION_TIME = 0.5f; // Time for the selection to occur in seconds
    private const float DEBOUNCE_DELAY = 0.1f;   // Time to wait before reseting a selection
    private float timer = 0;

    // Called when a collision occurs with this object
    void OnCollisionEnter(Collision collision)
    {
        // Check if the collided object has the specified tag
        if (collision.gameObject.CompareTag("Target"))
        {
            // Cancel any pending reset, so the timer remains if the collision resumes quickly.
            CancelInvoke("ResetSelection");

            Debug.Log("Collided: " + collision.gameObject.name);

            // Change scale of current object
            //collision.gameObject.GetComponent<Renderer>().material.color = Color.red;

            currentSelection = collision.gameObject;
        }
    }

    // Called when a collision occurs with this object
    void OnCollisionStay(Collision collision)
    {
        // Check if the collided object has the specified tag
        if (collision.gameObject.CompareTag("Target") && currentSelection == collision.gameObject)
        {
            // Check if timer has reached threshold
            timer += Time.fixedDeltaTime;
            if (timer >= SELECTION_TIME)
            {
                timer = 0;
                // Change scale of current object
                collision.gameObject.GetComponent<Renderer>().material.color = Color.red;
                currentSelection = null;
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (currentSelection != null && collision.gameObject == currentSelection)
        {
            // Instead of immediately resetting, schedule a reset after DEBOUNCE_DELAY seconds.
            Invoke("ResetSelection", DEBOUNCE_DELAY);
        }

    }
    // This method resets the timer and current selection.
    private void ResetSelection()
    {
        timer = 0;
        currentSelection = null;
    }

}