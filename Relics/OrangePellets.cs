using Luminous.Util;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.Rooms;

namespace Luminous.Relics;

[LuminousPool<SharedRelicPool>]
public sealed class OrangePellets : RelicModel {
    public override RelicRarity Rarity => RelicRarity.Shop;
    private int AttacksPlayedThisTurn {
        get; set;
    }

    private int SkillsPlayedThisTurn {
        get; set;
    }

    private int PowersPlayedThisTurn {
        get; set;
    }

    private int ActivationCountThisTurn {
        get; set;
    }

    public override Task BeforeSideTurnStart(PlayerChoiceContext choiceContext, CombatSide side, CombatState combatState) {
        if (side != Owner.Creature.Side) {
            return Task.CompletedTask;
        }
        AttacksPlayedThisTurn = 0;
        SkillsPlayedThisTurn = 0;
        PowersPlayedThisTurn = 0;
        ActivationCountThisTurn = 0;
        return Task.CompletedTask;
    }

    public override async Task AfterCardPlayed(PlayerChoiceContext context, CardPlay cardPlay) {
        if (cardPlay.Card.Owner == Owner && CombatManager.Instance.IsInProgress && ActivationCountThisTurn < 1) {
            AttacksPlayedThisTurn += ((cardPlay.Card.Type == CardType.Attack) ? 1 : 0);
            SkillsPlayedThisTurn += ((cardPlay.Card.Type == CardType.Skill) ? 1 : 0);
            PowersPlayedThisTurn += ((cardPlay.Card.Type == CardType.Power) ? 1 : 0);
            if (AttacksPlayedThisTurn > 0 && SkillsPlayedThisTurn > 0 && PowersPlayedThisTurn > 0) {
                Flash();
                foreach (PowerModel power in Owner.Creature.Powers.ToList()) {
                    if (power.Type == PowerType.Debuff)
                        await PowerCmd.Remove(power);
                }
                ActivationCountThisTurn++;
            }
        }
    }

    public override Task AfterCombatEnd(CombatRoom _) {
        AttacksPlayedThisTurn = 0;
        SkillsPlayedThisTurn = 0;
        PowersPlayedThisTurn = 0;
        ActivationCountThisTurn = 0;
        return Task.CompletedTask;
    }
}
