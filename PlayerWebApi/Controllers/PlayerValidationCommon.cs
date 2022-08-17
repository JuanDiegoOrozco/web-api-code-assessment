using PlayerWebApi.Data.Entities;

namespace PlayerWebApi.Controllers
{
    public static class PlayerValidationCommon
    {
        public static readonly List<string> PositionTypes = new List<string> { "defender", "midfielder", "forward" };
        public static readonly List<string> PlayerSkillsTypes = new List<string> { "defense", "attack", "speed", "strength", "stamina" };

        /// <summary>
        /// Returns the full list of possible position types in sentence format.
        /// </summary>
        /// <returns></returns>
        private static string GetListOfPositionTypes()
        {
            string positions = "";

            for (int i = 0; i < PositionTypes.Count; i++)
            {
                if (i == 0)
                {
                    positions += PositionTypes[i];
                }
                else
                {
                    positions += ", " + PositionTypes[i];
                }
            }
            return positions + ".";
        }

        /// <summary>
        /// Returns the full list of possible Player Skill types in sentence format.
        /// </summary>
        /// <returns></returns>
        private static string GetListOfSkillTypes()
        {
            string skillTypes = "";

            for (int i = 0; i < PlayerSkillsTypes.Count; i++)
            {
                if (i == 0)
                {
                    skillTypes += PlayerSkillsTypes[i];
                }
                else
                {
                    skillTypes += ", " + PlayerSkillsTypes[i];
                }
            }
            return skillTypes + ".";
        }

        /// <summary>
        /// Validates the Player's skill type
        /// </summary>
        /// <param name="skillType"></param>
        /// <exception cref="Exception"></exception>
        public static void ValidateSkillType(string skillType)
        {
            if (string.IsNullOrEmpty(skillType))
            {
                throw new Exception("Field \"" + nameof(PlayerSkill.Skill) + "\" cannot be empty.");
            }
            else
            {
                bool valid = false;
                foreach (var playerSkillType in PlayerSkillsTypes)
                {
                    if (playerSkillType == skillType)
                    {
                        valid = true;
                        break;
                    }
                }

                if (!valid)
                {
                    throw new Exception("Field \"" + nameof(PlayerSkill.Skill) + "\" must be one of the following: " + GetListOfSkillTypes());
                }
            }
        }

        /// <summary>
        /// Validates the Skill value
        /// </summary>
        /// <param name="value"></param>
        /// <exception cref="Exception"></exception>
        public static void ValidateSkillValue(int value)
        {
            if (value < 0 || value > 99)
            {
                throw new Exception("Field \"" + nameof(PlayerSkill.Value) + "\" must be between 1 and 99");
            }
        }

        /// <summary>
        /// Validates Player Name field
        /// </summary>
        /// <param name="name"></param>
        public static void ValidatePlayerName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new Exception("Field \"" + nameof(Player.Name) + "\" cannot be empty.");
            }
        }

        /// <summary>
        /// Validates Player Position field
        /// </summary>
        /// <param name="position"></param>
        public static void ValidatePlayerPosition(string position)
        {
            if (string.IsNullOrEmpty(position))
            {
                throw new Exception("Field \"" + nameof(Player.Position) + "\" cannot be empty.");
            }
            else
            {
                bool valid = false;
                foreach (var positionType in PositionTypes)
                {
                    if (positionType == position)
                    {
                        valid = true;
                        break;
                    }
                }

                if (!valid)
                {
                    throw new Exception("Field \"" + nameof(Player.Position) + "\" must be one of the following: " + GetListOfPositionTypes());
                }
            }
        }

        /// <summary>
        /// Validates single PlayerSkill
        /// </summary>
        /// <param name="playerSkill"></param>
        public static void ValidatePlayerSkill(PlayerSkill playerSkill)
        {
            ValidateSkillType(playerSkill.Skill);
            ValidateSkillValue(playerSkill.Value);
        }

        /// <summary>
        /// Validates list of PlayerSkill
        /// </summary>
        /// <param name="playerSkills"></param>
        public static void ValidatePlayerSkills(List<PlayerSkill> playerSkills)
        {
            foreach (var playerSkill in playerSkills)
            {
                ValidatePlayerSkill(playerSkill);
            }
        }
    }
}
