using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class Takoyaki : MonoBehaviour {

    public static Takoyaki Instance;
    public Image TakoyakiImg;
    public Sprite[] SpriteArray = new Sprite[6];
    public Image Tako, Exclamation, Cry;
    private Box DropBox;
    public AudioSource YakiSound, TrashSound;
    private int State;          //State  0 = Empty --Click-> 1 --Click-> 2 = Taco --3s-> 3 = Exclamation --Click-> 4 --5s-> 5 --3s-> 6 --3s-> 7
    private float time;
    private bool Dragging, MoveToTrash, MoveToBox;
    private Vector3 InitialPos;
    private float OffsetX, OffsetY;

    public void Click()
    {
        if (GameManger.Instance.play && !GameManger.Instance.pause && !Dragging)
        {
            if(State == 0)                  // State 0 --> 1
            {
                ChangeState((State + 1) % 8);
                YakiSound.Play();
            } else if(State == 1)           // State 1 --> 2
            {
                ChangeState((State + 1) % 8);
            } else if(State == 3)           // State 3 --> 4
            {
                ChangeState((State + 1) % 8);
                YakiSound.Play();
            }
            
        }
    }

    public void BeginDrag()
    {
        MoveToTrash = false;
        MoveToBox = false;
    }

    public void Drag()
    {
        if (GameManger.Instance.play && !GameManger.Instance.pause && State > 4)
        {
            Dragging = true;
            transform.position = Input.mousePosition;
            PointerEventData pointer = new PointerEventData(EventSystem.current);
            pointer.position = Input.mousePosition;

            List<RaycastResult> raycastResults = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointer, raycastResults);

            if (raycastResults.Count > 1)
            {
                //Debug.Log("Drag on " + raycastResults[1].gameObject.tag);
                // Judge if the Takoyaki was dragged to trash
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
                if (!MoveToBox && raycastResults[1].gameObject.tag == "Box")
                {
                    DropBox = raycastResults[1].gameObject.GetComponent<Box>();
                    MoveToBox = true;
                }
                else if (MoveToBox && raycastResults[1].gameObject.tag != "Box")
                {
                    MoveToBox = false;
                }
            }
        }
    }

    public void EndDrag()
    {
        if (!Dragging)
            return;
        gameObject.transform.position = InitialPos;
        Dragging = false;
        if (MoveToTrash)
        {
            //Debug.Log("Drop on trash");
            MoveToTrash = false;
            Trash.Instance.ChangeState();
            ChangeState(0);
            TrashSound.Play();
        } else if (MoveToBox)
        {
            //Debug.Log("Drop on Box");
            MoveToBox = false;
            if (State != 7)
                if(DropBox.AddTakoyaki(State))
                    ChangeState(0);
        }
    }


    // Use this for initialization
    void Start () {
        Instance = this;
        InitialPos = gameObject.transform.position;
        TakoyakiImg = this.GetComponent < Image >();
        ClearAll();
    }
	
	// Update is called once per frame
	void Update () {
        if (GameManger.Instance.play && !GameManger.Instance.pause)
        {
            time += Time.deltaTime;
            if(State == 2 && time > 3)              // State 2 --> 3
            {
                ChangeState((State + 1) % 8);
            } else if(State == 4 && time > 3)       // State 4 --> 5
            {
                ChangeState((State + 1) % 8);
            } else if(State == 5 && time > 2)       // State 5 --> 6
            {
                ChangeState((State + 1) % 8);
            } else if(State == 6 && time > 3)       // State 6 --> 7
            {
                Cry.gameObject.SetActive(true);
                ChangeState((State + 1) % 8);
            }
        }
        
    }

    void ChangeState(int s)
    {
        State = s;
        if (State < 2)
        {
            Cry.gameObject.SetActive(false);
            TakoyakiImg.sprite = SpriteArray[State];
        } else if (State == 2)
        {
            Tako.gameObject.SetActive(true);
        }
        else if (State == 3)
        {
            Exclamation.gameObject.SetActive(true);
        }
        else if (State == 4)
        {
            Tako.gameObject.SetActive(false);
            Exclamation.gameObject.SetActive(false);
            TakoyakiImg.sprite = SpriteArray[2];
        } else
        {
            TakoyakiImg.sprite = SpriteArray[State - 2];
        }
        time = 0;
    }

    public void ClearAll()
    {
        time = 0;
        Dragging = false;
        MoveToTrash = false;
        MoveToBox = false;
        Tako.gameObject.SetActive(false);
        Exclamation.gameObject.SetActive(false);
        Cry.gameObject.SetActive(false);
        ChangeState(0);
    }
}
