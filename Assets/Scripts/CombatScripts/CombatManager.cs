
using System.Collections.Generic;
using TMPro;
using Unity.Android.Gradle.Manifest;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.UI;

public class CombatManager : MonoBehaviour
{
    public static CombatManager Instance { get; private set; }

    [Header("Values")]
    [SerializeField] private float moveSpeedMod = 0.5f;
    [SerializeField] private float tileEstimateDist = 10.0f;

    [Header("Setup")]
    [SerializeField] private List<GameObject> PlayerParty;
    [SerializeField] private ClassAbility movementAbilityRef;
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
    private ClassAbility processingAction;
    [SerializeField] private Dictionary<ETeam, List<Combatant>> allCombatants = new Dictionary<ETeam, List<Combatant>>();
    [SerializeField] private Dictionary<ETeam, List<Combatant>> teamCombatants = new Dictionary<ETeam, List<Combatant>>();
    [SerializeField] private List<Combatant> combatantsByReverseInitiative;
    private int currentInitiativeIndex = -1;
    public int TotalKilled = 0;
     
    private void sortInitiativeList(){
        combatantsByReverseInitiative.Sort();
    }
    
    private void Awake() {
        if(Instance != null){
            Destroy(this.gameObject);
            return;
        } 
        
        Instance = this;

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

        EmptyCurrentCombatants();
        SpawnPlayerPartyMembers();
        PopulateCurrentCombatants();

        currentCombatTeamCount = 0;
        foreach(KeyValuePair<ETeam, List<Combatant>> team in teamCombatants){
            if(team.Value.Count <= 0) teamCombatants.Remove(team.Key);
            else currentCombatTeamCount++;
        }

        if(currentCombatTeamCount <= 1){
            Debug.LogWarning("[WARN]: Combat attempted to start with 1 or less teams");
            DisablePlayerPartyMembers();
            EmptyCurrentCombatants();
            return;
        }

        Debug.Log("Combat Began!");
        CombatActive = true;
        AwaitingActionSelect = false;

        TileManager.Instance.generateTileMap();

        combatantsByReverseInitiative.Clear();
        foreach(List<Combatant> teamList in teamCombatants.Values){
            foreach(Combatant combatant in teamList){
                combatantsByReverseInitiative.Add(combatant);
                combatant.UpdateCurrentTile();
                combatant.FullHeal();
            }
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
        
        playerTurnBanner.SetActive(false);
        playerActionSelectBanner.SetActive(false);
        enemyTurnBanner.SetActive(false);

        EmptyCurrentCombatants();
        DisablePlayerPartyMembers();
        TileManager.Instance.ClearTileMap();

        if(!teamCombatants.ContainsKey(ETeam.PLAYER_TEAM))
            Debug.LogWarning("[WARN]: TODO: Call the game over screen"); // TODO : Call the game over screen
    }

    private void PlayPlayerTurn(){
        playerTurnBanner.SetActive(true);
        GameObject slot1 = playerActionSelectBanner.transform.GetChild(0).GetChild(0).gameObject;
        GameObject slot2 = playerActionSelectBanner.transform.GetChild(0).GetChild(1).gameObject;
        GameObject slot3 = playerActionSelectBanner.transform.GetChild(0).GetChild(2).gameObject;

        slot1.GetComponent<UnitAction>().classAbility = this.movementAbilityRef;
        slot2.GetComponent<UnitAction>().classAbility = getCurrentCombatant().CombatantClass.Ability1;
        slot3.GetComponent<UnitAction>().classAbility = getCurrentCombatant().CombatantClass.Ability2;

        playerActionSelectBanner.SetActive(true);
        Debug.Log("Player's Turn!");

        AwaitingActionSelect = true;
    }

    private void PlayAITurn(){
        Debug.Log("AI's Turn!");
        if(getCurrentCombatant().GetComponent<Combatant>().Abilities.Count <= 0){
            EndTurn();
            return;
        } 
        
        enemyTurnBanner.SetActive(true);

        int choice = Random.Range(0, getCurrentCombatant().GetComponent<Combatant>().Abilities.Count - 1);
        ClassAbility action = getCurrentCombatant().GetComponent<Combatant>().Abilities[choice];
        setProcessingAction(action);
        
        switch(action.ActionType){
            case EUnitActionTypes.ATTACK:
                int targetChoice = Random.Range(0, getTeamCombatants(ETeam.PLAYER_TEAM).Count - 1);
                processingAction.Target = getTeamCombatants(ETeam.PLAYER_TEAM)[targetChoice];
                action.OnUse();
                break;
            case EUnitActionTypes.HEALING:
                int allyChoice = Random.Range(0, getTeamCombatants(ETeam.ENEMY_AI_TEAM).Count - 1);
                processingAction.Target = getTeamCombatants(ETeam.ENEMY_AI_TEAM)[allyChoice];
                action.OnUse();
                break;
            case EUnitActionTypes.AURA_HEALING:
                processingAction.OnUse();
                break;
        }
        Debug.Log($"{getCurrentCombatant()} used {processingAction} on {processingAction.Target}!");
        EndTurn();
    }

    private void EndTurn(){
        playerTurnBanner.SetActive(false);
        enemyTurnBanner.SetActive(false);

        getCurrentCombatant().UpdateCurrentTile();

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
        
        setProcessingAction(action.classAbility);
        playerActionSelectBanner.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = action.classAbility.Name;
        
        if(action.classAbility.ActionType == EUnitActionTypes.MOVEMENT){
            AwaitingMoveTarget = true;
            AwaitingActionTarget = false;
        } else  if (action.classAbility.ActionType == EUnitActionTypes.AURA_HEALING) {
            AwaitingActionTarget = false;
            AwaitingMoveTarget = false;
            AwaitingActionTarget = false;
            playerActionSelectBanner.SetActive(false);
            processingAction.OnUse();
            EndTurn();
        } else {
            AwaitingMoveTarget = false;
            AwaitingActionTarget = true;
        }
    }

    public void processTargeting(Combatant combatant){
        // Logic for processing the action depending on if AI or player's choice
        if(!AwaitingActionTarget) return;
        if(!isInCombat(combatant)) return;

        int targetDist = TileManager.Instance.getDistance(combatant.CurrentTile, processingAction.User.CurrentTile);
        if(targetDist <= -1) return;
        if(targetDist < processingAction.minimum_range) return;
        if(targetDist > processingAction.maximum_range) return;

        AwaitingActionSelect = false;
        playerActionSelectBanner.SetActive(false);

        Debug.Log($"Used {processingAction.Name} on {combatant}");
        processingAction.Target = combatant;
        AwaitingActionTarget = false;
        processingAction.OnUse();
        EndTurn();
    }

    public IEnumerator<CombatTile> processMovement(CombatTile tile){
        if(!AwaitingMoveTarget) yield break;

        int targetDist = TileManager.Instance.getDistance(tile, processingAction.User.CurrentTile);
        if(targetDist <= -1) yield break;
        if(targetDist < processingAction.minimum_range) yield break;
        if(targetDist > processingAction.maximum_range) yield break;

        AwaitingActionSelect = false;
        AwaitingMoveTarget = false; 
        playerActionSelectBanner.SetActive(false);

        Animator animator = processingAction.User.gameObject.GetComponent<Animator>();
        if(animator != null){
            animator.SetBool("IsMoving", true);
            animator.SetTrigger("Move");
        }

        while(processingAction.User.transform.position != tile.transform.position){
            processingAction.User.transform.position = Vector3.MoveTowards(processingAction.User.transform.position, tile.transform.position, moveSpeedMod * (processingAction.User.CombatantClass.DexMod + 6));
            Debug.Log($"{processingAction.User}: {processingAction.User.transform.position}");
            yield return null;
        }

        if(animator != null) animator.SetBool("IsMoving", false);

        EndTurn();
    }

    private void setProcessingAction(ClassAbility action){
        processingAction = action;
        action.User = this.getCurrentCombatant();
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
        if(!allCombatants.ContainsKey(combatant.GetComponent<Combatant>().CombatantTeam)){
            List<Combatant> teamList = new List<Combatant>{combatant};

            allCombatants.Add(combatant.GetComponent<Combatant>().CombatantTeam, teamList);
        }
        else if (!allCombatants[combatant.GetComponent<Combatant>().CombatantTeam].Contains(combatant))
            allCombatants[combatant.GetComponent<Combatant>().CombatantTeam].Add(combatant);

        if(CombatActive)
            Debug.LogWarning("[WARN]: Combatant added during combat, combatant will not be included in current combat");
    }

    public void AddCombatantToCurrentCombat(Combatant combatant){
        if(!teamCombatants.ContainsKey(combatant.GetComponent<Combatant>().CombatantTeam)){
            List<Combatant> teamList = new List<Combatant>{combatant};

            teamCombatants.Add(combatant.GetComponent<Combatant>().CombatantTeam, teamList);
        }
        else if (!teamCombatants[combatant.GetComponent<Combatant>().CombatantTeam].Contains(combatant))
            teamCombatants[combatant.GetComponent<Combatant>().CombatantTeam].Add(combatant);

        if(CombatActive)
            Debug.LogWarning("[WARN]: Combatant added during combat, combatant will not be included in current combat");
    }

    public void RemoveCombatantFromGame(Combatant combatant){
        if(!allCombatants.ContainsKey(combatant.GetComponent<Combatant>().CombatantTeam)) return;
        if(allCombatants[combatant.CombatantTeam].Contains(combatant)){
            allCombatants[combatant.CombatantTeam].Remove(combatant);
            if(allCombatants[combatant.CombatantTeam].Count <= 0)
                allCombatants.Remove(combatant.CombatantTeam);
        }
    }

    public void RemoveCombatantFromCombat(Combatant combatant){
        if(!teamCombatants.ContainsKey(combatant.GetComponent<Combatant>().CombatantTeam)) return;
        if(teamCombatants[combatant.CombatantTeam].Contains(combatant)){
            teamCombatants[combatant.CombatantTeam].Remove(combatant);
            if(teamCombatants[combatant.CombatantTeam].Count <= 0)
                teamCombatants.Remove(combatant.CombatantTeam);

            combatantsByReverseInitiative.Remove(combatant);
            sortInitiativeList();
        }

        RemoveCombatantFromGame(combatant);
    }

    public List<Combatant> getTeamCombatants(ETeam team){
        if(teamCombatants.ContainsKey(team)){
            return teamCombatants[team];
        }

        Debug.LogWarning("[WARN]: Trying to access non-existent team");
        return null;
    }

    public void AddAsPlayerAlly(GameObject character){
        Combatant combatant = character.GetComponent<Combatant>();
        if(combatant == null) return;
        RemoveCombatantFromGame(combatant);
        combatant.CombatantTeam = ETeam.PLAYER_TEAM;
        AddCombatant(combatant);
        this.PlayerParty.Add(character);
        Debug.Log($"Added {character} to Party!");
        DisablePlayerPartyMembers();
    }

    public void CyclePlayer(){
        if(PlayerParty.Count <= 1) return;

        GameObject prevPlayer = PlayerParty[0];
        prevPlayer.SetActive(false);
        for(int i = 1; i < PlayerParty.Count; i++){
            PlayerParty[i - 1] = PlayerParty[i];
        }
        PlayerParty[PlayerParty.Count - 1] = prevPlayer;
        PlayerParty[0].GetComponent<Combatant>().CurrentArea = prevPlayer.GetComponent<Combatant>().CurrentArea;
        PlayerParty[0].SetActive(true);
    }

    private void DisablePlayerPartyMembers(){
        for(int i = 1; i < PlayerParty.Count; i++)
            PlayerParty[i].SetActive(false);
    }
    private void SpawnPlayerPartyMembers(){
        for(int i = 1; i < PlayerParty.Count; i++){
            PlayerParty[i].transform.position = PlayerParty[0].transform.position;
            PlayerParty[i].GetComponent<Combatant>().CurrentArea = PlayerParty[0].GetComponent<Combatant>().CurrentArea;
            PlayerParty[i].SetActive(true);
        }

    }

    private void EmptyCurrentCombatants(){
        foreach(List<Combatant> team in teamCombatants.Values){
            team.Clear();
        }

        teamCombatants.Clear();
    }

    private bool isInCombat(Combatant combatant){
        return combatant.CurrentArea == PlayerParty[0].GetComponent<Combatant>().CurrentArea;
    }

    private void PopulateCurrentCombatants(){
        ESubAreas playerArea = PlayerParty[0].GetComponent<Combatant>().CurrentArea;
        foreach(List<Combatant> team in allCombatants.Values){
            foreach(Combatant combatant in team){
                if(combatant.CurrentArea == playerArea)
                    AddCombatantToCurrentCombat(combatant);
            }
        }
    }
}
