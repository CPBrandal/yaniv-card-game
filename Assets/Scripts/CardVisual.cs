using UnityEngine;
using TMPro;

public class CardVisual : MonoBehaviour
{
    public Card cardData; // The actual card data (rank, suit, etc.)
    private TextMeshPro textLabel;
    
    void Start()
    {
        // Create text label on the card
        CreateCardLabel();
        UpdateCardDisplay();
    }
    
    void CreateCardLabel()
    {
        // Create a text object as child
        GameObject textObj = new GameObject("CardText");
        textObj.transform.SetParent(transform);
        textObj.transform.localPosition = new Vector3(0, 0.01f, 0);
        textObj.transform.localRotation = Quaternion.Euler(90, 0, 0);
        textObj.transform.localScale = Vector3.one;
        
        textLabel = textObj.AddComponent<TextMeshPro>();
        textLabel.fontSize = 5;
        textLabel.alignment = TextAlignmentOptions.Center;
        textLabel.color = Color.black;
    }
    
    public void SetCard(Card card)
    {
        cardData = card;
        UpdateCardDisplay();
    }
    
    void UpdateCardDisplay()
    {
        if (cardData != null && textLabel != null)
        {
            textLabel.text = cardData.ToString();
        }
    }
}