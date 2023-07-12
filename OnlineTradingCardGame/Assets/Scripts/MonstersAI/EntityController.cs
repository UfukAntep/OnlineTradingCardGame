using UnityEngine;


public class EntityController : MonoBehaviour
{
    private StateMachine _stateMachine;
    public Animator Animator;
    
    [SerializeField] private StateType _currentState;
    [SerializeField] private StateType _nextState;

    private AIMove _monster;
    private void Awake()
    {
        _stateMachine = new StateMachine();
        Animator = GetComponent<Animator>();
    }

    void Start()
    {
        IState idleState = new IdleState(this);
        IState walkState = new WalkState(this);
        IState attackState = new AttackState(this);

        _stateMachine.SetNormalStates(idleState, walkState, () => _nextState == StateType.Walk);
        
        _stateMachine.SetNormalStates(walkState, idleState, () => _nextState == StateType.Idle);
        
        _stateMachine.SetNormalStates(walkState,attackState, () => _nextState == StateType.Attack);
        
        _stateMachine.SetNormalStates(attackState,walkState, () => _nextState == StateType.Walk);
        
        _stateMachine.SetNormalStates(attackState,idleState, () => _nextState == StateType.Idle);
        
        _stateMachine.SetAnyStates(idleState, () => _nextState == StateType.Idle);
    }

    public void SetStateType(AIMove aiMove)
    {
        _currentState = aiMove.CurrentStateType;
        _nextState = aiMove.NextStateType;
    }
    
    void Update()
    {
        _stateMachine.Tick();
    }

    public void ChangeState()
    {
        _currentState = _nextState;
    }
}