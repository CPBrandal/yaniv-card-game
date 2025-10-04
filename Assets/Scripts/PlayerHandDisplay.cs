using System.Collections.Generic;
using UnityEngine;

public class PlayerHandDisplay : MonoBehaviour
{
    [Header("Hand Settings")]
    public Transform fpsHandTransform;   // The FPSHand object
    public GameObject cardPrefab;
    
    [Header("Card Layout")]
    public float cardSpacing = 0.08f;
    public float cardArcAngle = 35f;
    public float cardVerticalSpread = 0.05f;
    public Vector3 cardOffset = new Vector3(0, 0.08f, 0.05f);  // Offset from hand
    
    private Transform cardAnchor;
    private List<GameObject> displayedCards = new List<GameObject>();
    
    void Start()
    {
        // If FPS hand not assigned, try to find it
        if (fpsHandTransform == null)
        {
            // Try to find it as child of camera
            Camera mainCam = Camera.main;
            if (mainCam != null)
            {
                Transform foundHand = mainCam.transform.Find("FPSHand");
                if (foundHand != null)
                {
                    fpsHandTransform = foundHand;
                }
                else
                {
                    Debug.LogWarning("FPSHand not found! Creating one automatically.");
                    CreateFPSHand();
                }
            }
        }
        
        // Create card anchor
        if (cardAnchor == null && fpsHandTransform != null)
        {
            GameObject anchorObj = new GameObject("CardAnchor");
            anchorObj.transform.SetParent(fpsHandTransform);
            anchorObj.transform.localPosition = cardOffset;
            anchorObj.transform.localRotation = Quaternion.identity;
            cardAnchor = anchorObj.transform;
        }
        
        TestHand();
    }
    
    void CreateFPSHand()
    {
        GameObject handObj = new GameObject("FPSHand");
        handObj.transform.SetParent(Camera.main.transform);
        handObj.transform.localPosition = new Vector3(0.2f, -0.4f, 0.6f);
        handObj.transform.localRotation = Quaternion.Euler(10, -15, 5);
        fpsHandTransform = handObj.transform;
    }
    
    public void UpdateHand(List<Card> cards)
    {
        ClearHand();
        
        int cardCount = cards.Count;
        
        for (int i = 0; i < cardCount; i++)
        {
            float normalizedPosition = cardCount > 1 ? (float)i / (cardCount - 1) : 0.5f;
            float angle = Mathf.Lerp(-cardArcAngle, cardArcAngle, normalizedPosition);
            float xOffset = (i - (cardCount - 1) / 2f) * cardSpacing;
            float yOffset = -Mathf.Abs(normalizedPosition - 0.5f) * cardVerticalSpread;
            float zOffset = (i - (cardCount - 1) / 2f) * 0.01f;
            
            GameObject cardObj = Instantiate(cardPrefab, cardAnchor);
            cardObj.transform.localPosition = new Vector3(xOffset, yOffset, zOffset);
            cardObj.transform.localRotation = Quaternion.Euler(-75, angle, 0);
            
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
    
    void TestHand()
    {
        List<Card> testCards = new List<Card>
        {
            new Card(Suit.Hearts, 1),
            new Card(Suit.Diamonds, 5),
            new Card(Suit.Clubs, 10),
            new Card(Suit.Spades, 13),
            new Card(Suit.Hearts, 7)
        };
        
        UpdateHand(testCards);
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            TestHand();
        }
    }
}