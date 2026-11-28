using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum CombatState { Setup, PlayerTurn, Resolving, EnemyTurn, Victory, Defeat }

public class CombatManager : MonoBehaviour
{
    [Header("Scene refs")]
    public PlayerHand playerHand;
    public Button endTurnButton;
    public RectTransform playZone;
    public FloatingNumber floatingNumberPrefab;
    public Transform numberParent;
    public Transform enemyTransform;
    public Transform playerAvatarTransform;
    public TMP_Text stateLabel;

    [Header("HP Display")]
    public TMP_Text playerHpText;
    public TMP_Text enemyHpText;
    public Image playerHpBar;
    public Image enemyHpBar;

    [Header("Enemy (stub)")]
    public int enemyHP = 40;
    public int enemyMaxHP = 40;
    public int enemyAttackDamage = 5;

    [Header("Player (stub)")]
    public int playerHP = 30;
    public int playerMaxHP = 30;

    public CombatState State { get; private set; }

    void Start()
    {
        endTurnButton.onClick.AddListener(OnEndTurnPressed);
        UpdateHpDisplay();
        StartCoroutine(SetupRoutine());
    }

    IEnumerator SetupRoutine()
    {
        SetState(CombatState.Setup);
        DragDropManager.Instance.CanInteract = false;
        yield return null;
        StartPlayerTurn();
    }

    void StartPlayerTurn()
    {
        SetState(CombatState.PlayerTurn);
        DragDropManager.Instance.CanInteract = true;
        endTurnButton.interactable = true;
        playerHand.ClearHand();
        playerHand.DrawRandom(3);
    }

    void OnEndTurnPressed()
    {
        if (State != CombatState.PlayerTurn) return;
        StartCoroutine(ResolvePlayerTurn());
    }

    IEnumerator ResolvePlayerTurn()
    {
        SetState(CombatState.Resolving);
        endTurnButton.interactable = false;
        DragDropManager.Instance.CanInteract = false;

        var selected = playerHand.GetSelectedCards();
        foreach (var card in selected)
        {
            Vector3 pos = card.data.cardType == CardType.Attack
                ? enemyTransform.position
                : playerAvatarTransform.position;
            SpawnNumber(card.data.value.ToString(), card.data.valueColor, pos);
            yield return new WaitForSeconds(0.15f);
        }

        playerHand.DiscardAllSelected();
        UpdateHpDisplay();
        yield return new WaitForSeconds(0.5f);

        if (enemyHP <= 0) { SetState(CombatState.Victory); yield break; }

        yield return StartCoroutine(EnemyTurnRoutine());

        if (playerHP <= 0) { SetState(CombatState.Defeat); yield break; }

        StartPlayerTurn();
    }

    IEnumerator EnemyTurnRoutine()
    {
        SetState(CombatState.EnemyTurn);
        yield return new WaitForSeconds(0.3f);
        SpawnNumber(enemyAttackDamage.ToString(), Color.red, playerAvatarTransform.position);
        playerHP -= enemyAttackDamage;
        UpdateHpDisplay();
        yield return new WaitForSeconds(0.8f);
    }

    void SpawnNumber(string text, Color color, Vector3 worldPos)
    {
        var fn = Instantiate(floatingNumberPrefab, numberParent);
        fn.Show(text, color, worldPos);
    }

    void UpdateHpDisplay()
    {
        if (playerHpText) playerHpText.text = $"{playerHP} / {playerMaxHP}";
        if (enemyHpText) enemyHpText.text = $"{enemyHP} / {enemyMaxHP}";
        if (playerHpBar) playerHpBar.fillAmount = (float)playerHP / playerMaxHP;
        if (enemyHpBar) enemyHpBar.fillAmount = (float)enemyHP / enemyMaxHP;
    }

    void SetState(CombatState s)
    {
        State = s;
        if (stateLabel) stateLabel.text = s.ToString();
    }
}