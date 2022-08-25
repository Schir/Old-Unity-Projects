using UnityEngine;
using System.Collections;

public class EnemyStateMachine : CharacterStateMachine
{
    private BattleStateMachine BSM;
    public BaseEnemy enemy;
    public enum TurnState
    {
        PROCESSING,
        CHOOSEACTION,
        WAITING,
        ACTION,
        DEAD,
        RUN
    }
    public TurnState currentState;
    //private Vector3 startPosition; // this is vestigial. Just ignore it.
    private bool actionStarted = false;
    public CharacterStats HeroToAttack;
    public Item attackToDo;
    public bool isThisMachineActive = false;

    public void Start()
    {
        
    }
    public void Awake()
    {
        currentState = TurnState.WAITING;
        BSM = GameObject.FindGameObjectWithTag("BattleManager").GetComponent<BattleStateMachine>();
    }
    public void Update()
    {
        switch(currentState)
        {
             case (TurnState.PROCESSING):
                if(isThisMachineActive)
                {
                    if(enemy.isDead)
                    {
                        currentState = TurnState.DEAD;
                    }
                    else
                    {
                    currentState = TurnState.CHOOSEACTION;
                    }
                }
                break;
            
            case (TurnState.CHOOSEACTION):
                ChooseAction();
                break;
            
            case (TurnState.WAITING):
                isThisMachineActive = false;
                break;
            case (TurnState.ACTION):
                //StartCoroutine(TimeForAction());
                break;
            case (TurnState.DEAD):

                break;
            case (TurnState.RUN):
                //do something with morale here.
                break;
        }
    }
    public void ChooseAction()
    {
        TurnHandler myAttack = new TurnHandler();
        myAttack.Attacker = enemy;
        myAttack.TargetOfAttack = BSM.HeroesInBattle[Random.Range(0, BSM.HeroesInBattle.Count)];
        myAttack.Attack = EnemyHolder.chooseRandomAttack(enemy);
        BSM.AddTurnToList(myAttack);
        currentState = TurnState.WAITING;
        //BSM.CollectActions(myAttack);
    }
    public void setEnemyInMachine(BaseEnemy vessel)
    {
        enemy = vessel;
        characterInMachine = enemy;
    }
    public TurnState GetCurrentTurnState()
    {
        return currentState;
    }
    public void MakeThisMachineActive(bool status)
    {
        isThisMachineActive = status;
    }
}