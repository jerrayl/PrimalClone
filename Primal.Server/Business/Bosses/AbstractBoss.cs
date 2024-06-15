using System;
using System.Linq;
using System.Collections.Generic;
using Primal.Entities;
using Primal.Models;
using Primal.Extensions;
using Primal.Business.Helpers;

namespace Primal.Business.Bosses
{
    public class BodyMapping
    {
        public Position RelativePosition { get; set; }
        public Direction? RelativeDirection { get; set; }
        public Corner? Corner { get; set; }
    }

    public class PartMapping
    {
        public List<Direction?> RelativeDirections { get; set; }
        public (BossPart, int) BossPart { get; set; }
        public BossPart? Break { get; set; }
    }

    public class ActionComponent
    {
    }

    public class MoveActionComponent : ActionComponent
    {
        public string ExtraEffect { get; set; }
        public Direction? Direction { get; set; }
        public int Spaces { get; set; }
    }

    public class AttackActionComponent : ActionComponent
    {
        public BossPart BossPart { get; set; }
        public int Range { get; set; }
        public Template? Template { get; set; }
        public int Size { get; set; }
        public string ExtraEffect { get; set; }
    }

    public class SpecialActionComponent : ActionComponent
    {
        public int Parameter { get; set; }
    }

    public class CustomActionComponent : ActionComponent
    {
        public string ExtraEffect { get; set; }
    }

    public class Action
    {
        public int Number { get; set; }
        public int Stage { get; set; }
        public string Name { get; set; }
        public List<ActionComponent> Components { get; set; }
    }

    public abstract class AbstractBoss
    {
        public AbstractBoss(Boss bossEntity, IBossDependencies bossDependencies)
        {
            BossEntity = bossEntity;
            BossDependencies = bossDependencies;
        }

        protected IBossDependencies BossDependencies { get; init; }
        protected Boss BossEntity { get; init; }
        protected abstract List<PartMapping> PartMappings { get; }
        protected abstract List<BodyMapping> BodyMappings { get; }
        public abstract string Name { get; }
        protected abstract Size Size { get; }


        private bool IsBroken(BossPart bossPart)
        {
            return PartMappings.Where(m => m.Break == bossPart).Any(m => BossEntity.Health[m.BossPart.ConvertToString()] <= 0);
        }

        public List<BossPartPosition> GetBossPositions()
        {
            var bossPartPositions = new List<BossPartPosition>();
            foreach (var partMapping in PartMappings)
            {
                foreach (var relativeDirection in partMapping.RelativeDirections)
                {
                    var bodyMapping = BodyMappings.Single(x => x.RelativeDirection == relativeDirection);
                    bossPartPositions.Add(
                        new BossPartPosition(
                            BossEntity.XPosition + bodyMapping.RelativePosition.XPosition,
                            BossEntity.YPosition + bodyMapping.RelativePosition.YPosition
                        )
                        {
                            Direction = bodyMapping.RelativeDirection,
                            Corner = bodyMapping.Corner,
                            BossPart = partMapping.BossPart.ConvertToString()
                        }
                    );
                }
            }

            return bossPartPositions;
        }

        public (BossPart, int) GetBossPartFromPosition(IPosition position)
        {
            var bodyPosition = BodyMappings
                .Where(m =>
                    m.RelativePosition.XPosition == position.XPosition - BossEntity.XPosition &&
                    m.RelativePosition.YPosition == position.YPosition - BossEntity.YPosition
                )
                .Single();

            return PartMappings.Single(x => x.RelativeDirections.Contains(bodyPosition.RelativeDirection)).BossPart;
        }

        private Dictionary<Might, int> GetMight(BossPart bossPart)
        {
            if (!IsBroken(bossPart))
            {
                return BossEntity.Might;
            }

            var deductedFromHighest = false;
            var bossMight = new Dictionary<Might, int>();

            foreach (var might in new List<Might>() { Might.Black, Might.Red, Might.Yellow, Might.White })
            {
                if (!deductedFromHighest && BossEntity.Might[might] > 0)
                {
                    bossMight[might] = BossEntity.Might[might] - 1;
                    deductedFromHighest = true;
                }
                bossMight[might] = BossEntity.Might[might];
            }
            return bossMight;
        }

        protected int GetNearestPlayer()
        {
            // Ignore normal sized enemies that need to path through obstacles for now
            var players = BossDependencies.EncounterPlayers.Read(x => x.EncounterId == BossEntity.EncounterId);
            var nearestPlayer = GridHelper.GetNearest(BossEntity, players) ?? throw new Exception("No player could be found");
            // If there is no nearest player in range, set a direction instead
            return ((EncounterPlayer)nearestPlayer).Id;
        }

