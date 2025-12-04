using Oculus.Interaction;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class FlagSnapZone : MonoBehaviour
{
    private XRGrabInteractable flagInsideZone;

    void OnTriggerEnter(Collider other)
    {
        // Check tag
        if (other.CompareTag("flag"))
        {
            XRGrabInteractable grab = other.GetComponent<XRGrabInteractable>();
            if (grab != null)
            {
                flagInsideZone = grab;

                // Subscribe to release event
                grab.selectExited.AddListener(OnFlagReleased);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("flag"))
        {
            XRGrabInteractable grab = other.GetComponent<XRGrabInteractable>();
            if (grab != null)
            {
                // Remove listener and clear
                grab.selectExited.RemoveListener(OnFlagReleased);

                if (flagInsideZone == grab)
                    flagInsideZone = null;
            }
        }
    }

    private void OnFlagReleased(SelectExitEventArgs args)
    {
        // Only parent it if still inside zone
        if (flagInsideZone != null && args.interactableObject.transform == flagInsideZone.transform)
        {
            flagInsideZone.transform.SetParent(this.transform);
            flagInsideZone.transform.localPosition = Vector3.zero; // optional snap to center
            flagInsideZone.transform.localRotation = Quaternion.identity;
        }
    }
}
