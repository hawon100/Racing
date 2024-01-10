using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Player : MonoBehaviour
{
    public Camera mainCam;
    public bool isMap = false;
    Coroutine mapMove;
    public enum PlayerMode
    {
        Move,
        Map
    }
    public PlayerMode playerMode = PlayerMode.Move;
    void Start()
    {
        mainCam = Camera.main;
        mainCam.transform.localPosition = new Vector3(0, 0f, 0);
    }
    void Update()
    {
        keyInput();
    }
    void keyInput()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            if (mapMove != null) StopCoroutine(mapMove);
            mapMove = StartCoroutine(MapMove(isMap));
        }
    }
    IEnumerator MapMove(bool mapActive)
    {
        isMap = !isMap;
        if (!mapActive)
        {
            yield return mainCam.transform.DOLocalMoveY(100, 1.5f).SetEase(Ease.InOutQuad);
            yield return mainCam.transform.DOLocalRotate(new Vector3(90, 0, 0), 1).SetEase(Ease.InOutQuad);
        }
        else
        {
            yield return mainCam.transform.DOLocalMoveY(0.8f, 1.5f).SetEase(Ease.InOutQuad);
            yield return mainCam.transform.DOLocalRotate(new Vector3(0, 0, 0), 1).SetEase(Ease.InOutQuad);
        }
        playerMode = isMap ? PlayerMode.Map : PlayerMode.Move;
    }
}
