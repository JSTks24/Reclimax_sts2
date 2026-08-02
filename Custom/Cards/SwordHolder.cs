#if ANGEL_COURTYARD
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace Luminous.Custom.Cards;
public sealed class SwordHolder : CardModel {
    public SwordHolder() : base(1, CardType.Power, CardRarity.Rare, TargetType.Self) { }
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay) {
        await PowerCmd.Apply<SwordHolderPower>(base.Owner.Creature, 1m, base.Owner.Creature, this);
    }
    protected override void OnUpgrade() => AddKeyword(CardKeyword.Retain);
}

public sealed class SwordHolderPower : PowerModel {
    private HashSet<CardModel>? _autoplayingCards;
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Single;
    private HashSet<CardModel> AutoplayingCards {
        get {
            AssertMutable();
            if (_autoplayingCards == null) {
                _autoplayingCards = new HashSet<CardModel>();
            }
            return _autoplayingCards;
        }
    }
    public override async Task AfterCardDrawnEarly(PlayerChoiceContext choiceContext, CardModel card, bool fromHandDraw) {
        if (card.Owner.Creature == base.Owner && card.Tags.Contains(CardTag.Strike) && !base.Owner.CombatState.HittableEnemies.All((Creature c) => c.ShowsInfiniteHp)) {
            AutoplayingCards.Add(card);
            await CardCmd.AutoPlay(choiceContext, card, null);
            AutoplayingCards.Remove(card);
        }
    }
    public override Task BeforeAttack(AttackCommand command) {
        if (!AutoplayingCards.Contains(command.ModelSource)) {
            return Task.CompletedTask;
        }
        command.WithHitFx("vfx/hellraiser_attack_vfx", command.HitSfx, command.TmpHitSfx).WithAttackerAnim("Cast", command.Attacker.Player.Character.CastAnimDelay).SpawningHitVfxOnEachCreature()
            .WithHitVfxSpawnedAtBase();
        return Task.CompletedTask;
    }
}
#endif