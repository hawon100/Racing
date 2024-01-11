using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;

public class Junu : MonoBehaviour
{
    NavMeshAgent nav;
    [SerializeField] Transform head;
    private void Awake()
    {
        nav = GetComponent<NavMeshAgent>();
        nav.updateRotation = false;
    }
    private void Start()
    {
        nav.speed = 3;
    }
    void Update()
    {
        var g = GameManager.instance;
        if (!g.isGame) return;
        nav.speed += Time.deltaTime / 30;
        nav.SetDestination(g.player.position);
        var distance = Vector3.Distance(g.player.position, transform.position);
        if (distance <= 15)
        {
            g.WarningMark(distance);
        }
        Vector2 forward = new Vector2(transform.position.z, transform.position.x);
        Vector2 steeringTarget = new Vector2(nav.steeringTarget.z, nav.steeringTarget.x);

        //방향을 구한 뒤, 역함수로 각을 구한다.
        Vector2 dir = steeringTarget - forward;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        //방향 적용
        transform.eulerAngles = Vector3.up * angle;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(gameEnd());
        }
    }
    IEnumerator gameEnd()
    {
        GameManager.instance.isGame = false;
        GameManager.instance.cam.SetParent(head);
        GameManager.instance.cam.DOLocalRotate(new Vector3(0, 180, 0), 1).SetEase(Ease.OutExpo);
        yield return GameManager.instance.cam.DOLocalMove(Vector3.zero, 1).SetEase(Ease.OutExpo).WaitForCompletion();
        yield return new WaitForSeconds(1.5f);
        GameManager.instance.DeathUI();
    }
}
