using System.Collections.Generic;

namespace SWD2Randomizer
{
    public class LocationsSpeedrunner
    {
        public List<Location> Locations { get; set; }
        public LocationsSpeedrunner()
        {
            Locations = new List<Location>
            {
                new Location
                {
                    Name = "break_rocks",
                    Grant = "break_rocks",
                    CanAccess = have => have.Contains("jackhammer")
                },
                new Location
                {
                    Name = "break_one_rock",
                    Grant = "break_one_rock",
                    CanAccess = have => have.Contains("jackhammer") || have.Contains("fate.explosions")
                },
                new Location
                {
                    Name = "fate.explosions",
                    Grant = "fate.explosions",
                    Type = Location.RandomizeType.Upgrade
                },
                new Location
                {
                    Name = "top_yarrow",
                    Grant = "top_yarrow",
                },
                new Location
                {
                    Name = "fate.xpx2",
                    Grant = "fate.xpx2",
                    Type = Location.RandomizeType.Upgrade,
                    CanAccess = have => have.Contains("top_yarrow")
                },
                new Location
                {
                    Name = "main_yarrow",
                    Grant = "main_yarrow",
                    CanAccess = have => have.Contains("top_yarrow")
                },
                new Location
                {
                    Name = "yarrow_barrier_1",
                    Grant = "yarrow_barrier_1",
                    CanAccess = have => have.Contains("main_yarrow") && have.Contains("pressurebomb")
                },
                new Location
                {
                    Name = "yarrow_barrier_2",
                    Grant = "yarrow_barrier_2",
                    CanAccess = have => have.Contains("yarrow_barrier_1") && (have.Contains("pressurebomb") || (have.Contains("steampack") && have.Contains("steampack.slayer")))
                },
                new Location
                {
                    Name = "archaea_passage",
                    Grant = "archaea_passage",
                    CanAccess = have => have.Contains("pressurebomb")
                },
                new Location
                {
                    Name = "archaea_barrier",
                    Grant = "archaea_barrier",
                    CanAccess = have => have.Contains("top_yarrow") || have.Contains("archaea_passage")
                },
                new Location
                {
                    Name = "fate.bloodquest",
                    Grant = "fate.bloodquest",
                    Type = Location.RandomizeType.Upgrade,
                    CanAccess = have => have.Contains("archaea_barrier") && have.Contains("break_one_rock")
                },
                new Location
                {
                    Name = "oasis",
                    Grant = "oasis",
                    CanAccess = have => (have.Contains("archaea_barrier") && have.Contains("break_rocks")) || (have.Contains("main_yarrow") && ((have.Contains("steampack") && have.Contains("steampack.slayer")) || have.Contains("pressurebomb"))) || (have.Contains("middle_totd") && have.Contains("pickaxe.fire"))
                },
                new Location
                {
                    Name = "bottom_archaea",
                    Grant = "bottom_archaea",
                    CanAccess = have => have.Contains("oasis") && have.Contains("break_rocks")
                },
                new Location
                {
                    Name = "top_totd",
                    Grant = "top_totd",
                    CanAccess = have => have.Contains("jackhammer") && (have.Contains("hook") || have.Contains("steampack"))
                },
                new Location
                {
                    Name = "middle_totd",
                    Grant = "middle_totd",
                    CanAccess = have => (have.Contains("top_totd") && have.Contains("jackhammer")) || (have.Contains("archaea_barrier") && have.Contains("break_rocks")) || (have.Contains("oasis") && have.Contains("pressurebomb"))
                },
                new Location
                {
                    Name = "lava_pit",
                    Grant = "lava_pit",
                    CanAccess = have => have.Contains("middle_totd") && (have.Contains("hook") || have.Contains("steampack"))
                },
                new Location
                {
                    Name = "first_generator",
                    Grant = "first_generator",
                    CanAccess = have => have.Contains("firetemple_cave_generator")
                },
                new Location
                {
                    Name = "tog_generator",
                    Grant = "tog_generator",
                    CanAccess = have => have.Contains("temple_of_guidance_2_cave_generator")
                },
                new Location
                {
                    Name = "yarrow_generator",
                    Grant = "yarrow_generator",
                    CanAccess = have => have.Contains("yarrow_cave_generator")
                },
                new Location
                {
                    Name = "totd_generator",
                    Grant = "totd_generator",
                    CanAccess = have => have.Contains("firetemple_cave_generator2")
                },
                new Location
                {
                    Name = "ending",
                    Grant = "ending",
                    CanAccess = have => have.Contains("tog_generator") && have.Contains("yarrow_generator") && have.Contains("totd_generator") && have.Contains("oasis")
                },

                new Location
                {
                    Name = "pressurebomb",
                    Grant = "pressurebomb",
                    Type = Location.RandomizeType.Upgrade,
                    CanAccess = have => have.Contains("archaea_cave_pressurebomb"),
                    CanEscape = have => have.Contains("pressurebomb") || (have.Contains("steampack") && have.Contains("steampack.slayer")) || have.Contains("fate.explosions")
                },
                new Location
                {
                    Name = "pressurebomb.launcher",
                    Grant = "pressurebomb.launcher",
                    Type = Location.RandomizeType.Upgrade,
                    CanAccess = have => have.Contains("temple_of_guidance_2_cave_maze"),
                    CanEscape = have => (have.Contains("pressurebomb") && have.Contains("pressurebomb.launcher")) || (have.Contains("steampack") && have.Contains("steampack.slayer")),
                    CanEscapeWithoutNew = have => have.Contains("pressurebomb")
                },
                new Location
                {
                    Name = "pressurebomb.launcher_triple",
                    Grant = "pressurebomb.launcher_triple",
                    Type = Location.RandomizeType.Upgrade,
                    CanAccess = have => have.Contains("pressurebomb.launcher")
                },
                new Location
                {
                    Name = "jackhammer",
                    Grant = "jackhammer",
                    Type = Location.RandomizeType.Upgrade,
                    CanAccess = have => have.Contains("archaea_cave_jackhammer") && (have.Contains("pressurebomb") || have.Contains("jackhammer") || have.Contains("hook")),
                    CanEscape = have => have.Contains("jackhammer") || (have.Contains("steampack") && have.Contains("steampack.slayer")),
                    CanEscapeWithoutNew = have => have.Contains("pressurebomb")
                },
                new Location
                {
                    Name = "hook",
                    Grant = "hook",
                    Type = Location.RandomizeType.Upgrade,
                    CanAccess = have => have.Contains("the_hub_cave_grapplinghook"),
                    CanEscape = have => have.Contains("hook") || (have.Contains("pressurebomb") && have.Contains("pressurebomb.launcher")) || have.Contains("steampack"),
                    CanEscapeWithoutNew = have => have.Contains("pressurebomb")
                },
                new Location
                {
                    Name = "steampack.slayer",
                    Grant = "steampack.slayer",
                    Type = Location.RandomizeType.Upgrade,
                    CanAccess = have => have.Contains("yarrow_barrier_1") && (have.Contains("steampack") || (have.Contains("pressurebomb") && have.Contains("pressurebomb.launcher")) || have.Contains("hook")),
                    CanEscape = have => have.Contains("steampack") && have.Contains("steampack.slayer"),
                    CanEscapeWithoutNew = have => have.Contains("pressurebomb")
                },
                new Location
                {
                    Name = "pickaxe.fire",
                    Grant = "pickaxe.fire",
                    Type = Location.RandomizeType.Upgrade,
                    CanAccess = have => have.Contains("firetemple_cave_flamer"),
                    CanEscape = have => have.Contains("pickaxe.fire")
                },
                new Location
                {
                    Name = "armor.damage_reduction",
                    Grant = "armor.damage_reduction",
                    Type = Location.RandomizeType.Upgrade,
                    CanAccess = have => have.Contains("firetemple_cave_armor")
                },
                new Location
                {
                    Name = "hook.long_hook",
                    Grant = "hook.long_hook",
                    Type = Location.RandomizeType.Upgrade,
                    CanAccess = have => have.Contains("firetemple_cave_treasure_chamber")
                },
                new Location
                {
                    Name = "steampack",
                    Grant = "steampack",
                    Type = Location.RandomizeType.Upgrade,
                    CanAccess = have => have.Contains("archaea_cave_vectron_entrance") && have.Contains("first_generator"),
                    CanEscape = have => have.Contains("steampack") || have.Contains("hook")
                },
                new Location
                {
                    Name = "archaea_cave_pressurebomb",
                    Grant = "archaea_cave_pressurebomb",
                    Type = Location.RandomizeType.Area,
                },
                new Location
                {
                    Name = "archaea_cave_unclimbable",
                    Grant = "archaea_cave_unclimbable",
                    Type = Location.RandomizeType.Area,
                },
                new Location
                {
                    Name = "archaea_cave_snakestone_blocks",
                    Grant = "archaea_cave_snakestone_blocks",
                    Type = Location.RandomizeType.Area,
                    CanAccess = have => have.Contains("archaea_passage")
                },
                new Location
                {
                    Name = "archaea_cave_fallblock_puzzle",
                    Grant = "archaea_cave_fallblock_puzzle",
                    Type = Location.RandomizeType.Area,
                    CanAccess = have => have.Contains("archaea_passage")
                },
                new Location
                {
                    Name = "archaea_cave_cactus",
                    Grant = "archaea_cave_cactus",
                    Type = Location.RandomizeType.Area,
                    CanAccess = have => have.Contains("archaea_passage")
                },
                new Location
                {
                    Name = "archaea_cave_trilobite_puzzle",
                    Grant = "archaea_cave_trilobite_puzzle",
                    Type = Location.RandomizeType.Area,
                    CanAccess = have => have.Contains("archaea_barrier") && have.Contains("break_rocks")
                },
                new Location
                {
                    Name = "archaea_cave_jackhammer",
                    Grant = "archaea_cave_jackhammer",
                    Type = Location.RandomizeType.Area,
                    CanAccess = have => have.Contains("archaea_barrier")
                },
                new Location
                {
                    Name = "archaea_cave_snakestone",
                    Grant = "archaea_cave_snakestone",
                    Type = Location.RandomizeType.Area,
                    CanAccess = have => have.Contains("archaea_barrier") && have.Contains("break_rocks")
                },
                new Location
                {
                    Name = "archaea_cave_vectron_entrance",
                    Grant = "archaea_cave_vectron_entrance",
                    Type = Location.RandomizeType.Area,
                    CanAccess = have => have.Contains("bottom_archaea")
                },
                new Location
                {
                    Name = "west_desert_cave_cart_puzzle",
                    Grant = "west_desert_cave_cart_puzzle",
                    Type = Location.RandomizeType.Area,
                },
                new Location
                {
                    Name = "east_desert_cave_runfallblock",
                    Grant = "east_desert_cave_runfallblock",
                    Type = Location.RandomizeType.Area,
                },
                new Location
                {
                    Name = "temple_of_guidance_2_cave_generator",
                    Grant = "temple_of_guidance_2_cave_generator",
                    Type = Location.RandomizeType.Area,
                    CanAccess = have => have.Contains("top_yarrow") && (have.Contains("hook") || have.Contains("steampack"))
                },
                new Location
                {
                    Name = "temple_of_guidance_2_cave_maze",
                    Grant = "temple_of_guidance_2_cave_maze",
                    Type = Location.RandomizeType.Area,
                    CanAccess = have => have.Contains("top_yarrow"),
                },
                new Location
                {
                    Name = "yarrow_cave_water",
                    Grant = "yarrow_cave_water",
                    Type = Location.RandomizeType.Area,
                    CanAccess = have => have.Contains("main_yarrow"),
                },
                new Location
                {
                    Name = "yarrow_cave_cliffhanger",
                    Grant = "yarrow_cave_cliffhanger",
                    Type = Location.RandomizeType.Area,
                    CanAccess = have => have.Contains("yarrow_barrier_1"),
                },
                new Location
                {
                    Name = "yarrow_cave_mushimushiroom",
                    Grant = "yarrow_cave_mushimushiroom",
                    Type = Location.RandomizeType.Area,
                    CanAccess = have => have.Contains("yarrow_barrier_1"),
                },
                new Location
                {
                    Name = "yarrow_cave_bats",
                    Grant = "yarrow_cave_bats",
                    Type = Location.RandomizeType.Area,
                    CanAccess = have => have.Contains("yarrow_barrier_1"),
                },
                new Location
                {
                    Name = "yarrow_cave_run",
                    Grant = "yarrow_cave_run",
                    Type = Location.RandomizeType.Area,
                    CanAccess = have => have.Contains("yarrow_barrier_1"),
                },
                new Location
                {
                    Name = "yarrow_cave_generator",
                    Grant = "yarrow_cave_generator",
                    Type = Location.RandomizeType.Area,
                    CanAccess = have => have.Contains("yarrow_barrier_2"),
                },
                new Location
                {
                    Name = "firetemple_cave_firebat",
                    Grant = "firetemple_cave_firebat",
                    Type = Location.RandomizeType.Area,
                    CanAccess = have => have.Contains("top_totd"),
                },
                new Location
                {
                    Name = "firetemple_cave_floor_is_lava",
                    Grant = "firetemple_cave_floor_is_lava",
                    Type = Location.RandomizeType.Area,
                    CanAccess = have => have.Contains("top_totd"),
                },
                new Location
                {
                    Name = "firetemple_cave_lava_shooters",
                    Grant = "firetemple_cave_lava_shooters",
                    Type = Location.RandomizeType.Area,
                    CanAccess = have => have.Contains("top_totd"),
                },
                new Location
                {
                    Name = "firetemple_cave_crusher",
                    Grant = "firetemple_cave_crusher",
                    Type = Location.RandomizeType.Area,
                    CanAccess = have => have.Contains("top_totd"),
                },
                new Location
                {
                    Name = "firetemple_cave_golem",
                    Grant = "firetemple_cave_golem",
                    Type = Location.RandomizeType.Area,
                    CanAccess = have => have.Contains("middle_totd"),
                },
                new Location
                {
                    Name = "firetemple_cave_spikeconveyor",
                    Grant = "firetemple_cave_spikeconveyor",
                    Type = Location.RandomizeType.Area,
                    CanAccess = have => have.Contains("middle_totd"),
                },
                new Location
                {
                    Name = "firetemple_cave_flamer",
                    Grant = "firetemple_cave_flamer",
                    Type = Location.RandomizeType.Area,
                    CanAccess = have => have.Contains("middle_totd"),
                },
                new Location
                {
                    Name = "firetemple_cave_generator",
                    Grant = "firetemple_cave_generator",
                    Type = Location.RandomizeType.Area,
                    CanAccess = have => have.Contains("middle_totd") && have.Contains("pickaxe.fire"),
                },
                new Location
                {
                    Name = "firetemple_cave_treasure_chamber",
                    Grant = "firetemple_cave_treasure_chamber",
                    Type = Location.RandomizeType.Area,
                    CanAccess = have => have.Contains("top_totd") && have.Contains("middle_totd") && have.Contains("pickaxe.fire")
                },
                new Location
                {
                    Name = "firetemple_cave_boxes",
                    Grant = "firetemple_cave_boxes",
                    Type = Location.RandomizeType.Area,
                    CanAccess = have => have.Contains("lava_pit")
                },
                new Location
                {
                    Name = "firetemple_cave_hell_carts",
                    Grant = "firetemple_cave_hell_carts",
                    Type = Location.RandomizeType.Area,
                    CanAccess = have => have.Contains("lava_pit")
                },
                new Location
                {
                    Name = "firetemple_cave_armor",
                    Grant = "firetemple_cave_armor",
                    Type = Location.RandomizeType.Area,
                    CanAccess = have => have.Contains("lava_pit")
                },
                new Location
                {
                    Name = "firetemple_cave_generator2",
                    Grant = "firetemple_cave_generator2",
                    Type = Location.RandomizeType.Area,
                    CanAccess = have => have.Contains("lava_pit")
                },
                new Location
                {
                    Name = "the_hub_cave_grapplinghook",
                    Grant = "the_hub_cave_grapplinghook",
                    Type = Location.RandomizeType.Area,
                    CanAccess = have => have.Contains("oasis")
                },
            };
        }
    }
}
