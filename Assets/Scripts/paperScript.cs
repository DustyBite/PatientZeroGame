using System.Collections;
using TMPro;
using UnityEngine;

public class PaperScript : MonoBehaviour
{
    public GameObject green;
    public GameObject yellow;
    public GameObject orange;
    public TMP_Text text;

    public TestingScript board;

    private Transform assignedSpot;
    private SpotScript spotData;

    private void Start()
    {
        StartCoroutine(DelayedSetup());
    }

    private IEnumerator DelayedSetup()
    {
        // Wait for board to populate
        yield return new WaitForSeconds(0.1f);

        assignedSpot = board.getRandomPos();

        if (assignedSpot == null)
        {
            Debug.LogError("PaperScript: BoardScript returned a null spot.");
            yield break;
        }

        spotData = assignedSpot.GetComponent<SpotScript>();
        if (spotData == null)
        {
            Debug.LogError("PaperScript: SpotScript missing on assigned spot.");
            yield break;
        }

        // Set the text to the spot's name
        text.text = assignedSpot.name;

        // Update indicators
        green.SetActive(spotData.radiOne);
        yellow.SetActive(spotData.radiTwo);
        orange.SetActive(spotData.radiThree);
    }
}
