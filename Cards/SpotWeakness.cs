using Luminous.Util;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Luminous.Cards;

[LuminousPool<IroncladCardPool>]
public sealed class SpotWeakness : CardModel {
    public SpotWeakness() : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.AnyEnemy) { }
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<StrengthPower>(3)];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay) {
        if (cardPlay.Target.Monster.IntendsToAttack) {
            await PowerCmd.Apply<StrengthPower>(Owner.Creature, base.DynamicVars["StrengthPower"].BaseValue, Owner.Creature, this);
        }
    }
    protected override void OnUpgrade() => DynamicVars["StrengthPower"].UpgradeValueBy(1);
}