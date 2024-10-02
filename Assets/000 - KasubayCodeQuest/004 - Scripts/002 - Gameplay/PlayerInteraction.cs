using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private GameObject storeObj;
    [SerializeField] private GameObject gameplayObj;
    [SerializeField] private Button interactionButton;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Interactable" || other.gameObject.tag == "ItemInteractable")
        {
            interactionButton.interactable = true;
            interactionButton.onClick.AddListener(() => other.gameObject.GetComponent<DialogueController>().Initialize());
        }
        else if (other.gameObject.tag == "Merchant")
        {
            interactionButton.interactable = true;
            interactionButton.onClick.AddListener(() => 
            {
                storeObj.SetActive(true);
                gameplayObj.SetActive(false);
            });
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Interactable" || other.gameObject.tag == "ItemInteractable" || other.gameObject.tag == "Merchant")
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
