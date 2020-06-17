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
            PatchTriple();
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
            catch (Exception e)
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
            catch (Exception e)
            {
                Console.WriteLine("caught exception");
            }

            XmlNode door = doc.SelectSingleNode("//CustomEntity[Name='gate_hub_small']");

            if (door != null)
                door["Property"]["Value"].InnerText = "True";

            doc.Save(hubPatch);
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
            catch (Exception e)
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
            catch (Exception e)
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
            catch (Exception e)
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
			<Connections />
			<Property>
				<Name>QuestName</Name>
				<Type>String</Type>
				<Value>quest_arrowtrap_defeated</Value>
			</Property>
            ";


            doc.Save(introPatch);
        }


    }
}
