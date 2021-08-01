using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{
    public GameObject m_cardPrefab;
    public List<Card> m_deck;

    // Start is called before the first frame update
    void Start()
    {
        GenerateDeck();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void GenerateDeck()
    {
        m_deck = new List<Card>();

        for (int s = 0; s <  4 ; s++)
        {
            CardSuit currentSuit = CardHelper.Instance.GetCardSuitFromIndex(s);

            for (int v = 0; v < 13; v++)
            {
                CardValue currentCardValue = CardHelper.Instance.GetCardValueFromInt(v);

                GameObject clonedCard = Instantiate(m_cardPrefab, transform.position, Quaternion.identity);

                clonedCard.GetComponent<Card>().InitializeCard(v, currentSuit);

                clonedCard.name = currentCardValue.ToString() + " of " + currentSuit.ToString();

                clonedCard.transform.position = new Vector3( v * 1.2f , s * 1.7f, 0 );

                clonedCard.transform.SetParent(this.transform);

                m_deck.Add(clonedCard.GetComponent<Card>());
            }
        }
    }
}
