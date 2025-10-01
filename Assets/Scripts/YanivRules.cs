using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class YanivRules
{
    // Check if cards form a valid discard (single, set, or run)
    public static bool IsValidDiscard(List<Card> cards)
    {
        if (cards.Count == 0) return false;
        if (cards.Count == 1) return true; // Single card always valid
        
        // Check for set (same rank)
        if (IsSet(cards)) return true;
        
        // Check for run (consecutive same suit)
        if (IsRun(cards)) return true;
        
        return false;
    }
    
    // Check if cards form a set (same rank, different suits)
    private static bool IsSet(List<Card> cards)
    {
        if (cards.Count < 2) return false;
        
        int firstRank = cards[0].rank;
        foreach (Card card in cards)
        {
            if (card.rank != firstRank) return false;
        }
        return true;
    }
    
    // Check if cards form a run (3+ consecutive, same suit)
    private static bool IsRun(List<Card> cards)
    {
        if (cards.Count < 3) return false;
        
        // All same suit?
        Suit firstSuit = cards[0].suit;
        foreach (Card card in cards)
        {
            if (card.suit != firstSuit) return false;
        }
        
        // Sort by rank
        List<Card> sorted = cards.OrderBy(c => c.rank).ToList();
        
        // Check consecutive
        for (int i = 1; i < sorted.Count; i++)
        {
            if (sorted[i].rank != sorted[i - 1].rank + 1)
            {
                return false;
            }
        }
        
        return true;
    }
    
    // Check for Assaf (someone has lower/equal score when Yaniv called)
    public static bool CheckAssaf(YanivPlayer caller, List<YanivPlayer> allPlayers)
    {
        int callerValue = caller.GetHandValue();
        
        foreach (YanivPlayer player in allPlayers)
        {
            if (player == caller) continue;
            
            if (player.GetHandValue() <= callerValue)
            {
                return true; // Assaf!
            }
        }
        
        return false;
    }
}