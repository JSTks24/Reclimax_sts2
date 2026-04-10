using Luminous.Util;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.ValueProps;

namespace Luminous.Cards;

[LuminousPool<IroncladCardPool>]
public sealed class Brutality : CardModel {
    public Brutality() : base(0, CardType.Skill, CardRarity.Rare, TargetType.Self) { }
     protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(1)];
     protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay) {
        (await PowerCmd.Apply<BrutalityPower>(base.Owner.Creature, base.DynamicVars.Cards.BaseValue, base.Owner.Creature, this))?.IncrementSelfDamage();
    }
     protected override void OnUpgrade() => AddKeyword(CardKeyword.Innate);
}

public sealed class BrutalityPower : PowerModel {
    public override PowerStackType StackType => PowerStackType.Counter;
    public override PowerType Type => PowerType.Buff;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar("SelfDamage", 0m, ValueProp.Unblockable | ValueProp.Unpowered)];
    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player) {
        if (player == base.Owner.Player) {
            NCombatRoom.Instance?.CombatVfxContainer.AddChildSafely(NFireSmokePuffVfx.Create(base.Owner));
            await Cmd.CustomScaledWait(0.2f, 0.4f);
            DamageVar damageVar = (DamageVar)base.DynamicVars["SelfDamage"];
            await CreatureCmd.Damage(choiceContext, base.Owner, damageVar.BaseValue, damageVar.Props, base.Owner, null);
        }
    }
    public void IncrementSelfDamage() {
        DynamicVars["SelfDamage"].BaseValue++;
    }
    public override decimal ModifyHandDraw(Player player, decimal count) {
        if (player != base.Owner.Player) {
            return count;
        }
        return count + (decimal)base.Amount;
    }
}