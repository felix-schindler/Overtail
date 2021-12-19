using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class ResponseHandler : MonoBehaviour
{
    [SerializeField] private RectTransform responseBox;
    [SerializeField] private RectTransform responseButtonTemp;
    [SerializeField] private RectTransform responseContainer;

    private DialogueManager dialogueManager;

    private List<GameObject> tempResponseButtons = new List<GameObject>();

    private void Start()
    {
        dialogueManager = GetComponent<DialogueManager>();
    }

    public void showResponses(Response[] responses)
    {
        float responseBoxHeight = 0;
         foreach(Response response in responses)
        {
            GameObject responseButton = Instantiate(responseButtonTemp.gameObject, responseContainer);
            responseButton.gameObject.SetActive(true);
            responseButton.GetComponent<TMP_Text>().text = response.ResponseText;
            responseButton.GetComponent<Button>().onClick.AddListener(() => OnPickeedResponse(response));

            tempResponseButtons.Add(responseButton);

            responseBoxHeight += responseButtonTemp.sizeDelta.y;
        }

        responseBox.sizeDelta = new Vector2(responseBox.sizeDelta.x, responseBoxHeight);
        responseBox.gameObject.SetActive(true);

    }

    private void OnPickeedResponse(Response response)
    {
        responseBox.gameObject.SetActive(false);

        foreach(GameObject button in tempResponseButtons)
        {
            Destroy(button);
        }
        tempResponseButtons.Clear();

        dialogueManager.StartDialogue(response.DialogueObject);
    }
}
