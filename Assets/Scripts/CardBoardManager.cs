using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;


public class CardBoardManager : MonoBehaviour
{
    public static CardBoardManager Instance { get; private set; }

    [SerializeField] private Transform cardContainer;
    [SerializeField] private CardController card;

    private List<CardController> selectedCard;

    private bool isProcessing = false;
    [SerializeField] private List<CardData> cardDataList;


    private void Awake()
    {
        Instance = this;
        selectedCard = new List<CardController>();
    }

    private void Start()
    {
        InitializeCardBoard();
    }

    private void InitializeCardBoard()
    {
        List<CardData> cardDataBoard = new();

        foreach (CardData cardData in cardDataList)
        {
            cardDataBoard.Add(cardData);
            cardDataBoard.Add(cardData);
        }

        for (int i = cardDataBoard.Count - 1; i > 0; i--)
        {
            int randomCardIndex = Random.Range(0, i + 1); // hàm random lấy từ a đến b - 1, nên phải truyền i + 1

            (cardDataBoard[i], cardDataBoard[randomCardIndex]) = (cardDataBoard[randomCardIndex], cardDataBoard[i]);
        }


        foreach (var cardData in cardDataBoard)
        {
            CardController currentCard = Instantiate(card, cardContainer);
            currentCard.OnCardSelected += OnCardSelected;
            currentCard.setID(cardData.pairID);
            currentCard.setClubName(cardData.clubName);
            currentCard.setLogo(cardData.clubLogo);
            currentCard.setKnowledge(cardData.knowledgeDescription);
        }
    }

    private void OnCardSelected(CardController currentCard)
    {
        if (currentCard.isFlippedFront || currentCard.isMatched || isProcessing) return;

        selectedCard.Add(currentCard);
        currentCard.FlipFront();

        if (selectedCard.Count == 2) StartCoroutine(CheckMatch());
    }

    private IEnumerator CheckMatch()
    {
        isProcessing = true;
        yield return new WaitForSeconds(0.65f);

        var firstCard = selectedCard[0];
        var secondCard = selectedCard[1];

        if (firstCard.id == secondCard.id)
        {
            firstCard.setMatch(true);
            firstCard.SetAfterMatched();
            secondCard.setMatch(true);
            secondCard.SetAfterMatched();
        }
        else
        {
            firstCard.FlipBack();
            secondCard.FlipBack();
            yield return new WaitForSeconds(0.35f);
        }

        selectedCard.Clear();
        isProcessing = false;
    }
}
