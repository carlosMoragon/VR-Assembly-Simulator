using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class PlaceRemovePiece : MonoBehaviour
{

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == this.gameObject.tag)
        {
            Rigidbody rb = other.gameObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                rb.isKinematic = true;
                rb.useGravity = false;
            }

            other.transform.SetParent(transform);
            other.transform.localPosition = Vector3.zero;
            other.transform.localRotation = Quaternion.identity;

            XRGrabInteractable grab = other.gameObject.GetComponent<XRGrabInteractable>();
            if (grab != null)
            {
                grab.enabled = false;
                StartCoroutine(ReactivateGrabAfterDelay(grab, 3f));
            }
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.tag == this.gameObject.tag)
        {
            other.transform.SetParent(null);

            Rigidbody rb = other.gameObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false;
                rb.useGravity = true;
            }
        }
    }

    private IEnumerator ReactivateGrabAfterDelay(XRGrabInteractable grab, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (grab != null)
        {
            grab.enabled = true;
        }
    }
}
