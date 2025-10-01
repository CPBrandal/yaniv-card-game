using System.Collections.Generic;
using UnityEngine;

public class YanivPlayer
{
    public string playerName;
    public List<Card> hand = new List<Card>();
    public int totalScore = 0; // Total score across rounds
    
    public YanivPlayer(string name)
    {
        this.playerName = name;
    }
    
    // Calculate current hand value
    public int GetHandValue()
    {
        int value = 0;
        foreach (Card card in hand)
        {
            value += card.GetValue();
        }
        return value;
    }
    
    // Can this player call Yaniv?
    public bool CanCallYaniv()
    {
        return GetHandValue() <= 7;
    }
    
    // Add card to hand
    public void AddCard(Card card)
    {
        hand.Add(card);
    }
    
    // Remove cards from hand
    public void RemoveCards(List<Card> cards)
    {
        foreach (Card card in cards)
        {
            hand.Remove(card);
        }
    }
    
    // Clear hand (new round)
    public void ClearHand()
    {
        hand.Clear();
    }
    
    // Debug: Show hand
    public void PrintHand()
    {
        Debug.Log($"{playerName}'s hand ({GetHandValue()} points):");
        foreach (Card card in hand)
        {
            Debug.Log($"  - {card}");
        }
    }
}