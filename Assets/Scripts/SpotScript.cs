using UnityEngine;
using Oculus.Interaction;

public class SpotScript : MonoBehaviour
{
    private Grabbable flag;
    private Rigidbody flagRB;

    private bool flagInsideZone = false;
    private bool wasHeldLastFrame = false;

    public bool radiOne = false;
    public bool radiTwo = false;
    public bool radiThree = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("flag"))
        {
            print("Wrong Tag");
            return;
        }

        flag = other.GetComponent<Grabbable>();
        if (flag == null)
        {
            print("Grabbable Null");
            return;
        }

        flagRB = other.GetComponent<Rigidbody>();
        if (flagRB == null)
        {
            print("rigid Null");
            return;
        }


        flagInsideZone = true;
        wasHeldLastFrame = flag.SelectingPointsCount > 0;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("flag"))
            return;

        if (flagRB != null)
            Unfreeze(flagRB);

        // Reset state
        flagInsideZone = false;
        wasHeldLastFrame = false;
        flag = null;
        flagRB = null;
    }

    private void OnTriggerStay(Collider other)
    {
        /*
        if (!other.CompareTag("flag") || flag == null)
            return;
        */

        bool isHeld = flag.SelectingPointsCount > 0;

        // RELEASE DETECTION:
        // Only triggers when:
        //  1. It was held last frame
        //  2. Now it is NOT held
        if (flagInsideZone && wasHeldLastFrame && !isHeld)
        {
            // RELEASED INSIDE THE ZONE → SNAP IT
            Freeze(flagRB);
            Snap(flag.transform);
        }

        // Update for next frame
        wasHeldLastFrame = isHeld;
    }

    // --- Helpers ---

    private void Freeze(Rigidbody rb)
    {
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        //rb.useGravity = false;
        //rb.isKinematic = true;

        rb.constraints = RigidbodyConstraints.FreezeAll;

        Debug.Log("Flag frozen.");
    }

    private void Unfreeze(Rigidbody rb)
    {
        //rb.useGravity = true;
        //rb.isKinematic = false;
        rb.constraints = RigidbodyConstraints.None;

        Debug.Log("Flag unfrozen.");
    }

    private void Snap(Transform t)
    {
        t.SetParent(transform);
        t.localPosition = new Vector3(0f, 1f, 0f);   // <-- apply your 1-unit offset
        t.localRotation = Quaternion.identity;

        Debug.Log("Flag snapped with offset.");
    }

}
