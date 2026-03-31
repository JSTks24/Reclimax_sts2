using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Luminous.Cards;

class LimitBreak : CardModel {
    public override CardPoolModel Pool => ModelDb.CardPool<IroncladCardPool>();
    public LimitBreak() : base(1, CardType.Skill, CardRarity.Rare, TargetType.Self) { }
    protected override IEnumerable<DynamicVar> CanonicalVars => base.CanonicalVars;
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<StrengthPower>()];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay) {
        StrengthPower? strength = this.Owner.Creature.GetPower<StrengthPower>();
        if (strength != null) {
            await PowerCmd.ModifyAmount(strength, strength.Amount, Owner.Creature, this);
        }
    }
    protected override void OnUpgrade() => RemoveKeyword(CardKeyword.Exhaust);
}