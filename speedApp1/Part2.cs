using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace speedApp1
{
    class Part2
    {
        private const string statsPath = @"d:\work\speed-coding-2021\input\1_pokemon_species_base_stats.json";
        private const string levelsPath = @"d:\work\speed-coding-2021\input\2_pokemon_level_cp_multipliers.json";
        private const string pokemonPath = @"d:\work\speed-coding-2021\input\3_rupert_pokemon.json";
        public static void Run()
        {
            var statsByName = JsonConvert.DeserializeObject<List<PokemonStats>>(
                File.ReadAllText(statsPath))
                .ToDictionary(ps => ps.Name + ps.Form, ps => ps);
            
            var levelsByLevelList = JsonConvert.DeserializeObject<List<LevelMulitplier>>(
                File.ReadAllText(levelsPath));
            var levelsByLevel = levelsByLevelList.ToDictionary(l => l.Level, l => l);
            
            var rupertsPokemon = JsonConvert.DeserializeObject<List<RupertsPokemon>>(
                File.ReadAllText(pokemonPath));

            var results = new List<string>();

            for (var targetCp = 3155; targetCp <= 3254; targetCp++)
            {
                var foundCp = false;
                foreach (var pokemon in rupertsPokemon)
                {
                    for (var level = pokemon.Level; level <= 51; level += 0.5)
                    {
                        if (pokemon.CpAtLevel(level, statsByName[pokemon.Name+pokemon.Form], levelsByLevel[level]) == targetCp)
                        {
                            results.Add($"{targetCp},{pokemon.Name},{pokemon.CpAtLevel(pokemon.Level, statsByName[pokemon.Name+pokemon.Form], levelsByLevel[pokemon.Level])},{level}");
                            foundCp = true;
                            break;
                        }
                    }

                    if (foundCp)
                    {
                        break;
                    }
                }

                if (!foundCp)
                {
                    results.Add($"{targetCp},Impossible,,");
                }
            }

            foreach (var result in results)
            {
                Console.WriteLine(result);
            }
        }
    }

    public class PokemonStats
    {
        [JsonProperty("number")]
        public int Number { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("form")]
        public string Form { get; set; }

        [JsonProperty("attack")]
        public int Attack { get; set; }

        [JsonProperty("defense")]
        public int Defense { get; set; }

        [JsonProperty("stamina")]
        public int Stamina { get; set; }
    }
    
    public class LevelMulitplier
    {
        [JsonProperty("level")]
        public double Level { get; set; }

        [JsonProperty("cp_multiplier")]
        public double CpMultiplier { get; set; }

        [JsonProperty("stardust")]
        public int Stardust { get; set; }

        [JsonProperty("candy")]
        public int Candy { get; set; }

        [JsonProperty("xl_candy")]
        public int XlCandy { get; set; }
    }
    
    public class RupertsPokemon
    {
        [JsonProperty("index")]
        public int Index { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("form")]
        public string Form { get; set; }

        [JsonProperty("cp")]
        public int Cp { get; set; }

        [JsonProperty("attack_iv")]
        public int AttackIv { get; set; }

        [JsonProperty("defense_iv")]
        public int DefenseIv { get; set; }

        [JsonProperty("stamina_iv")]
        public int StaminaIv { get; set; }

        [JsonProperty("level")]
        public double Level { get; set; }


        public int CpAtLevel(double level, PokemonStats pokemonStats, LevelMulitplier levelMulitplier)
        {
            var attack = pokemonStats.Attack + AttackIv;
            var defense = pokemonStats.Defense + DefenseIv;
            var stamina = pokemonStats.Stamina + StaminaIv;

            return (int)Math.Floor((attack * Math.Sqrt(defense) * Math.Sqrt(stamina) * levelMulitplier.CpMultiplier *
                               levelMulitplier.CpMultiplier) / 10);
        }
    }


}