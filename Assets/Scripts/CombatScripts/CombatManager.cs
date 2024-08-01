
using System.Collections.Generic;
using TMPro;
using Unity.Android.Gradle.Manifest;
using UnityEngine;
using UnityEngine.UI;

public class CombatManager : MonoBehaviour
{
    public static CombatManager Instance { get; private set; }

    [Header("Setup")]
    [SerializeField] private GameObject playerTurnBanner;
    [SerializeField] private GameObject enemyTurnBanner;
    [SerializeField] private GameObject playerActionSelectBanner;

    [Header("Debug")]
    [SerializeField] private bool debugStartCombat = false;

    [Header("CombatInfo")]
    public bool CombatActive {get; private set;} = false;
    private int currentCombatTeamCount;
    private bool AwaitingActionSelect{get; set;} = false;
    private bool AwaitingActionTarget{get; set;} = false;
    private bool AwaitingMoveTarget{get; set;} = false;
    private UnitAction processingAction;
    [SerializeField] private Dictionary<ETeam, List<Combatant>> teamCombatants = new Dictionary<ETeam, List<Combatant>>();
    [SerializeField] private List<Combatant> combatantsByReverseInitiative;
    private int currentInitiativeIndex = -1;
     
    private void sortInitiativeList(){
        combatantsByReverseInitiative.Sort();
    }
    
    private void Awake() {
        if(Instance != null){
            Destroy(this.gameObject);
            return;
        } 
        
        Instance = this;
        DontDestroyOnLoad(Instance);

        playerTurnBanner.SetActive(false);
        enemyTurnBanner.SetActive(false);
        playerActionSelectBanner.SetActive(false);
    }

    private void Update(){
        if(debugStartCombat){
            debugStartCombat = false;
            this.BeginCombat();
        }
    }

    public void BeginCombat(){
        if(CombatActive){
            Debug.LogWarning("[WARN]: Combat attempted to begin while another was running");
            return;
        }

        currentCombatTeamCount = 0;

        foreach(KeyValuePair<ETeam, List<Combatant>> team in teamCombatants){
            if(team.Value.Count <= 0) teamCombatants.Remove(team.Key);
            else currentCombatTeamCount++;
        }

        if(currentCombatTeamCount <= 1){
            Debug.LogWarning("[WARN]: Combat attempted to start with 1 or less teams");
            return;
        }

        Debug.Log("Combat Began!");
        CombatActive = true;
        AwaitingActionSelect = false;

        combatantsByReverseInitiative.Clear();
        foreach(List<Combatant> teamList in teamCombatants.Values){
            foreach(Combatant combatant in teamList)
                combatantsByReverseInitiative.Add(combatant);
        }
        sortInitiativeList();
        currentInitiativeIndex = combatantsByReverseInitiative.Count - 1;

        if(getCurrentCombatant().GetComponent<Combatant>().CombatantTeam == ETeam.PLAYER_TEAM) 
            PlayPlayerTurn();
        else 
            PlayAITurn();
    }


    private void EndCombat(){
        CombatActive = false;

        if(!teamCombatants.ContainsKey(ETeam.PLAYER_TEAM))
            Debug.LogWarning("[WARN]: TODO: Call the game over screen"); // TODO : Call the game over screen
    }

    private void PlayPlayerTurn(){
        playerTurnBanner.SetActive(true);
        GameObject slot1 = playerActionSelectBanner.transform.GetChild(0).GetChild(0).gameObject;
        GameObject slot2 = playerActionSelectBanner.transform.GetChild(0).GetChild(1).gameObject;
        GameObject slot3 = playerActionSelectBanner.transform.GetChild(0).GetChild(2).gameObject;

        slot2.GetComponent<UnitAction>().classAbility = getCurrentCombatant().CombatantClass.Ability1;
        slot3.GetComponent<UnitAction>().classAbility = getCurrentCombatant().CombatantClass.Ability2;

        playerActionSelectBanner.SetActive(true);

        Debug.Log("Player's Turn!");
        AwaitingActionSelect = true;
    }

