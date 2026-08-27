using UnityEngine;

public enum NPCType { Trainer, Shopkeeper, Healer, QuestGiver, Breeder }

public class NPCController : MonoBehaviour
{
    public NPCType npcType;

    public void Interact()
    {
        switch (npcType)
        {
            case NPCType.Trainer:
                // TODO: start a scripted trainer battle via BattleSystem.
                break;
            case NPCType.Shopkeeper:
                // TODO: open the shop UI.
                break;
            case NPCType.Healer:
                // TODO: fully heal the player's party.
                break;
            case NPCType.QuestGiver:
                // TODO: open quest dialogue/offer.
                break;
            case NPCType.Breeder:
                // TODO: open the breeding facility UI.
                break;
        }
    }
}