        public void BeginAction()
        {
            if (BossEntity.ActionComponentIndex is not null)
            {
                throw new Exception("Invalid Boss Action Component Index");
            }

            var encounter = BossDependencies.Encounters.ReadOne(x => x.Id == BossEntity.EncounterId);
            encounter.CharacterPerformingAction = CharacterType.Boss;

            if (BossEntity.TargetId is null) // Not a retailiation
            {
                BossEntity.TargetId = GetDefaultTarget();
            }
            BossEntity.ActionComponentIndex = 0;

            BossDependencies.Bosses.Update(BossEntity);
            BossDependencies.Encounters.Update(encounter);

            PerformAction();
        }

        public void PerformAction()
        {
            var action = GetAction(GetNextAction().Number);
            if (BossEntity.ActionComponentIndex > action.Components.Count - 1)
            {
                EndAction();
            }
            else
            {
                PerformActionComponent(action.Components[BossEntity.ActionComponentIndex.Value]);
            }
            BossDependencies.Bosses.Update(BossEntity);
        }

        private void EndAction()
        {
            var encounter = BossDependencies.Encounters.ReadOne(x => x.Id == BossEntity.EncounterId);

            if (encounter.CharacterPerformingAction != CharacterType.Boss)
            {
                throw new Exception("Invalid character performing action");
            }

            encounter.CharacterPerformingAction = null;

            BossEntity.TargetId = null;
            BossEntity.ActionComponentIndex = null;

            BossDependencies.Encounters.Update(encounter);
            BossDependencies.Bosses.Update(BossEntity);
            BossDependencies.BossActions.Delete(GetNextAction());
        }

        private void PerformActionComponent(ActionComponent actionComponent)
        {
            switch (actionComponent)
            {
                case MoveActionComponent moveComponent:
                    PerformMove(moveComponent);
                    BossEntity.ActionComponentIndex++;
                    break;
                case AttackActionComponent attackComponent:
                    PerformAttack(attackComponent);
                    break;
                case SpecialActionComponent specialComponent:
                    PerformSpecialAction(specialComponent);
                    BossEntity.ActionComponentIndex++;
                    break;
                case CustomActionComponent customComponent:
                    PerformCustomAction(customComponent);
                    BossEntity.ActionComponentIndex++;
                    break;
            }
        }

        private bool CanMove(IPosition movementVector)
        {
            return GetBossPositions().All(x => GridHelper.IsValidPosition(x.Add(movementVector)));
        }

        private void PerformMove(MoveActionComponent actionComponent)
        {
            if (Size == Size.Normal)
            {
                throw new NotImplementedException("Obstacle pathing for normal sized bosses has not been implemented");
            }

            if (BossEntity.TargetId is null && actionComponent.Direction is null)
            {
                throw new Exception("Target could not be found");
            }

            if (actionComponent.Direction is null) // Move to target
            {
                var target = BossDependencies.EncounterPlayers.ReadOne(x => x.Id == BossEntity.TargetId);
                var closestBossHex = GridHelper.GetNearest(target, GetBossPositions());
                var relativeDirection = GridHelper.GetRelativeDirection(target, closestBossHex);
                var validDirection = GridHelper.TryAllDirections((Direction d) =>
                {
                    var destinationHex = GridHelper.GetTemplate(target, Template.Line, 1, d);
                    return destinationHex.Any() && CanMove(destinationHex.First().Subtract(closestBossHex));
                }, relativeDirection);
                var destinationHex = GridHelper.GetTemplate(target, Template.Line, 1, validDirection).First();
                var movementVector = destinationHex.Subtract(closestBossHex);
                BossEntity.XPosition += movementVector.XPosition;
                BossEntity.YPosition += movementVector.YPosition;
                BossEntity.Direction = GridHelper.GetRelativeDirection(destinationHex, target);
            }
            else
            {
                var closestBossHex = new Position(BossEntity.XPosition, BossEntity.YPosition).Add(BodyMappings.Single(x => x.RelativeDirection == actionComponent.Direction).RelativePosition);
                var totalMovementLine = new List<IPosition>();
                var movementLine = GridHelper.GetTemplate(closestBossHex, Template.Line, actionComponent.Spaces, actionComponent.Direction);
                while (movementLine.Count < actionComponent.Spaces)
                {
                    totalMovementLine.AddRange(movementLine);
                    actionComponent.Spaces -= movementLine.Count;
                    actionComponent.Direction = GridHelper.Bounce(actionComponent.Direction.Value);
                    movementLine = GridHelper.GetTemplate(movementLine.Last(), Template.Line, actionComponent.Spaces);
                }
                if (!CanMove(movementLine.Last().Subtract(closestBossHex)))
                {
                    throw new Exception("Boss could not move in direction");
                }

                var destinationHex = movementLine.Last();
                var movementVector = destinationHex.Subtract(closestBossHex);
                BossEntity.XPosition += movementVector.XPosition;
                BossEntity.YPosition += movementVector.YPosition;
                BossEntity.Direction = actionComponent.Direction.Value;
            }

            BossDependencies.Bosses.Update(BossEntity);

            // Destroy obstacles 

            var players = BossDependencies.EncounterPlayers.Read();
            var newBossPositions = GetBossPositions();
            var overlappingPlayers = players.Where(p => GridHelper.IsOverlapping(p, newBossPositions)).ToList();
            overlappingPlayers.Sort(GridHelper.NorthWestiestComparer);

            foreach (var player in overlappingPlayers)
            {
                var validCombination = GridHelper.TryAllDirectionsAndDistances((Direction direction, int distance) =>
                {
                    var destinationHex = GridHelper.GetTemplate(player, Template.Line, distance, direction);
                    return destinationHex.Any() && !GridHelper.IsOverlapping(destinationHex.Single(), players, newBossPositions, overlappingPlayers);
                }, newBossPositions.Single(b => b.EqualTo(player)).Direction ?? BossEntity.Direction);

                var destinationHex = GridHelper.GetTemplate(player, Template.Line, validCombination.Item2, validCombination.Item1).Single();
                player.XPosition = destinationHex.XPosition;
                player.YPosition = destinationHex.YPosition;
            }

            BossDependencies.EncounterPlayers.UpdateBatch(overlappingPlayers);
        }

