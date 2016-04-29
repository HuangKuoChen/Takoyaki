using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Customer : MonoBehaviour {

    public bool Pointing;
    public static Customer Instance;
    public Image[] CustomerImg = new Image[3];
    public Text[] OrderNum = new Text[3];
    public Text[] GetScore = new Text[3];
    public Sprite[] CustomerArray = new Sprite[10];
    public AudioSource CoinSound;


    public void ChangeCustomer(int customer)
    {
        //CustomerImg[i].sprite = CustomerArray[0];
        CustomerImg[customer].sprite = CustomerArray[Random.Range(0,10)];
        OrderNum[customer].text = Random.Range(3, 9).ToString();
    }

    public bool ReceiveTakoyaki(int customer, int num, int score)
    {
        Debug.Log("Customer " + customer + " " + num);
        if (num == int.Parse(OrderNum[customer].text))
        {
            GameManger.Instance.AddScore(score);
            GetScore[customer].text = "+"+score;
            StartCoroutine(DeactiveScore(customer, 2));
            CoinSound.Play();
            ChangeCustomer(customer);
            return true;
        }
            
        return false;
    }

    public void CreateAll()
    {
        gameObject.SetActive(true);
        ChangeCustomer(0);
        ChangeCustomer(1);
        ChangeCustomer(2);
        for (int i = 0; i < 3; i++)
            GetScore[i].text = "";
    }

    public void ClearAll()
    {
        for(int i = 0; i < 3; i++)
        {
            CustomerImg[i].sprite = null;
            OrderNum[i].text = "";
            GetScore[i].text = "";
        }
        gameObject.SetActive(false);
    }

    // Use this for initialization
    void Start () {
        Instance = this;
        gameObject.SetActive(false);
        Pointing = false;
	}

    IEnumerator DeactiveScore(int customer, float second)
    {
        yield return new WaitForSeconds(second);
        GetScore[customer].text = "";
    }
}
