using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private Button interactionButton;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Interactable" || other.gameObject.tag == "ItemInteractable")
        {
            interactionButton.interactable = true;
            interactionButton.onClick.AddListener(() => other.gameObject.GetComponent<DialogueController>().Initialize());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Interactable" || other.gameObject.tag == "ItemInteractable")
        {
            interactionButton.interactable = false;
            interactionButton.onClick.RemoveAllListeners();
        }
    }

    private void Awake()
    {
        interactionButton.interactable = false;
    }
}
