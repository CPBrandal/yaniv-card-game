using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class YanivGameManager : MonoBehaviour
{
    private Deck deck;
    private List<YanivPlayer> players = new List<YanivPlayer>();
    private int currentPlayerIndex = 0;
    public GameObject cardPrefab; // Drag the Card prefab here in Inspector
    private List<GameObject> spawnedCards = new List<GameObject>();
        private Vector3 deckPosition = new Vector3(-0.5f, 1.06f, 0);
    private Vector3 discardPilePosition = new Vector3(0.5f, 1.06f, 0);
    private GameObject deckVisual;
    
    void Start()
    {
        // Test the game logic
        StartNewGame();
    }
    
    void StartNewGame()
    {
        Debug.Log("=== Starting Yaniv Game ===");
        
        // Create players
        players.Add(new YanivPlayer("Player 1"));
        players.Add(new YanivPlayer("Player 2"));
        players.Add(new YanivPlayer("Player 3"));
        
        // Initialize and shuffle deck
        deck = new Deck();
        deck.Initialize();
        deck.Shuffle();
        
        
        // Deal initial hands (5 cards each)
        foreach (YanivPlayer player in players)
        {
            for (int i = 0; i < 5; i++)
            {
                player.AddCard(deck.DrawCard());
            }
        }
        
        CreateDeckVisual();
        
        // Start with one card in discard pile
        List<Card> initialDiscard = new List<Card> { deck.DrawCard() };
        deck.AddToDiscard(initialDiscard);
        
        // Spawn the first discard card visually in discard pile position
        SpawnCardOnTable(initialDiscard[0], discardPilePosition);
        
        // Print initial state
        foreach (YanivPlayer player in players)
        {
            player.PrintHand();
        }
        
        Debug.Log($"Top discard: {deck.GetTopDiscard()}");
        Debug.Log($"Cards in deck: {deck.CardsInDeck()}");
        
        // Test a turn
        TestTurn();
    }

    void CreateDeckVisual()
{
    if (cardPrefab == null) return;
    
    // Create a card to represent the deck (face down)
    deckVisual = Instantiate(cardPrefab, deckPosition, Quaternion.identity);
    
    // Optional: Make it slightly different color to show it's the deck
    Renderer renderer = deckVisual.GetComponent<Renderer>();
    if (renderer != null)
    {
        renderer.material.color = new Color(0.8f, 0.8f, 1f); // Slight blue tint
    }
    
    // Remove the CardVisual component so it doesn't show text
    CardVisual cardVisual = deckVisual.GetComponent<CardVisual>();
    if (cardVisual != null)
    {
        Destroy(cardVisual);
    }
    
    // Add text label to show it's the deck
    GameObject textObj = new GameObject("DeckLabel");
    textObj.transform.SetParent(deckVisual.transform);
    textObj.transform.localPosition = new Vector3(0, 0.01f, 0);
    textObj.transform.localRotation = Quaternion.Euler(90, 0, 0);
    textObj.transform.localScale = Vector3.one;
    
    TextMeshPro textLabel = textObj.AddComponent<TextMeshPro>();
    textLabel.text = "DECK";
    textLabel.fontSize = 0.4f;
    textLabel.alignment = TextAlignmentOptions.Center;
    textLabel.color = Color.black;
}

void SpawnCardOnTable(Card card, Vector3 position)
{
    if (cardPrefab == null)
    {
        Debug.LogError("Card Prefab not assigned!");
        return;
    }
    
    GameObject cardObj = Instantiate(cardPrefab, position, Quaternion.identity);
    CardVisual cardVisual = cardObj.GetComponent<CardVisual>();
    
    if (cardVisual != null)
    {
        cardVisual.SetCard(card);
    }
    
    spawnedCards.Add(cardObj);
}
    
    void TestTurn()
    {
        YanivPlayer currentPlayer = players[currentPlayerIndex];
        Debug.Log($"\n=== {currentPlayer.playerName}'s Turn ===");
        
        // Draw a card from deck
        Card drawnCard = deck.DrawCard();
        currentPlayer.AddCard(drawnCard);
        Debug.Log($"Drew: {drawnCard}");
        
        // Try to discard first card in hand
        List<Card> cardsToDiscard = new List<Card> { currentPlayer.hand[0] };
        
        if (YanivRules.IsValidDiscard(cardsToDiscard))
        {
            Debug.Log($"Discarding: {cardsToDiscard[0]}");
            currentPlayer.RemoveCards(cardsToDiscard);
            deck.AddToDiscard(cardsToDiscard);
        }
        
        currentPlayer.PrintHand();
        
        // Check if can call Yaniv
        if (currentPlayer.CanCallYaniv())
        {
            Debug.Log($"{currentPlayer.playerName} CAN call Yaniv!");
        }
    }
}