    private void PlayAITurn(){
        Debug.Log("AI's Turn!");
        if(getCurrentCombatant().GetComponent<Combatant>().UnitActions.Count <= 0){
            EndTurn();
            return;
        } 
        
        enemyTurnBanner.SetActive(true);

        // TODO : Code AI in determining what action to take
        int choice = Random.Range(0, getCurrentCombatant().GetComponent<Combatant>().UnitActions.Count - 1);
        UnitAction action = getCurrentCombatant().GetComponent<Combatant>().UnitActions[choice];
        setProcessingAction(action);
        
        switch(action.classAbility.ActionType){
            case EUnitActionTypes.MOVEMENT:
                AwaitingMoveTarget = true;
                break;
            case EUnitActionTypes.ATTACK:
            case EUnitActionTypes.HEALING:
                AwaitingActionTarget = true;
                break;
            case EUnitActionTypes.AURA_HEALING:
                // play aura healing
                break;
        }
    }

    public void processTargeting(Combatant combatant){
        // Logic for processing the action depending on if AI or player's choice
        if(!AwaitingActionTarget) return;
        playerActionSelectBanner.SetActive(false);

        Debug.Log($"Used {processingAction.classAbility.Name} on {combatant}");
        AwaitingActionSelect = false;
        EndTurn();
    }

    private void processMove(){

    }

    private void EndTurn(){
        playerTurnBanner.SetActive(false);
        enemyTurnBanner.SetActive(false);

        if(!CanCombatContinue()){
            EndCombat();
            return;
        }

        if(--currentInitiativeIndex < 0)
            currentInitiativeIndex = combatantsByReverseInitiative.Count - 1;

        if(getCurrentCombatant().GetComponent<Combatant>().CombatantTeam == ETeam.PLAYER_TEAM) 
            PlayPlayerTurn();
        else 
            PlayAITurn();        
    }

    public void UnitActionTapped(UnitAction action){
        if(!CombatActive){
            Debug.LogWarning("[WARN]: Action tapped while combat is not active");
            return;
        } 
        if(!AwaitingActionSelect){
            Debug.LogWarning("[WARN]: Action tapped while not awaiting action");
            return;
        }
        
        setProcessingAction(action);
        playerActionSelectBanner.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = action.classAbility.Name;
        // if action is targeting
        // AwaitingMoveTarget = true;
        AwaitingActionTarget = true;
    }

    private void setProcessingAction(UnitAction action){
        processingAction = action;
        action.classAbility.User = this.getCurrentCombatant();
        // Change the canvas to match the selected action depending on the turn
    }

    private bool CanCombatContinue(){
        if(currentCombatTeamCount <= 1) return false;

        int checkTeamCount = 0;
        foreach(KeyValuePair<ETeam, List<Combatant>> team in teamCombatants){
            foreach(Combatant combatant in team.Value){
                if(combatant.CurrentHealth <= 0){
                    team.Value.Remove(combatant);
                    combatantsByReverseInitiative.Remove(combatant);
                    sortInitiativeList();
                }
            }

            if(team.Value.Count <= 0) teamCombatants.Remove(team.Key);
            else checkTeamCount++;
        }
        if(checkTeamCount <= 1) return false;

        currentCombatTeamCount = checkTeamCount;
        return true;
    }

    private Combatant getCurrentCombatant(){
        return combatantsByReverseInitiative[currentInitiativeIndex];
    }

    public void AddCombatant(Combatant combatant){
        if(!teamCombatants.ContainsKey(combatant.GetComponent<Combatant>().CombatantTeam)){
            List<Combatant> teamList = new List<Combatant>{combatant};

            teamCombatants.Add(combatant.GetComponent<Combatant>().CombatantTeam, teamList);
        }
        else if (!teamCombatants[combatant.GetComponent<Combatant>().CombatantTeam].Contains(combatant))
            teamCombatants[combatant.GetComponent<Combatant>().CombatantTeam].Add(combatant);

        if(CombatActive)
            Debug.LogWarning("[WARN]: Combatant added during combat, combatant will not be included in current combat");
    }

    public void RemoveCombatant(Combatant combatant){
        if(!teamCombatants.ContainsKey(combatant.GetComponent<Combatant>().CombatantTeam)) return;
        if(teamCombatants[combatant.CombatantTeam].Contains(combatant)){
            teamCombatants[combatant.CombatantTeam].Remove(combatant);
            if(teamCombatants[combatant.CombatantTeam].Count <= 0)
                teamCombatants.Remove(combatant.CombatantTeam);

            combatantsByReverseInitiative.Remove(combatant);
            sortInitiativeList();
        }
    }

    public List<Combatant> getTeamCombatants(ETeam team){
        if(teamCombatants.ContainsKey(team)){
            return teamCombatants[team];
        }

        Debug.LogWarning("[WARN]: Trying to access non-existent team");
        return null;
    }
}
