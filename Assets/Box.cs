using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class Box : MonoBehaviour {

    public static Box Instance;
    public Image[] TakoyakiImg = new Image[8];
    public int Number;
    public bool Dragging, MoveToTrash, MoveToCustomer;
    public int OnCustomer;
    public AudioSource TrashSound;
    private Vector3 InitialPos, InitialScale;
    public int BoxScore;

    public void BeginDrag()
    {
        
    }

    public void Drag()
    {
        if (GameManger.Instance.play && !GameManger.Instance.pause && Number > 0)
        {
            if (!Dragging)
                transform.localScale -= new Vector3(0.1f, 0.1f, 0);
            Dragging = true;
            transform.position = Input.mousePosition;
            PointerEventData pointer = new PointerEventData(EventSystem.current);
            pointer.position = Input.mousePosition;

            List<RaycastResult> raycastResults = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointer, raycastResults);
            if (raycastResults.Count > 1)
            {
                // Judge if the box was dragged to trash
                if (!MoveToTrash && raycastResults[1].gameObject.tag == "Trash")
                {
                    MoveToTrash = true;
                    Trash.Instance.ChangeState();
                }
                else if (MoveToTrash && raycastResults[1].gameObject.tag != "Trash")
                {
                    MoveToTrash = false;
                    Trash.Instance.ChangeState();
                }

                // Judge if the box was dragged to customer
                if (raycastResults[1].gameObject.tag.Contains("Customer"))
                {
                    MoveToCustomer = true;
                    OnCustomer = int.Parse("" + raycastResults[1].gameObject.tag[8]);
                    Debug.Log("Drag on Customer" + OnCustomer);
                }
                else if (MoveToCustomer && !raycastResults[1].gameObject.tag.Contains("Customer"))
                {
                    MoveToCustomer = false;
                }

            }
        }
    }

    public void EndDrag()
    {
        transform.position = InitialPos;
        transform.localScale = InitialScale;
        if (!Dragging)
            return;
        
        if (MoveToTrash)
        {
            MoveToTrash = false;
            Trash.Instance.ChangeState();
            Reset();
            TrashSound.Play();
        } else if (MoveToCustomer)
        {
            MoveToCustomer = false;
            Debug.Log("Move to Customer");
            if (Customer.Instance.ReceiveTakoyaki(OnCustomer, Number, BoxScore))
            {
                Reset();
            }
        }
        Dragging = false;
    }


    // Use this for initialization
    void Start () {
        Instance = this;
        InitialPos = transform.position;
        InitialScale = transform.localScale;
        Reset();
    }
	
	public bool AddTakoyaki(int state)
    {
        if (Number == 8)
            return false;
        Number += 1;
        if(state == 5)
            BoxScore += 10;
        else if(state == 6)
            BoxScore += 5;
        TakoyakiImg[Number - 1].gameObject.SetActive(true);
        return true;
    }

    public void Reset()
    {
        Number = 0;
        BoxScore = 0;
        Dragging = false;
        for (int i = 0; i < 8; i++)
        {
            TakoyakiImg[i].gameObject.SetActive(false);
        }
    }
}
