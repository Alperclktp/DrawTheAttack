using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

public class CardManager : Singleton<CardManager>
{
    public List<Card> cardList = new List<Card>();

    [Header("Mana Settings")]
    public int currentMana;
    [SerializeField] private int maxMana;

    [Space(5)]
    [SerializeField] private Card selectedCard;

    [SerializeField] private GameObject soldierSpawnHolder;

    [Header("UI Elements")]
    [SerializeField] private Slider manaSliderBar;

    [SerializeField] private Text currentManaText;
    [SerializeField] private Text maxManaText;

    [Header("Card Colors")]
    [SerializeField] private Color defaultColor;
    [SerializeField] private Color selectedColor;
    //[SerializeField] private Color unselectedColor;

    private float spawnIntervalTimer = 0.04f;

    private void Start()
    {
        for (int i = 0; i < cardList.Count; i++)
        {
            cardList[i].GetComponent<Image>().color = defaultColor;
        }

        currentMana = maxMana;

        manaSliderBar.maxValue = maxMana;
        manaSliderBar.value = maxMana;
    }

    private void Update()
    {
        SpawnCard(selectedCard);

        CheckMana();

        CheckCardInteractable();

        if (selectedCard != null)
        {
            ChooseCard(selectedCard);
        }
    }

    public void CheckCardInteractable()
    {
        foreach (Card card in cardList)
        {
            if (currentMana >= card.currentManaCost)
            {
                card.GetComponent<Button>().interactable = true;
            }
            else
            {
                card.GetComponent<Button>().interactable = false;
            }
        }    
    }

    public void ChooseCard(Card card)
    {
        for (int i = 0; i < cardList.Count; i++) // Deselect operation
        {
            cardList[i].GetComponent<Image>().color = defaultColor;

            cardList[i].IsSelected = false;

            cardList[i].transform.DOScale(1.05f, 0.2f);
        }

        if (currentMana >= card.currentManaCost)
        {
            selectedCard = card;

            card.cardObj.GetComponent<Image>().color = selectedColor; // Select operation

            card.IsSelected = true;

            card.transform.DOScale(1.15f, 0.2f);
        }
        else
        {
            //card.GetComponent<Image>().color = Color.gray;
        }
    }

    private void SpawnCard(Card selectedCard)
    {
        spawnIntervalTimer -= Time.deltaTime;

        if (spawnIntervalTimer <= 0)
        {
            if (selectedCard != null && selectedCard.IsSelected && currentMana > 0 && Input.GetMouseButton(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
                {
                    if (hit.collider != null && !hit.collider.CompareTag("Soldier") && !hit.collider.CompareTag("Enemy") && hit.collider.CompareTag("Ground"))
                    {
                        GameObject obj = Instantiate(selectedCard.cardPrefab, hit.point, selectedCard.cardPrefab.transform.rotation);

                        obj.transform.parent = soldierSpawnHolder.transform;

                        GameManager.Instance.soldierList.Add(obj);

                        currentMana -= selectedCard.currentManaCost;

                        spawnIntervalTimer = 0.04f;

                        Destroy(VFXManager.SpawnEffect(VFXType.CARD_SPAWN_EFFECT, obj.transform.position + new Vector3(0,1,0),Quaternion.identity),1);
                       
                        Debug.Log("Spawned the: " + selectedCard.name);
                    }
                }
            }
            else if(selectedCard != null && !selectedCard.IsSelected && currentMana < selectedCard.currentManaCost && Input.GetMouseButton(0))
            {
                //Alert mana bar animation.

                //manaSliderBar.GetComponent<Animator>().SetBool("Slider", true);
            }
        }
    }

    private void CheckMana()
    {
        manaSliderBar.value = currentMana;

        manaSliderBar.maxValue = maxMana;

        currentManaText.text = currentMana.ToString();
        maxManaText.text = maxMana.ToString();

        if (currentMana <= 0)
        {
            currentMana = 0;
        }

        if (currentMana >= maxMana)
        {
            currentMana = maxMana;
        }
    }
}