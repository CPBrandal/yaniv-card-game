using UnityEngine;
using TMPro;

public class CardVisual : MonoBehaviour
{
    private Card card;
    public MeshRenderer cardFaceRenderer;  // Changed to MeshRenderer
    private Material cardMaterial;
    private TextMeshPro textLabel;
    private bool isFaceUp = true;

    void Awake()
    {
        // Create material for the card face
        if (cardFaceRenderer != null)
        {
            cardMaterial = new Material(Shader.Find("Unlit/Transparent"));
            cardFaceRenderer.material = cardMaterial;
        }
        else
        {
            CreateTextLabel();
        }
    }

    public void SetCard(Card card, bool faceUp = true)
    {
        this.card = card;
        this.isFaceUp = faceUp;
        
        if (cardFaceRenderer != null && cardMaterial != null)
        {
            DisplayCardSprite();
        }
        else
        {
            DisplayCardText();
        }
    }

    void DisplayCardSprite()
    {
        if (isFaceUp && card != null)
        {
            // Build the sprite name based on your naming convention
            string suitName = card.suit switch
            {
                Suit.Hearts => "hearts",
                Suit.Diamonds => "diamonds",
                Suit.Clubs => "clubs",
                Suit.Spades => "spades",
                _ => "hearts"
            };
            
            string spriteName = $"card-{suitName}-{card.rank}";
            
            // Load sprite and convert to texture
            Sprite cardSprite = Resources.Load<Sprite>($"Cards/{spriteName}");
            
            if (cardSprite != null)
            {
                cardMaterial.mainTexture = cardSprite.texture;
            }
            else
            {
                Debug.LogWarning($"Card sprite not found: {spriteName}");
            }
        }
        else
        {
            // Show card back
            Sprite backSprite = Resources.Load<Sprite>("Cards/card-back1");
            if (backSprite != null)
            {
                cardMaterial.mainTexture = backSprite.texture;
            }
        }
    }

    void DisplayCardText()
    {
        if (textLabel == null)
        {
            CreateTextLabel();
        }
        
        if (isFaceUp && card != null)
        {
            string rankStr = card.rank == 1 ? "A" :
                           card.rank == 11 ? "J" :
                           card.rank == 12 ? "Q" :
                           card.rank == 13 ? "K" :
                           card.rank.ToString();
            
            string suitSymbol = card.suit switch
            {
                Suit.Hearts => "♥",
                Suit.Diamonds => "♦",
                Suit.Clubs => "♣",
                Suit.Spades => "♠",
                _ => ""
            };
            
            textLabel.text = $"{rankStr}{suitSymbol}";
            textLabel.color = (card.suit == Suit.Hearts || card.suit == Suit.Diamonds)
                ? Color.red : Color.black;
        }
        else
        {
            textLabel.text = "🂠";
            textLabel.color = Color.blue;
        }
    }

    void CreateTextLabel()
    {
        GameObject textObj = new GameObject("CardText");
        textObj.transform.SetParent(transform);
        textObj.transform.localPosition = new Vector3(0, 0.51f, 0);
        textObj.transform.localRotation = Quaternion.Euler(90, 0, 0);
        textObj.transform.localScale = Vector3.one;
        
        textLabel = textObj.AddComponent<TextMeshPro>();
        textLabel.fontSize = 3;
        textLabel.alignment = TextAlignmentOptions.Center;
    }
}