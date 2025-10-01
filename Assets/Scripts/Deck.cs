using System.Collections.Generic;
using UnityEngine;

public class Deck
{
    private List<Card> cards = new List<Card>();
    private List<Card> discardPile = new List<Card>();
    
    // Create a standard 52-card deck
    public void Initialize()
    {
        cards.Clear();
        
        foreach (Suit suit in System.Enum.GetValues(typeof(Suit)))
        {
            for (int rank = 1; rank <= 13; rank++)
            {
                cards.Add(new Card(suit, rank));
            }
        }
    }
    
    // Shuffle using Fisher-Yates algorithm
    public void Shuffle()
    {
        for (int i = cards.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            Card temp = cards[i];
            cards[i] = cards[randomIndex];
            cards[randomIndex] = temp;
        }
    }
    
    // Draw a card from deck
    public Card DrawCard()
    {
        if (cards.Count == 0)
        {
            ReshuffleDiscardPile();
        }
        
        if (cards.Count == 0)
        {
            Debug.LogError("No cards left to draw!");
            return null;
        }
        
        Card drawnCard = cards[0];
        cards.RemoveAt(0);
        return drawnCard;
    }
    
    // Add cards to discard pile
    public void AddToDiscard(List<Card> discardedCards)
    {
        discardPile.AddRange(discardedCards);
    }
    
    // Get top card from discard pile (for pickup option)
    public Card GetTopDiscard()
    {
        if (discardPile.Count == 0) return null;
        return discardPile[discardPile.Count - 1];
    }
    
    // Pick up from discard pile
    public Card PickupFromDiscard()
    {
        if (discardPile.Count == 0) return null;
        
        Card card = discardPile[discardPile.Count - 1];
        discardPile.RemoveAt(discardPile.Count - 1);
        return card;
    }
    
    // When deck runs out, reshuffle discard pile (keep top card)
    private void ReshuffleDiscardPile()
    {
        if (discardPile.Count <= 1) return;
        
        Card topCard = discardPile[discardPile.Count - 1];
        discardPile.RemoveAt(discardPile.Count - 1);
        
        cards = new List<Card>(discardPile);
        discardPile.Clear();
        discardPile.Add(topCard);
        
        Shuffle();
        Debug.Log("Reshuffled discard pile into deck");
    }
    
    public int CardsInDeck() => cards.Count;
    public int CardsInDiscard() => discardPile.Count;
}