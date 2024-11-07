

using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class HammerReturn : MonoBehaviour
{
    public XRSocketInteractor socket;
    public XRGrabInteractable hammer;

    private Transform hammerTransform;

    private bool isCoroutineActive = false;


    private void Start(){
        hammerTransform = hammer.transform;
    }

    private void Update()
    {
        if (socket.isPerformingManualInteraction)
        {
            if (!isCoroutineActive) return;
            isCoroutineActive = false;
            StopCoroutine(ReturnToSocket());
        }
        else
        {
            isCoroutineActive = true;
            StartCoroutine(ReturnToSocket());
        }
    }

    private IEnumerator ReturnToSocket()
    {
        while(true)
        {
            float Distance = Vector3.Distance(socket.transform.position,hammerTransform.position);
            if(Distance > 20f)
                {
                    socket.StartManualInteraction(hammer as IXRSelectInteractable);
                }
            yield return new WaitForSeconds(0.5f);
        }

    }
}
