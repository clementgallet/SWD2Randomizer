using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace SWD2Randomizer
{
    class Patcher
    {
        private readonly string baseDir;

        public Patcher(string baseDir)
        {
            this.baseDir = baseDir;
        }

        public void PatchAll()
        {
            PatchIntro();
            PatchPriest();
            PatchHook();
            PatchVectron();
            PatchRosieAccess();
            PatchTriple();
            PatchOasis();
            PatchBlueprints();
            PatchRosie();
        }

        /* Make the ignition axe effective against Priest Glorious */
        public void PatchPriest()
        {
            string damageTypePatch = Path.Combine(baseDir, "Definitions", "damage_types.xml");

            XmlDocument doc = new XmlDocument();
            try
            {
                doc.Load(damageTypePatch);
            }
            catch (Exception)
            {
                Console.WriteLine("caught exception");
            }

            XmlNode firePriest = doc.SelectSingleNode("//DamageType[@Name='fire']/Multiplier[@AgainstArmor='priest_glorious']");

            if (firePriest != null)
                firePriest.Attributes["Value"].Value = "1";

            doc.Save(damageTypePatch);
        }

        /* Open gate to hook */
        public void PatchHook()
        {
            string hubPatch = Path.Combine(baseDir, "Patchsets", "TheHub", "the_hub_patch_main.le");

            XmlDocument doc = new XmlDocument();
            try
            {
                doc.Load(hubPatch);
            }
            catch (Exception)
            {
                Console.WriteLine("caught exception");
            }

            XmlNode door = doc.SelectSingleNode("//CustomEntity[Name='gate_hub_small']");

            if (door != null)
                door["Property"]["Value"].InnerText = "True";

            XmlNode target = doc.SelectSingleNode("//Connection[TargetId='32586635']");
            target.ParentNode.RemoveChild(target);

            doc.Save(hubPatch);
        }

        /* Close Vectron access, and give access to upgrade */
        public void PatchVectron()
        {
            string quests = Path.Combine(baseDir, "Definitions", "quests.xml");

            XmlDocument doc = new XmlDocument();
            try
            {
                doc.Load(quests);
            }
            catch (Exception)
            {
                Console.WriteLine("caught exception");
            }

            XmlNode questVectron = doc.SelectSingleNode("//Quest[@Name='quest_vectron_helper']");

            /* I'm lazy */
            questVectron.InnerXml = @"
                <AutoStart>true</AutoStart>
                <AutoComplete>true</AutoComplete>
                <Objective Name='obj_outro_conv' />";

            doc.Save(quests);

            string cavePatch = Path.Combine(baseDir, "Patchsets", "Archaea", "archaea_cave_vectron_entrance.le");

            try
            {
                doc.Load(cavePatch);
            }
            catch (Exception)
            {
                Console.WriteLine("caught exception");
            }

            XmlNode tiles = doc.SelectSingleNode("//TileLayer[Id='32565491']");

            if (tiles != null)
            {
                /* Add the vertical snake tile in the list */
                XmlElement snaketile = doc.CreateElement("Mapping");
                snaketile.SetAttribute("Name", "snake_vertical_01");
                snaketile.SetAttribute("FlipX", "False");
                snaketile.SetAttribute("FlipY", "False");
                snaketile.SetAttribute("Rotation", "False");
                tiles["TileMappings"].AppendChild(snaketile);

                tiles["Tiles"].ChildNodes[33].InnerText = "1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 0 0 0 0 0 0 0 0 0 0 0 1 1 1 0 0 1 0 0 0 0 0 0 1 17 2 1 1 1 1 1 1 1 1 1 1 1 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 1 1 1 1 1 1";
            }

            doc.Save(cavePatch);
        }

        /* Open door to Rosie after all generators */
        public void PatchRosieAccess()
        {
            string hubPatch = Path.Combine(baseDir, "Patchsets", "TheHub", "the_hub_patch_main.le");

            XmlDocument doc = new XmlDocument();
            try
            {
                doc.Load(hubPatch);
            }
            catch (Exception)
            {
                Console.WriteLine("caught exception");
            }

            XmlNode door = doc.SelectSingleNode("//Property[Value='quest_destroy_the_last_generators']");

            if (door != null)
                door["Value"].InnerText = "quest_destroy_all_generators";

            doc.Save(hubPatch);

            /* Remove Oasis blockade */
            string quests = Path.Combine(baseDir, "Definitions", "quests.xml");

            try
            {
                doc.Load(quests);
            }
            catch (Exception)
            {
                Console.WriteLine("caught exception");
            }

            XmlNode questOasis = doc.SelectSingleNode("//Quest[@Name='quest_hub_teleport_blocked']");
            questOasis.ParentNode.RemoveChild(questOasis);

            questOasis = doc.SelectSingleNode("//Quest[@Name='quest_see_blockade']");
            questOasis.ParentNode.RemoveChild(questOasis);

            questOasis = doc.SelectSingleNode("//Quest[@Name='quest_hubtube']");
            questOasis.ParentNode.RemoveChild(questOasis);

            questOasis = doc.SelectSingleNode("//Quest[@Name='quest_confront_rosie']");
            questOasis["UnlockedBy"].InnerText = "quest_destroy_all_generators";

            doc.Save(quests);
        }

        /* Open Oasis front gate */
        public void PatchOasis()
        {
            string levels = Path.Combine(baseDir, "Definitions", "levels.xml");

            XmlDocument doc = new XmlDocument();
            try
            {
                doc.Load(levels);
            }
            catch (Exception)
            {
                Console.WriteLine("caught exception");
            }

            XmlNode hubIntro = doc.SelectSingleNode("//Level[@Name='the_hub']/LayerFilters/Filter[@Layer='filter_intro']");
            hubIntro.InnerXml = @"<Disable />";

            XmlNode hubPostIntro = doc.SelectSingleNode("//Level[@Name='the_hub']/LayerFilters/Filter[@Layer='filter_post_intro']");
            hubPostIntro.InnerXml = @"
				<Enable/>
                <Disable Quest = ""quest_destroy_all_generators"" Status = ""completed"" />";

            doc.Save(levels);
        }

        /* Lower the price of triple grenade blueprint */
        public void PatchTriple()
        {
            string priceList = Path.Combine(baseDir, "Definitions", "pricelists.xml");

            XmlDocument doc = new XmlDocument();
            try
            {
                doc.Load(priceList);
            }
            catch (Exception)
            {
                Console.WriteLine("caught exception");
            }

            XmlNode price = doc.SelectSingleNode("//PriceList[@Name='blueprint_vendor_02']");

            if (price != null)
                price["Prices"].InnerText = "200";

            doc.Save(priceList);
        }

        /* Remove intro cutscene */
        public void PatchIntro()
        {
            string quests = Path.Combine(baseDir, "Definitions", "quests.xml");

            XmlDocument doc = new XmlDocument();
            try
            {
                doc.Load(quests);
            }
            catch (Exception)
            {
                Console.WriteLine("caught exception");
            }

            XmlNode questEarthquake = doc.SelectSingleNode("//Quest[@Name='quest_earthquake']");
            questEarthquake.Attributes["Template"].Value = "SIDE_QUEST";

            XmlNode questTemple = doc.SelectSingleNode("//Quest[@Name='quest_enter_temple']");
            questTemple.RemoveChild(questTemple["UnlockedBy"]);

            XmlNode questFen = doc.SelectSingleNode("//Quest[@Name='quest_find_fen']");
            questFen.Attributes["Template"].Value = "SIDE_QUEST";
            questFen.RemoveChild(questFen["UnlockedBy"]);

            XmlNode questRelFen = doc.SelectSingleNode("//Quest[@Name='quest_release_fen']");
            questRelFen.Attributes["Template"].Value = "SIDE_QUEST";

            XmlNode questExitTemple = doc.SelectSingleNode("//Quest[@Name='quest_exit_temple']");
            questExitTemple["UnlockedBy"].InnerText = "quest_get_run_boots";

            doc.Save(quests);

            string introPatch = Path.Combine(baseDir, "Patchsets", "WestDesert", "west_desert_intro.le");
            try
            {
                doc.Load(introPatch);
            }
            catch (Exception)
            {
                Console.WriteLine("caught exception");
            }

            XmlNode checkQuestProp = doc.SelectSingleNode("//ScriptEntity[Id='32564402']");

            checkQuestProp.InnerXml = @"
			<Id>32564402</Id>
			<Name>SetUpgrade2</Name>
			<Position>1664, 2035</Position>
			<Definition>SetUpgrade</Definition>
			<Area>1562, 2003, 203, 63</Area>
			<Connections>
				<Connection>
					<SourceContact>out</SourceContact>
					<TargetContact>in</TargetContact>
					<TargetId>32564403</TargetId>
				</Connection>
			</Connections>
            <Property>
                <Name>UpgradeId</Name>
                <Type>String</Type>
                <Value>minimap</Value>
            </Property>
            <Property>
                <Name>Tier</Name>
                <Type>Int32</Type>
                <Value>1</Value>
            </Property>
            <Property>
                <Name>HideNewMarker</Name>
                <Type>Boolean</Type>
                <Value>True</Value>
            </Property>
            ";

            checkQuestProp = doc.SelectSingleNode("//ScriptEntity[Id='32564403']");

            checkQuestProp.InnerXml = @"
			<Id>32564403</Id>
			<Name>ProgressQuest4</Name>
			<Position>1839, 2035</Position>
			<Definition>ProgressQuest</Definition>
			<Area>1807, 2003, 63, 63</Area>
			<Connections>
				<Connection>
					<SourceContact>out</SourceContact>
					<TargetContact>in</TargetContact>
					<TargetId>32564404</TargetId>
				</Connection>
			</Connections>
			<Property>
				<Name>QuestName</Name>
				<Type>String</Type>
				<Value>quest_lit_the_lamp</Value>
			</Property>
			<Property>
				<Name>ObjectiveName</Name>
				<Type>String</Type>
				<Value>obj_lit_the_lamp</Value>
			</Property>
			<Property>
				<Name>Increase</Name>
				<Type>Int32</Type>
				<Value>1</Value>
			</Property>
            ";

            checkQuestProp = doc.SelectSingleNode("//ScriptEntity[Id='32564404']");

            checkQuestProp.InnerXml = @"
			<Id>32564404</Id>
			<Name>CompleteQuest</Name>
			<Position>1988, 2035</Position>
			<Definition>CompleteQuest</Definition>
			<Area>1921, 2003, 132, 63</Area>
			<Connections>
				<Connection>
					<SourceContact>out</SourceContact>
					<TargetContact>in</TargetContact>
					<TargetId>32581848</TargetId>
				</Connection>
			</Connections>
			<Property>
				<Name>QuestName</Name>
				<Type>String</Type>
				<Value>quest_arrowtrap_defeated</Value>
			</Property>
            ";

            checkQuestProp = doc.SelectSingleNode("//ScriptEntity[Id='32581848']");

            checkQuestProp.InnerXml = @"
			<Id>32581848</Id>
			<Name>ProgressQuest5</Name>
			<Position>2168, 2036</Position>
			<Definition>ProgressQuest</Definition>
			<Area>2101, 2004, 132, 63</Area>
			<Connections>
				<Connection>
					<SourceContact>out</SourceContact>
					<TargetContact>in</TargetContact>
					<TargetId>32566045</TargetId>
				</Connection>
			</Connections>
			<Property>
				<Name>QuestName</Name>
				<Type>String</Type>
				<Value>quest_find_the_hub</Value>
			</Property>
			<Property>
				<Name>ObjectiveName</Name>
				<Type>String</Type>
				<Value>obj_find_the_hub</Value>
			</Property>
			<Property>
				<Name>Increase</Name>
				<Type>Int32</Type>
				<Value>1</Value>
			</Property>
            ";

            checkQuestProp = doc.SelectSingleNode("//ScriptEntity[Id='32566045']");

            checkQuestProp.InnerXml = @"
			<Id>32566045</Id>
			<Name>ProgressQuest6</Name>
			<Position>2389, 2036</Position>
			<Definition>ProgressQuest</Definition>
			<Area>2280, 2004, 216, 63</Area>
			<Connections/>
			<Property>
				<Name>QuestName</Name>
				<Type>String</Type>
				<Value>quest_enter_temple</Value>
			</Property>
			<Property>
				<Name>ObjectiveName</Name>
				<Type>String</Type>
				<Value>obj_enter_temple</Value>
			</Property>
			<Property>
				<Name>Increase</Name>
				<Type>Int32</Type>
				<Value>1</Value>
			</Property>
            ";

            doc.Save(introPatch);
        }

        /* Lower the price of triple grenade blueprint */
        public void PatchBlueprints()
        {
            string upgradeList = Path.Combine(baseDir, "Definitions", "upgrades.xml");

            XmlDocument doc = new XmlDocument();
            try
            {
                doc.Load(upgradeList);
            }
            catch (Exception)
            {
                Console.WriteLine("caught exception");
            }

            XmlNode blueprint = doc.SelectSingleNode("//Upgrade[@Name='fate.bloodquest']");

            XmlNode name = doc.CreateNode(XmlNodeType.Element, "CategoryStringId", null);
            name.InnerText = "upgrade_fate_bloodquest";
            blueprint.AppendChild(name);

            blueprint = doc.SelectSingleNode("//Upgrade[@Name='fate.xpx2']");

            name = doc.CreateNode(XmlNodeType.Element, "CategoryStringId", null);
            name.InnerText = "upgrade_fate_xpx2";
            blueprint.AppendChild(name);

            blueprint = doc.SelectSingleNode("//Upgrade[@Name='fate.explosions']");

            name = doc.CreateNode(XmlNodeType.Element, "CategoryStringId", null);
            name.InnerText = "upgrade_fate_explosions";
            blueprint.AppendChild(name);

            blueprint = doc.SelectSingleNode("//Upgrade[@Name='pressurebomb.launcher_triple']");

            name = doc.CreateNode(XmlNodeType.Element, "CategoryStringId", null);
            name.InnerText = "upgrade_pressurebomb_launcher_triple";
            blueprint.AppendChild(name);

            doc.Save(upgradeList);
        }

        /* Make Rosie fight trivial */
        public void PatchRosie()
        {
            string hubmain = Path.Combine(baseDir, "Patchsets", "TheHub", "the_hub_patch_main.le");

            XmlDocument doc = new XmlDocument();
            try
            {
                doc.Load(hubmain);
            }
            catch (Exception)
            {
                Console.WriteLine("caught exception");
            }

            XmlNode doorLevel = doc.SelectSingleNode("//ScriptEntity[Id='32595987']/Property[Name='DestinationLevel']");
            doorLevel["Value"].InnerText = "the_hub_cave_boss_saving_rusty";

            XmlNode doorEntity = doc.SelectSingleNode("//ScriptEntity[Id='32595987']/Property[Name='DestinationEntity']");
            doorEntity["Value"].InnerText = "";

            doc.Save(hubmain);


            string bosses = Path.Combine(baseDir, "Definitions", "entities.bosses.xml");

            try
            {
                doc.Load(bosses);
            }
            catch (Exception)
            {
                Console.WriteLine("caught exception");
            }

            XmlNode rosie = doc.SelectSingleNode("//Entity[@Name='rosie_boss']");

            rosie["Health"]["Health"].InnerText = "1999";
            rosie["Health"]["HurtShakeAmount"].InnerText = "2";

            for (int i = rosie["BossRosieComponent"]["Phases"].ChildNodes.Count - 1; i >= 0; i--)
            {
                XmlNode rosiePhase = rosie["BossRosieComponent"]["Phases"].ChildNodes[i];
                if (rosiePhase.Name == "Phase")
                {
                    if (rosiePhase.Attributes["Index"].Value == "0")
                    {
                        continue;
                    }
                    else if (rosiePhase.Attributes["Index"].Value == "1")
                    {
                        rosiePhase.Attributes["DisabledSignalEnd"].Value = "disabled_end_late";
                        continue;
                    }
                    else if (rosiePhase.Attributes["Index"].Value == "6")
                    {
                        rosiePhase.Attributes["Index"].Value = "2";
                        continue;
                    }
                }

                rosie["BossRosieComponent"]["Phases"].RemoveChild(rosiePhase);
            }

            doc.Save(bosses);
        }
    }
}
