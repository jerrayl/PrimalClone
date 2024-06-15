using System;
using System.Collections.Generic;
using System.Linq;
using Primal.Entities;
using Primal.Extensions;
using Primal.Models;

namespace Primal.Business.Bosses
{
    public class BroodMother : AbstractBoss
    {
        private static readonly List<BodyMapping> BODY_MAPPINGS = new()
        {
            new (){ RelativePosition = new Position(0, -1), RelativeDirection = Direction.North },
            new (){ RelativePosition = new Position(1, -1), RelativeDirection = Direction.NorthEast },
            new (){ RelativePosition = new Position(1, 0), RelativeDirection = Direction.SouthEast },
            new (){ RelativePosition = new Position(0, 1), RelativeDirection = Direction.South },
            new (){ RelativePosition = new Position(-1, 1), RelativeDirection = Direction.SouthWest },
            new (){ RelativePosition = new Position(-1, 0), RelativeDirection = Direction.NorthWest },
            new (){ RelativePosition = new Position(0, 0), RelativeDirection = null }
        };

        private static readonly List<PartMapping> PART_MAPPINGS = new()
        {
            new (){ RelativeDirections = new(){ Direction.North }, BossPart = (BossPart.Front, 0), Break = BossPart.Front},
            new (){ RelativeDirections = new() { Direction.NorthEast, Direction.SouthEast }, BossPart = (BossPart.RightFlank, 0)},
            new (){ RelativeDirections = new(){ Direction.South }, BossPart = (BossPart.Rear, 0), Break = BossPart.Rear},
            new (){ RelativeDirections = new(){ Direction.SouthWest, Direction.NorthWest }, BossPart = (BossPart.LeftFlank, 0), Break = BossPart.Flank },
            new (){ RelativeDirections = new(){ null }, BossPart = (BossPart.Core, 0), Break = BossPart.Core }
        };

        private static readonly List<Action> ACTIONS = new()
        {
            new (){ Number = 1, Stage = 1, Name = "Gnaw", Components = new(){
                new SpecialActionComponent { },
                new MoveActionComponent{ Spaces = 7 },
                new AttackActionComponent{ BossPart = BossPart.Front },
                new MoveActionComponent { Spaces = 3, Direction = Direction.SouthEast }
            }},
            new (){ Number = 2, Stage = 1, Name = "Gnaw", Components = new(){
                new SpecialActionComponent { },
                new MoveActionComponent{ Spaces = 7 },
                new AttackActionComponent{ BossPart = BossPart.Front },
                new MoveActionComponent { Spaces = 3, Direction = Direction.SouthWest }
            }},
            new (){ Number = 3, Stage = 1, Name = "Acidic Maw", Components = new(){
                new SpecialActionComponent { },
                new MoveActionComponent{ Spaces = 5 },
                new AttackActionComponent{ BossPart = BossPart.Front, ExtraEffect = "The character loses once defence for each health lost. Lasts the rest of the encounter" },
                new MoveActionComponent { Spaces = 4, Direction = Direction.NorthEast }
            }},
            new (){ Number = 4, Stage = 1, Name = "Encourage", Components = new(){
                new SpecialActionComponent { Parameter = 1 },
                new MoveActionComponent { Spaces = 3, Direction = Direction.NorthWest },
                new CustomActionComponent { ExtraEffect = "Each rat gains one extra might card (same as its highest might cards) until the end of the round"}
            }},
            new (){ Number = 5, Stage = 1, Name = "Tail Whip", Components = new(){
                new SpecialActionComponent { },
                new MoveActionComponent{ Spaces = 4 },
                new CustomActionComponent { ExtraEffect = "Turn to face directly away from her target"},
                new AttackActionComponent{ BossPart = BossPart.Rear, ExtraEffect = "Cone, Range 3, Knockback 2, this cone attack comes from her rear hex" }
            }},
            new (){ Number = 6, Stage = 2, Name = "Maternal Rage", Components = new(){
                new SpecialActionComponent { },
                new MoveActionComponent{ Spaces = 7 },
                new AttackActionComponent{ BossPart = BossPart.Front },
                new CustomActionComponent { ExtraEffect = "Change targets to the next closest enemy"},
                new MoveActionComponent{ Spaces = 4 },
                new AttackActionComponent{ BossPart = BossPart.Front },
            }},
            new (){ Number = 7, Stage = 2, Name = "Encourage", Components = new(){
                new SpecialActionComponent { Parameter = 1 },
                new MoveActionComponent { Spaces = 3, Direction = Direction.NorthEast },
                new CustomActionComponent { ExtraEffect = "Each rat gains one extra might card (same as its highest might cards) until the end of the round"}
            }},
            new (){ Number = 8, Stage = 2, Name = "Burst", Components = new(){
                new SpecialActionComponent { },
                new MoveActionComponent{ Spaces = 3, Direction = Direction.NorthWest },
                new CustomActionComponent { ExtraEffect = "Burst the rat closest to an primal. As it dies, it creates a hex 1 toxic cloud template on its hex"},
                new AttackActionComponent{ BossPart = BossPart.Front, ExtraEffect = "Targets all enemies in the template" }
            }},
            new (){ Number = 9, Stage = 2, Name = "Burst", Components = new(){
                new SpecialActionComponent { },
                new MoveActionComponent{ Spaces = 3, Direction = Direction.NorthEast },
                new CustomActionComponent { ExtraEffect = "Burst the rat closest to an primal. As it dies, it creates a hex 1 toxic cloud template on its hex"},
                new AttackActionComponent{ BossPart = BossPart.Front, ExtraEffect = "Targets all enemies in the template" }
            }},
            new (){ Number = 10, Stage = 2, Name = "Mother's Meal", Components = new(){
                new SpecialActionComponent { },
                new MoveActionComponent{ Spaces = 7, ExtraEffect = "Target farthest rat, consume it, regain 3 health to most damaged location, not restoring broken body parts" },
                new MoveActionComponent{ Spaces = 4 },
                new AttackActionComponent{ BossPart = BossPart.Front },
            }},
            new (){ Number = 11, Stage = 3, Name = "Tail Whip", Components = new(){
                new SpecialActionComponent { },
                new MoveActionComponent{ Spaces = 4 },
                new CustomActionComponent { ExtraEffect = "Turn to face directly away from her target"},
                new AttackActionComponent{ BossPart = BossPart.Rear, ExtraEffect = "Cone, Range 3, Knockback 2, this cone attack comes from her rear hex" }
            }},
            new (){ Number = 12, Stage = 3, Name = "Screech", Components = new(){
                new SpecialActionComponent { },
                new MoveActionComponent{ Spaces = 3, Direction = Direction.NorthEast },
                new CustomActionComponent { ExtraEffect = "This damages all Primal and Allies on the board. For each target, draw might.white for each hex between them and the next closest target to them. Draw seperately for each target. For each card showing a 2, the target must either lose 2 combat tokens (if they have at least 2) or lose one health (their choice)"},
            }},
            new (){ Number = 13, Stage = 3, Name = "Acid Spit", Components = new(){
                new SpecialActionComponent { },
                new MoveActionComponent{ Spaces = 3, Direction = Direction.SouthWest },
                new AttackActionComponent{ BossPart = BossPart.Front, Range = 6, ExtraEffect = "Place a 1 hex toxic cloud template centered on the target's hex, this attack also targets all enemies within the template." }
            }},
            new (){ Number = 14, Stage = 3, Name = "Acid Spit", Components = new(){
                new SpecialActionComponent { },
                new MoveActionComponent{ Spaces = 3, Direction = Direction.SouthEast },
                new AttackActionComponent{ BossPart = BossPart.Front, Range = 6, ExtraEffect = "Place a 1 hex toxic cloud template centered on the target's hex, this attack also targets all enemies within the template." }
            }},
            new (){ Number = 15, Stage = 3, Name = "Retreat", Components = new(){
                new SpecialActionComponent { },
                new CustomActionComponent { ExtraEffect = "The broodmother gains 1 defence for the rest of the encounter"},
                new AttackActionComponent{ BossPart = BossPart.Rear, ExtraEffect = "This targets all adjacent enemies. Draw once for all damage." },
                new MoveActionComponent{ Spaces = 4, ExtraEffect = "Move to the nearest board edge. She will stop at any board edge reached during this move" },
             }},
        };

