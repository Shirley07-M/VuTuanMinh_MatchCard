using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using System;

[RequireComponent(typeof(Button), typeof(Animator), typeof(CanvasGroup))]

public class CardController : MonoBehaviour
{
    private Button cardClick;
    private Animator cardAnimator;
    private CanvasGroup canvasGroup;

    public bool isFlippedFront {  get; private set; }
    public bool isMatched { get; private set; }  public void setMatch(bool match) => isMatched = match;

    private static readonly int IsFlippedFrontHash = Animator.StringToHash("frontFlipped");
    //! Vì biến này chỉ tạo 1 lần rồi thôi - dùng chung cho cả 24 card, nên phải static để tạo đúng 1 lần, 24 lá bài đều đc dùng luôn
    //* Nếu ko static, mỗi lá bài đc Instantiate ra sẽ phải chạy lại dòng này, gây thừa thãi.

    public int id {  get; private set; }    public void setID(int id) => this.id = id;
    public string clubName { get; private set; }   public void setClubName(string clubName) => this.clubName = clubName;
    [SerializeField] private Image logo;    public void setLogo(Sprite logo) => this.logo.sprite = logo;
    public string knowledgeDescription { get; private set; }    public void setKnowledge(string knowledge)  => this.knowledgeDescription = knowledge;

    public event Action<CardController> OnCardSelected;

    private void Awake()
    {
        cardClick = GetComponent<Button>();
        cardAnimator = GetComponent<Animator>();
        canvasGroup = GetComponent<CanvasGroup>();

        cardClick.onClick.AddListener(() => OnCardSelected?.Invoke(this));
        isMatched = false;
        isFlippedFront = false;
    }
    private void OnDestroy()
    {
        OnCardSelected = null;
    }

    public void SetAfterMatched()
    {
        canvasGroup.blocksRaycasts = false;
        canvasGroup.interactable = false;
        canvasGroup.alpha = 0.65f;
    }
    public void FlipFront()
    {
        isFlippedFront = true;
        cardAnimator.SetBool(IsFlippedFrontHash, true);
    }
    public void FlipBack()
    {
        isFlippedFront = false;
        cardAnimator.SetBool(IsFlippedFrontHash, false);
    }
}
