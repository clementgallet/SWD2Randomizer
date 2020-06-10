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
/*            PatchElMachino();
*/        }

        /* Remove intro cutscene */
        public void PatchIntro()
        {
            string introPatch = Path.Combine(baseDir, "Patchsets", "WestDesert", "west_desert_intro.le");

            XmlDocument doc = new XmlDocument();
            try
            {
                doc.Load(introPatch);
            }
            catch (Exception e)
            {
                Console.WriteLine("caught exception");
            }

            XmlNode cutsceneScript = doc.SelectSingleNode("//ScriptEntity[Id='32564401']");

            if (cutsceneScript != null)
                cutsceneScript.ParentNode.RemoveChild(cutsceneScript);

            doc.Save(introPatch);
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

        public void PatchElMachino()
        {
            string introPatch = Path.Combine(baseDir, "Patchsets", "WestDesert", "west_desert_intro.le");

            XmlDocument doc = new XmlDocument();
            try
            {
                doc.Load(introPatch);
            }
            catch (Exception e)
            {
                Console.WriteLine("caught exception");
            }

/*            XmlNode elmachinoReload = doc.SelectSingleNode("//ScriptEntity[Definition='ProgressQuest']/Property[Name='QuestName' and Value='quest_tutorial_indicators']");
            XmlNode target = doc.SelectSingleNode("//Connection[TargetId='1042288']");

            target["TargetId"].InnerText = "32578602";*/


            XmlNode newScript = doc.CreateNode("element", "ScriptEntity", "");
            newScript.InnerXml = @"
				<Id>92578602</Id>
				<Name>ProgressQuest</Name>
              <Position>3291, 3111</Position>
              <Definition>ProgressQuest</Definition>
              <Area>3203, 3079, 174, 63</Area>
              <Connections />
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
				</Property>";

            XmlNode newEnter = doc.CreateNode("element", "ScriptEntity", "");
            newEnter.InnerXml = @"							<Id>32564401</Id>
							<Name>OnEnter9</Name>
              <Position>2391, 3348</Position>
							<Definition>OnEnter</Definition>
              <Area>2322, 3316, 97, 63</Area>
							<Connections>
								<Connection>
									<SourceContact>out</SourceContact>
									<TargetContact>in</TargetContact>
									<TargetId>92578602</TargetId>
								</Connection>
							</Connections>
							<Property>
								<Name>TriggerOnlyOnce</Name>
								<Type>Boolean</Type>
								<Value>True</Value>
							</Property>";

            XmlNode layer = doc.SelectSingleNode("//TileLayer[Id='1040543']");

            layer["Entities"].AppendChild(newScript);
            layer["Entities"].AppendChild(newEnter);

            doc.Save(introPatch);
        }


    }
}