        public static Boss GetBossEntity()
        {
            return new Boss()
            {
                Number = 1,
                Health = PART_MAPPINGS.Select(x => x.BossPart.ConvertToString()).ToDictionary(x => x, x => GlobalConstants.MAXIMUM_HEALTH),
                Defence = 2,
                XPosition = 0,
                YPosition = -2,
                Might = new Dictionary<Might, int>() { { Might.Red, 2 }, { Might.Yellow, 3 } },
                Direction = Direction.South
            };
        }

        public BroodMother(Boss bossEntity, IBossDependencies bossDependencies) : base(bossEntity, bossDependencies)
        {
        }

        protected override List<PartMapping> PartMappings => PART_MAPPINGS;
        protected override Size Size => Size.Large;
        public override string Name => "Brood Mother";

        protected override int GetStage()
        {
            var healthLost = BossEntity.Health.Values.Where(x => x <= 0).Count();
            return healthLost >= 4 ? 3 : healthLost >= 2 ? 2 : 1;
        }

        protected override List<BodyMapping> BodyMappings
        {
            get
            {
                // TODO: Boss rotation
                return BODY_MAPPINGS;
            }
        }

        protected override int GetDefaultTarget()
        {
            return GetNearestPlayer();
        }

        protected override Action GetAction(int number)
        {
            return ACTIONS[number];
        }

        protected override List<string> GetActionText(int number)
        {
            var action = ACTIONS.Find(x => x.Number == number) ?? throw new Exception("Invalid action number");
            var actionList = new List<string>() { $"{action.Name} (Stage {action.Stage})" };
            foreach (var component in action.Components)
            {
                switch (component)
                {
                    case MoveActionComponent moveComponent:
                        actionList.Add($"Move {moveComponent.Spaces} {(moveComponent.Direction is null ? "to target" : moveComponent.Direction)}");
                        break;
                    case AttackActionComponent attackComponent:
                        actionList.Add($"Attack {attackComponent.Range.ToEmptyIfZero("Range ", " ")}{attackComponent.Template.ToEmptyIfNull(null, " ")}{attackComponent.Size.ToEmptyIfZero(null, " ")} {attackComponent.BossPart.ToEmptyIfNull("with ", string.Empty)}");
                        break;
                    case SpecialActionComponent specialComponent:
                        actionList.Add($"Special Action");
                        break;
                    case CustomActionComponent customComponent:
                        actionList.Add($"Custom Action");
                        break;
                }
            }
            return actionList;
        }

        protected override void PerformSpecialAction(SpecialActionComponent actionComponent)
        {
            //do nothing for now
        }

        protected override void PerformCustomAction(CustomActionComponent number)
        {
            //do nothing for now
        }

        protected override void PerformStartOfRoundActions()
        {
            throw new NotImplementedException();
        }

        protected override void PerformEndOfRoundActions()
        {
            throw new NotImplementedException();
        }
    }
}