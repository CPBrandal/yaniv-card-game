using UnityEngine;

public enum Suit
{
    Hearts,
    Diamonds,
    Clubs,
    Spades
}

public class Card
{
    public Suit suit;
    public int rank; // 1 = Ace, 11 = Jack, 12 = Queen, 13 = King
    
    public Card(Suit suit, int rank)
    {
        this.suit = suit;
        this.rank = rank;
    }
    
    // Get point value for Yaniv scoring
    public int GetValue()
    {
        if (rank == 1) return 1; // Ace
        if (rank > 10) return 10; // Face cards
        return rank; // Number cards
    }
    
    // For debugging/display
    public override string ToString()
    {
        string rankName = rank switch
        {
            1 => "Ace",
            11 => "Jack",
            12 => "Queen",
            13 => "King",
            _ => rank.ToString()
        };
        return $"{rankName} of {suit}";
    }
}