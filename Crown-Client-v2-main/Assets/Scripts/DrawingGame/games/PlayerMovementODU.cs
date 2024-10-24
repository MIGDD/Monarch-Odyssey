using UnityEngine;

public class PlayerMovementODU : MonoBehaviour {

	// This is a reference to the Rigidbody component called "rb"
	public Rigidbody rb;

	public float forwardForce = 2f;	// Variable that determines the forward force
	public float sidewaysForce = 5f;  // Variable that determines the sideways force

	// We marked this as "Fixed"Update because we
	// are using it to mess with physics.
	void FixedUpdate ()
	{
		// Add a forward force
		rb.transform.Translate(0, 0, forwardForce * Time.deltaTime);
		if(gameObject.tag == "Player")
		{
		if (Input.GetKey("d"))	// If the player is pressing the "d" key
		{
			// Add a force to the right
			rb.AddForce(sidewaysForce * Time.deltaTime, 0, 0, ForceMode.VelocityChange);
		}

		if (Input.GetKey("a"))  // If the player is pressing the "a" key
		{
			// Add a force to the left
			rb.AddForce(-sidewaysForce * Time.deltaTime, 0, 0, ForceMode.VelocityChange);
		}
		}
	}
	void OntriggerEnter(Collider other)
		{
			if(other.CompareTag("Floor"))
			{
				Debug.Log("yay");
			}
		}
}

