using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Player : MonoBehaviour
{

    Rigidbody rigid;
    public Camera mainCam;
    public float moveSpeed;
    bool isMap = false;
    Coroutine mapMove;
    void Start()
    {
        mainCam.transform.localPosition = new Vector3(0, 0.8f, 0);
        rigid = GetComponent<Rigidbody>();
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
        if (!isMap)
        {

            float x = Input.GetAxisRaw("Horizontal") * moveSpeed * Time.deltaTime;
            float z = Input.GetAxisRaw("Vertical") * moveSpeed * Time.deltaTime;

            rigid.velocity = new Vector3(x, rigid.velocity.y, z);
        }
    }
    IEnumerator MapMove(bool mapActive)
    {
        isMap = !isMap;
        if (!mapActive)
        {
            yield return mainCam.transform.DOLocalMoveY(100, 1.5f).SetEase(Ease.InOutExpo);
            yield return mainCam.transform.DOLocalRotate(new Vector3(90, 0, 0), 1).SetEase(Ease.InOutExpo);
        }
        else
        {
            yield return mainCam.transform.DOLocalMoveY(0.8f, 1.5f).SetEase(Ease.InOutExpo);
            yield return mainCam.transform.DOLocalRotate(new Vector3(0, 0, 0), 1).SetEase(Ease.InOutExpo);
        }
    }
}
