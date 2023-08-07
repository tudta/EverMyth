using Source.Abilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Source
{
    public static class CustomAbilityManager
    {
        private static Dictionary<int, CustomAbility> _customAbilities;

        public static Dictionary<int, CustomAbility> CustomAbilities
        {
            get
            {
                return _customAbilities;
            }

            set
            {
                _customAbilities = value;
            }
        }

        public static void Init()
        {
            _customAbilities = new Dictionary<int, CustomAbility>();
            _customAbilities.Add(Constants.ABILITY_BOP, new Bop());
            for (int i = 0; i < _customAbilities.Count; i++)
            {
                _customAbilities.ElementAt(i).Value.Init();
            }
        }
    }
}