        private void PerformAttack(AttackActionComponent actionComponent)
        {
            var existingAttack = BossDependencies.BossAttacks.ReadOne(x => x.BossId == BossEntity.Id);
            if (existingAttack is not null)
            {
                CompleteAttack(existingAttack.Id);
                return;
            }

            // Assume single target
            var targets = new List<int> { BossEntity.TargetId.Value };

            var attack = new BossAttack() { BossId = BossEntity.Id };
            BossDependencies.BossAttacks.Add(attack);

            foreach (var target in targets)
            {
                BossDependencies.BossAttackPlayers.Add(new BossAttackPlayer() { BossAttackId = attack.Id, PlayerId = target });
            }

            var bossMightDeck = BossDependencies.EncounterMightDecks
                .ReadOne(x => x.EncounterId == BossEntity.EncounterId && !x.IsFreeCompanyDeck);

            BossDependencies.MightCardsService.DrawCards(bossMightDeck.Id, attack.Id, CharacterType.Boss, GetMight(actionComponent.BossPart));
        }

        public void CompleteAttack(int attackId)
        {
            var attack = BossDependencies.BossAttacks
                .ReadOne(x => x.Id == attackId, x => x.MightCards, x => x.BossAttackPlayers) ?? throw new Exception("Invalid attack id");
            var encounterPlayers = BossDependencies.EncounterPlayers
                .Read(p => p.EncounterId == BossEntity.EncounterId && attack.BossAttackPlayers.Any(x => x.PlayerId == p.Id), x => x.Player);

            foreach (var encounterPlayer in encounterPlayers)
            {
                // account for cards played in defence
                encounterPlayer.CurrentHealth -= attack.MightCards.Sum(x => x.Value) / encounterPlayer.Player.Defence;
                BossDependencies.EncounterPlayers.Update(encounterPlayer);
                // handle scenario where player health drops below 1
            }
            BossDependencies.MightCards.DeleteBatch(attack.MightCards);
            BossDependencies.BossAttackPlayers.DeleteBatch(attack.BossAttackPlayers);
            BossDependencies.BossAttacks.Delete(attack);

            BossEntity.ActionComponentIndex++;
        }

        private BossAction GetNextAction()
        {
            //temporarily ignore action shuffling
            return BossDependencies.BossActions.Read(x => x.BossId == BossEntity.Id).OrderBy(x => x.Id).First();
        }

        public List<string> GetNextActionText()
        {
            return GetActionText(GetNextAction().Number);
        }

        protected abstract int GetStage();
        protected abstract int GetDefaultTarget();
        protected abstract Action GetAction(int number);
        protected abstract List<string> GetActionText(int number);
        protected abstract void PerformSpecialAction(SpecialActionComponent actionComponent);
        protected abstract void PerformCustomAction(CustomActionComponent actionComponent);
        protected abstract void PerformStartOfRoundActions();
        protected abstract void PerformEndOfRoundActions();
    }
}