using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }
    public bool isGame;
    public Transform player;
    public Transform cam;
    public VolumeProfile volume;
    private Vignette vignette;
    public float Stamina;
    public float curStamina;
    [SerializeField] Image fadeImage;
    [SerializeField] Image staminaSlider;
    [SerializeField] Text aliveTime;

    [SerializeField] Image[] hand = new Image[2];
    float curtimeSec;
    int curtimeMin;

    float curHandMoveTime;
    float curBeforeHandTime;
    private void Awake()
    {
        instance = this;
        volume.TryGet(out vignette);
    }
    private void Start()
    {
        curStamina = Stamina;
        StartCoroutine(StartGame());
    }
    private void Update()
    {
        if(isGame) curtimeSec += Time.deltaTime;
        if (curtimeSec >= 60)
        {
            curtimeMin++;
            curtimeSec -= 60;
        }
        aliveTime.text = $"{string.Format("{0:D2}", curtimeMin)}:{string.Format("{0:D2}", (int)curtimeSec)}";

        staminaSlider.fillAmount = curStamina / Stamina;
        bool isRun = Input.GetKey(KeyCode.LeftShift);
        if(isRun) curHandMoveTime += Time.deltaTime;
        else { curHandMoveTime = 0; curBeforeHandTime = 0;}
        if(Mathf.Abs(curHandMoveTime - curBeforeHandTime) >= 0.5f)
        {
            curBeforeHandTime = curHandMoveTime;
            StartCoroutine(HandMove((int)(curHandMoveTime/0.5f) % 2));
        }

        curStamina += (isRun ? -10 : 4) * Time.deltaTime;
        StaminaMaxUpdate();
    }
    IEnumerator HandMove(int index)
    {
        float x = index == 1 ? 1 : -1;
        yield return hand[index].rectTransform.DOAnchorPos(new Vector2(x * 300,-150),0.3f).WaitForCompletion();
        yield return hand[index].rectTransform.DOAnchorPos(new Vector2(x * 1098,-525),0.3f).WaitForCompletion();
    }
    public void WarningMark(float distance)
    {
        var lerpValue = Mathf.InverseLerp(0,15,distance);
        var colorR = Mathf.Lerp(1,0,lerpValue);
        var intensity = Mathf.Lerp(1,0.3f,lerpValue);
        vignette.intensity.Override(intensity);
        vignette.color.Override(new Color(colorR,0,0,1));
    }
    public void StaminaMaxUpdate()
    {
        curStamina = Mathf.Clamp(curStamina, 0, Stamina);
    }
    IEnumerator StartGame()
    {
        fadeImage.color = new Color(0, 0, 0, 1);
        yield return fadeImage.DOColor(new Color(0, 0, 0, 0), 1).WaitForCompletion();
        yield return new WaitForSeconds(1f);
        isGame = true;
    }
    public void DeathUI()
    {
        StartCoroutine(EndGame());
    }
    IEnumerator EndGame()
    {
        yield return fadeImage.DOColor(new Color(0, 0, 0, 1), 1).WaitForCompletion();
        yield return new WaitForSeconds(0.5f);
        aliveTime.rectTransform.DOAnchorPos(Vector3.zero,1).WaitForCompletion();
        yield return aliveTime.rectTransform.DOScale(Vector3.one * 4,1).WaitForCompletion();
    }
}
