using UnityEngine;
using UnityEngine.Serialization;

public class AIMove : MonoBehaviour
{
    //TODO: target bilgisi cardslot'dan red/blue team gibi ayrimlarla yollanacak
    //player blue, enemy red gibi...
    public Card stats;
    public TeamColor team;
    public bool isAttacked;
    
    
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
    }

    private void Start()
    {
        CurrentTarget = transform;
        NextTarget = transform;

        _spawnPos = CurrentTarget.position;
        stats = transform.parent.GetComponent<CardSlot>()._card;
        team = transform.parent.GetComponent<CardSlot>().Team;
        _controller.SetStateType(this);
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
                HandleWalk();
                break;
        }
    }

    public void HandleIdle()
    {
        NextStateType = StateType.Idle;
        _controller.SetStateType(this);
       
        GetTarget();

        if (NextTarget != CurrentTarget && !isAttacked && NextTarget != null)
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
        if (Vector3.Distance(NextTarget.position, transform.position) > 2f)
        {
            _rb.freezeRotation = false;
            transform.LookAt(NextTarget);
            _rb.velocity = (NextTarget.position - transform.position).normalized * 10f;
            
            // Debug.Log("trying look at and velocity " + _rb.velocity);
            CurrentStateType = StateType.Idle;
            NextStateType = StateType.Walk;
            _controller.SetStateType(this);
        }
        
        if (team == GameManager.Instance.PlayableColor && Vector3.Distance(NextTarget.position, transform.position) < 2f)
        {
            if(!isAttacked)
            {
                _rb.velocity = Vector3.zero;
                Debug.Log(name + " " + team + " attacking");
                isAttacked = true;
                Invoke("StartAttack", 1f);
            }
        }
    }

    public void StartAttack()
    {
        CurrentStateType = StateType.Walk;
        NextStateType = StateType.Attack;
        _controller.SetStateType(this);
        
        HitEnemy();
        StopAttack();
    }

    private void HitEnemy()
    {
        if(NextTarget != transform) NextTarget.GetComponent<Health>().GetHit(stats.attack);
    }

    public void StopAttack()
    {
        CurrentStateType = StateType.Attack;
        NextStateType = StateType.Idle;
        
        NextTarget = CurrentTarget;
        transform.rotation = new Quaternion(0, 0, 0, 1);
        _rb.freezeRotation = true;
        _rb.velocity = Vector3.zero;
        
        Debug.Log("stop and idle time");
        transform.position = _spawnPos;
        _controller.SetStateType(this);
    }
}
