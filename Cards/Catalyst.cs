using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Luminous.Cards;
public class Catalyst : CardModel {
    public override CardPoolModel Pool => ModelDb.CardPool<SilentCardPool>();
    public Catalyst() : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.AnyEnemy) { }
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DynamicVar("Aminous", 2M)];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay) {
        PoisonPower? poison = cardPlay.Target!.GetPower<PoisonPower>();
        if (poison != null) {
            await PowerCmd.ModifyAmount(poison, poison.Amount * (this.DynamicVars["Aminous"].BaseValue - 1), Owner.Creature, this);
        }
    }
    protected override void OnUpgrade() => this.DynamicVars["Aminous"].UpgradeValueBy(1);
}
