using System.Collections;
using UnityEngine;


public class AIMove : MonoBehaviour
{
    //TODO: target bilgisi cardslot'dan red/blue team gibi ayrimlarla yollanacak
    //player blue, enemy red gibi...
    public Card stats;
    public TeamColor team;
    public bool isAttacked;
    public bool canAttack;

    public StateType CurrentStateType;
    public StateType NextStateType;
    public Transform CurrentTarget;
    public Transform NextTarget;

    private EntityController _controller;
    private Rigidbody _rb;
    private Vector3 _spawnPos;
    
    private void Awake()
    {
        _controller = GetComponent<EntityController>();
        CurrentStateType = StateType.Idle;
        NextStateType = StateType.Idle;
        _rb = GetComponent<Rigidbody>();
        isAttacked = false;
        canAttack = false;
    }

    private void Start()
    {
        CurrentTarget = transform;
        NextTarget = transform;

        _spawnPos = CurrentTarget.position;
        stats = transform.parent.GetComponent<CardSlot>()._card;
        team = transform.parent.GetComponent<CardSlot>().Team;
        _controller.SetStateType(this);
        
        if(team == TeamColor.Red) transform.rotation = new Quaternion(0, 1, 0, 0);
    }

    private void Update()
    {
        //TODO: monsterlarin karar mekanizmasi yazilacak...

        if (NextTarget == null)
        {
            NextTarget = CurrentTarget;
        }
        
        switch (NextStateType)
        {
            case StateType.Idle:
                HandleIdle();
                break;
            case StateType.Walk:
                if (canAttack) HandleWalk();
                break;
        }
    }

    public void HandleIdle()
    {
        NextStateType = StateType.Idle;
        _controller.SetStateType(this);
       
        GetTarget();

        if (NextTarget != CurrentTarget && !isAttacked && NextTarget != null && canAttack)
        {
            HandleWalk();
        }
    }

    private void GetTarget()
    {
        if (team == GameManager.Instance.PlayableColor)
        {
            switch (team)
            {
                case TeamColor.Red:
                    if(CardGameManager.Instance.playerMonsters.Count > 0) NextTarget = CardGameManager.Instance.playerMonsters[0].transform;
                    break;
                
                case TeamColor.Blue:
                    if(CardGameManager.Instance.enemyMonsters.Count > 0) NextTarget = CardGameManager.Instance.enemyMonsters[0].transform;
                    break;
                default:
                    NextTarget = CurrentTarget;
                    break;
            }
        }
    }

    public void HandleWalk()
    {
        if (Vector3.Distance(NextTarget.position, transform.position) > 3f)
        {
            _rb.freezeRotation = false;
            transform.LookAt(NextTarget);
            _rb.velocity = (NextTarget.position - transform.position).normalized * 10f;
            
            // Debug.Log("trying look at and velocity " + _rb.velocity);
            CurrentStateType = StateType.Idle;
            NextStateType = StateType.Walk;
            _controller.SetStateType(this);
        }
        
        if (team == GameManager.Instance.PlayableColor && Vector3.Distance(NextTarget.position, transform.position) < 3f)
        {
            if(!isAttacked)
            {
                _rb.velocity = Vector3.zero;
                Debug.Log(name + " " + team + " attacking");
                isAttacked = true;
                StartAttack();
            }
        }
    }

    public void StartAttack()
    {
        CurrentStateType = StateType.Walk;
        NextStateType = StateType.Attack;
        _controller.SetStateType(this);
        
        HitEnemy();
        //Invoke("StopAttack", 1.5f);
    }

    private void HitEnemy()
    {
        if(NextTarget != transform) NextTarget.GetComponent<Health>().GetHit(stats.attack);
        canAttack = false;
    }

    //thats function called by anim events
    public void StopAttack()
    {
        CurrentStateType = StateType.Attack;
        NextStateType = StateType.Idle;
        _controller.SetStateType(this);
        
        Invoke("StopAttackwithDelay", .5f);
    }

    private void StopAttackwithDelay()
    {
        NextTarget = CurrentTarget;
        transform.rotation = new Quaternion(0, 0, 0, 0);
        if(team == TeamColor.Red) transform.rotation = new Quaternion(0, 1, 0, 0);
        _rb.freezeRotation = true;
        _rb.velocity = Vector3.zero;
        
        Debug.Log(name + " " + team + " stop and idle time");
        transform.position = _spawnPos;
    }
}
