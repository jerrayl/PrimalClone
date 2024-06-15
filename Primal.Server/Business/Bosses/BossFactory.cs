using System;
using System.Linq;
using Primal.Business.Constants;
using Primal.Entities;
using Primal.Extensions;
using Primal.Repositories;

namespace Primal.Business.Bosses
{
    public interface IBossFactory
    {
        void CreateBossByNumber(int number, int encounterId);
        AbstractBoss GetBossInstance(Boss bossEntity);
    }

    public class BossFactory : IBossFactory
    {
        private readonly IBossDependencies _bossDependencies;
        private readonly IDatabaseRepository<Boss> _bosses;
        private readonly IDatabaseRepository<BossAction> _bossActions;

        public BossFactory(
            IBossDependencies bossDependencies,
            IDatabaseRepository<Boss> bosses,
            IDatabaseRepository<BossAction> bossActions
        )
        {
            _bossDependencies = bossDependencies;
            _bosses = bosses;
            _bossActions = bossActions;
        }

        public void CreateBossByNumber(int number, int encounterId)
        {
            var bossEntity = number switch
            {
                1 => BroodMother.GetBossEntity(),
                _ => throw new NotImplementedException()
            };
            bossEntity.EncounterId = encounterId;
            _bosses.Add(bossEntity);

            var bossActions = BossActions.BOSS_ACTIONS
                .Select(x => new BossAction { BossId = bossEntity.Id, Stage = x.Stage, Number = x.Number })
                .ToList();
            bossActions.Shuffle();
            bossActions.Sort((a, b) => a.Stage - b.Stage);
            _bossActions.AddBatch(bossActions);
        }

        public AbstractBoss GetBossInstance(Boss bossEntity)
        {
            return bossEntity.Number switch
            {
                1 => new BroodMother(bossEntity, _bossDependencies),
                _ => throw new NotImplementedException()
            };
        }
    }
}