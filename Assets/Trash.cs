using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Trash : MonoBehaviour {

    public static Trash Instance;
    public Image TrashImg;
    public Sprite[] TrashSprite = new Sprite[2];
    private int State;



    public void ChangeState()
    {
        State = (State + 1) % 2;
        TrashImg.sprite = TrashSprite[State];
    }

    public void InitialState()
    {
        State = 0;
        TrashImg.sprite = TrashSprite[State];
    }

    // Use this for initialization
    void Start () {
        Instance = this;
        InitialState();
    }
}
