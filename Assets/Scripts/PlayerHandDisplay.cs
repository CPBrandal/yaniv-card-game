using System.Collections.Generic;
using UnityEngine;

public class PlayerHandDisplay : MonoBehaviour
{
    [Header("Hand Settings")]
    public Transform handAnchor;  // Position where cards appear
    public GameObject cardPrefab;
    public float cardSpacing = 0.2f;  // Space between cards
    public float cardArcAngle = 20f;  // How much the hand fans out
    public float distanceFromCamera = 1.5f;  // How far from camera
    public float cardYOffset = -0.5f;  // How low below camera center
    
    private List<GameObject> displayedCards = new List<GameObject>();
    private YanivPlayer player;
    
    void Start()
    {
        // Create hand anchor if not assigned
        if (handAnchor == null)
        {
            GameObject anchorObj = new GameObject("HandAnchor");
            anchorObj.transform.SetParent(Camera.main.transform);
            anchorObj.transform.localPosition = new Vector3(0, cardYOffset, distanceFromCamera);
            anchorObj.transform.localRotation = Quaternion.identity;
            handAnchor = anchorObj.transform;
        }
        
        // For testing - will connect to actual player later
        TestHand();
    }
    
public void UpdateHand(List<Card> cards)
{
    // Clear existing cards
    ClearHand();
    
    // Spawn new cards
    int cardCount = cards.Count;
    
    for (int i = 0; i < cardCount; i++)
    {
        // Calculate position in arc
        float normalizedPosition = cardCount > 1 ? (float)i / (cardCount - 1) : 0.5f;
        float angle = Mathf.Lerp(-cardArcAngle, cardArcAngle, normalizedPosition);
        
        // Calculate horizontal offset
        float xOffset = (i - (cardCount - 1) / 2f) * cardSpacing;
        
        // Spawn card
        GameObject cardObj = Instantiate(cardPrefab, handAnchor);
        
        // Position card
        cardObj.transform.localPosition = new Vector3(xOffset, 0, 0);
        
        // Rotate card to face camera - FIXED HERE
        cardObj.transform.localRotation = Quaternion.Euler(-90, angle, 0);
        
        // Set card data
        CardVisual cardVisual = cardObj.GetComponent<CardVisual>();
        if (cardVisual != null)
        {
            cardVisual.SetCard(cards[i], true);
        }
        
        displayedCards.Add(cardObj);
    }
}
    
    void ClearHand()
    {
        foreach (GameObject card in displayedCards)
        {
            Destroy(card);
        }
        displayedCards.Clear();
    }
    
    // For testing - creates dummy cards
    void TestHand()
    {
        List<Card> testCards = new List<Card>
        {
            new Card(Suit.Hearts, 1),    // Ace of Hearts
            new Card(Suit.Diamonds, 5),  // 5 of Diamonds
            new Card(Suit.Clubs, 10),    // 10 of Clubs
            new Card(Suit.Spades, 13),   // King of Spades
            new Card(Suit.Hearts, 7)     // 7 of Hearts
        };
        
        UpdateHand(testCards);
    }
    
    void Update()
    {
        // Press 'H' to refresh hand (for testing)
        if (Input.GetKeyDown(KeyCode.H))
        {
            TestHand();
        }
    }
}