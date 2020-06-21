using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace SWD2Randomizer
{
    class Randomizer
    {
        private static SeedRandom random;
        private readonly string baseDir;
        private readonly int seed;
        private List<string> haveFlags;
        private List<Location> locations;

        public Randomizer(string baseDir, int seed, List<Location> locations)
        {
            random = new SeedRandom(seed);
            this.locations = locations;
            this.seed = seed;
            this.baseDir = baseDir;
            haveFlags = new List<string>();
        }

        public int Randomize()
        {
            bool ret;

            ret = PermuteFlags(Location.RandomizeType.Upgrade);
            if (!ret) return -1;

            ret = PermuteFlags(Location.RandomizeType.Area);
            if (!ret) return -1;

            ret = CheckValid();
            if (!ret) return 0;

            /* Write into the game */
            string[] patchFiles = Directory.GetFiles(Path.Combine(baseDir, "Patchsets"), "*.le", SearchOption.AllDirectories);

            Dictionary<string, string> areaDoors = new Dictionary<string, string>();
            Dictionary<string, string> caveDoors = new Dictionary<string, string>();
            Dictionary<string, string> areaLevel = new Dictionary<string, string>();

            foreach (string patchFile in patchFiles)
            {
                XmlDocument doc = new XmlDocument();
                try
                {
                    doc.Load(patchFile);
                }
                catch (Exception)
                {
                    Console.WriteLine("caught exception");
                }

                XmlNodeList upgradeNodes = doc.SelectNodes("//CustomEntity[Name='upgrade_podium' or Name='upgrade_podium1'] | //ScriptEntity[Name='GiveBlueprint' or Name='GiveBlueprint1']");
                foreach (XmlNode upgradeNode in upgradeNodes)
                {
                    foreach (var location in locations)
                    {
                        if (location.Type == Location.RandomizeType.Upgrade)
                        {
                            if (location.Name == upgradeNode["Property"]["Value"].InnerText)
                            {
                                upgradeNode["Property"]["Value"].InnerText = location.Grant;
                                break;
                            }
                        }
                    }
                }

                XmlNodeList doorNodes = doc.SelectNodes("//CustomEntity[Definition='door']");
                foreach (XmlNode doorNode in doorNodes)
                {
                    string area_door = doorNode["Name"].InnerText;
                    XmlNode cave = doorNode.SelectSingleNode("Property[Name='DestinationLevel']");
                    XmlNode cave_door = doorNode.SelectSingleNode("Property[Name='DestinationEntity']");

                    foreach (var location in locations)
                    {
                        if (location.Type == Location.RandomizeType.Area)
                        {
                            if (location.Name == cave["Value"].InnerText)
                            {
                                areaDoors[location.Name] = area_door;
                                caveDoors[location.Name] = cave_door["Value"].InnerText;
                                break;
                            }
                        }
                    }
                }

                doc.Save(patchFile);
            }

            foreach (string patchFile in patchFiles)
            {
                XmlDocument doc = new XmlDocument();
                try
                {
                    doc.Load(patchFile);
                }
                catch (Exception)
                {
                    Console.WriteLine("caught exception");
                }

                XmlNodeList doorNodes = doc.SelectNodes("//CustomEntity[Definition='door']");
                foreach (XmlNode doorNode in doorNodes)
                {
                    string cave_door = doorNode["Name"].InnerText;
                    XmlNode area = doorNode.SelectSingleNode("Property[Name='DestinationLevel']");

                    foreach (var location in locations)
                    {
                        if (location.Type == Location.RandomizeType.Area)
                        {
                            if (cave_door == caveDoors[location.Name])
                            {
                                areaLevel[location.Name] = area["Value"].InnerText;
                                break;
                            }
                        }
                    }
                }
            }

            foreach (string patchFile in patchFiles)
            {
                XmlDocument doc = new XmlDocument();
                try
                {
                    doc.Load(patchFile);
                }
                catch (Exception)
                {
                    Console.WriteLine("caught exception");
                }

                XmlNodeList doorNodes = doc.SelectNodes("//CustomEntity[Definition='door']");
                foreach (XmlNode doorNode in doorNodes)
                {
                    string this_door = doorNode["Name"].InnerText;
                    XmlNode dest_level = doorNode.SelectSingleNode("Property[Name='DestinationLevel']");
                    XmlNode dest_door = doorNode.SelectSingleNode("Property[Name='DestinationEntity']");

                    foreach (var location in locations)
                    {
                        if (location.Type == Location.RandomizeType.Area)
                        {
                            if (this_door == areaDoors[location.Name])
                            {
                                dest_level["Value"].InnerText = location.Grant;
                                dest_door["Value"].InnerText = caveDoors[location.Grant];
                                break;
                            }
                            if (this_door == caveDoors[location.Grant])
                            {
                                dest_level["Value"].InnerText = areaLevel[location.Name];
                                dest_door["Value"].InnerText = areaDoors[location.Name];
                                break;
                            }
                        }
                    }
                }

                doc.Save(patchFile);
            }

            RandomizeResources();
            RandomizeCogUpgrades();

            return 1;
        }

        private bool CheckValid()
        {
            haveFlags.Clear();

            bool didAdd = false;

            do
            {
                didAdd = false;
                foreach (var location in locations)
                {
                    /* We don't care if we already have the item */
                    if (haveFlags.Contains(location.Grant))
                    {
                        continue;
                    }

                    /* Check that we have access to the location */
                    if (location.CanAccess(haveFlags))
                    {
                        /* Make a copy of our items and add the granted item, then check if we can escape */
                        List<string> itemsAdded = new List<string>(haveFlags);
                        itemsAdded.Add(location.Grant);
                        if (location.CanEscape(itemsAdded) || location.CanEscapeWithoutNew(haveFlags))
                        {
                            /* We can successfully grab the item and escape! Add the item to our list */
                            haveFlags.Add(location.Grant);
                            didAdd = true;
                        }
                    }
                }
            }
            while (didAdd && !haveFlags.Contains("ending"));

            return haveFlags.Contains("ending");
        }

        private static void Shuffle<T>(IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = random.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        private bool PermuteFlags(Location.RandomizeType type)
        {
            List<string> randomized_flags = new List<string>();

            foreach (var location in locations)
            {
                if (location.Type == type)
                {
                    randomized_flags.Add(location.Name);
                }
            }

            Shuffle(randomized_flags);
            int i = 0;
            foreach (var location in locations)
            {
                if (location.Type == type)
                {
                    if (i == randomized_flags.Count)
                    {
                        return false;
                    }
                    location.Grant = randomized_flags[i];
                    i++;
                }
            }

            if (i != randomized_flags.Count)
            {
                return false;
            }

            return true;
        }

        public void RandomizeResources()
        {

            /* Write into the game */
            string resourcesFile = Path.Combine(baseDir, "Definitions", "resource_table.xml");

            List<string> oreList = new List<string>();
            List<string> gemList = new List<string>();

            XmlDocument doc = new XmlDocument();
            try
            {
                doc.Load(resourcesFile);
            }
            catch (Exception)
            {
                Console.WriteLine("caught exception");
            }

            XmlNode oreNodes = doc.SelectSingleNode("//ResourceTable[@Name='default']/ResourceGroup[@Name='ore']");

            foreach (XmlNode ore in oreNodes.ChildNodes)
            {
                if (ore.Name == "Entry")
                    oreList.Add(ore.Attributes["Name"].Value);
            }

            Shuffle(oreList);

            int i = 0;
            foreach (XmlNode ore in oreNodes.ChildNodes)
            {
                if (ore.Name == "Entry")
                    ore.Attributes["Name"].Value = oreList[i++];
            }

            XmlNode gemNodes = doc.SelectSingleNode("//ResourceTable[@Name='default']/ResourceGroup[@Name='gem']");

            foreach (XmlNode gem in gemNodes.ChildNodes)
            {
                if (gem.Name == "Entry")
                    gemList.Add(gem.Attributes["Name"].Value);
            }

            Shuffle(gemList);

            i = 0;
            foreach (XmlNode gem in gemNodes.ChildNodes)
            {
                if (gem.Name == "Entry")
                    gem.Attributes["Name"].Value = gemList[i++];
            }

            doc.Save(resourcesFile);
        }

        public void RandomizeCogUpgrade(XmlDocument doc, string upgrade, string ignore_upgrade1, string ignore_upgrade2, string add_upgrade1, string add_upgrade2)
        {
            List<string> upgrades = new List<string>();

            XmlNode upgradeNode = doc.SelectSingleNode("//Upgrade[@Name='" + upgrade + "']");
            if (upgradeNode["LockedUpgrades"] != null)
            {
                for (int k = upgradeNode["LockedUpgrades"].ChildNodes.Count - 1; k >= 0; k--)
                {
                    XmlNode locked = upgradeNode["LockedUpgrades"].ChildNodes[k];
                    if (locked.Name != "LockedUpgrade")
                        continue;

                    /* Skip the ignore */
                    if ((ignore_upgrade1 != null) && (locked.Attributes["Id"].Value == ignore_upgrade1))
                        continue;

                    if ((ignore_upgrade2 != null) && (locked.Attributes["Id"].Value == ignore_upgrade2))
                        continue;

                    upgrades.Add(locked.Attributes["Id"].Value);
                    upgradeNode["LockedUpgrades"].RemoveChild(locked);
                }
            }

            foreach (XmlNode tier in upgradeNode.ChildNodes)
            {
                if (tier.Name == "Tier")
                {
                    /* Skip the first tier if we got an upgrade already */
                    if ((tier["MoneyCost"] == null) || (tier["MoneyCost"].InnerText == "0"))
                        if (upgrades.Count != 0)
                            continue;

                    if (tier["SubUpgrade"] != null)
                    {
                        upgrades.Add(tier["SubUpgrade"].Attributes["Id"].Value);
                        tier.RemoveChild(tier["SubUpgrade"]);
                    }
                    else
                    {
                        upgrades.Add(null);
                    }
                }
            }

            /* Add upgrades */
            if (add_upgrade1 != null)
            {
                for (int j = 0; j < upgrades.Count; j++)
                {
                    if (upgrades[j] == null)
                    {
                        upgrades[j] = add_upgrade1;
                        add_upgrade1 = null;
                        break;
                    }
                }
                if (add_upgrade1 != null)
                    upgrades.Add(add_upgrade1);
            }

            if (add_upgrade2 != null)
            {
                for (int j = 0; j < upgrades.Count; j++)
                {
                    if (upgrades[j] == null)
                    {
                        upgrades[j] = add_upgrade2;
                        add_upgrade1 = null;
                        break;
                    }
                }
                if (add_upgrade2 != null)
                    upgrades.Add(add_upgrade2);
            }

            Shuffle(upgrades);

            int i = 0;

            foreach (XmlNode tier in upgradeNode.ChildNodes)
            {
                if (tier.Name == "Tier")
                {
                    if (upgrades[i] != null)
                    {
                        XmlNode sub = doc.CreateNode(XmlNodeType.Element, "SubUpgrade", null);
                        XmlAttribute attr = doc.CreateAttribute("Id");
                        attr.Value = upgrades[i];
                        sub.Attributes.Append(attr);
                        tier.AppendChild(sub);
                    }
                    i++;
                }
            }
        }

        public void RandomizeCogUpgrades()
        {

            string upgradesFile = Path.Combine(baseDir, "Definitions", "upgrades.xml");

            XmlDocument doc = new XmlDocument();
            try
            {
                doc.Load(upgradesFile);
            }
            catch (Exception)
            {
                Console.WriteLine("caught exception");
            }

            RandomizeCogUpgrade(doc, "pickaxe", "pickaxe.fire", null, null, null);
            RandomizeCogUpgrade(doc, "backpack", null, null, null, null);
            RandomizeCogUpgrade(doc, "lamp", null, null, "lamp.damaging_light", null);
            RandomizeCogUpgrade(doc, "armor", "armor.damage_reduction", null, null, null);
            RandomizeCogUpgrade(doc, "watertank", null, null, "watertank.water_pickup", null);
            RandomizeCogUpgrade(doc, "pressurebomb", "pressurebomb.launcher", "pressurebomb.launcher_triple", "pressurebomb.increased_radius", "pressurebomb.improved_water");
            RandomizeCogUpgrade(doc, "jackhammer", null, null, "jackhammer.shockwave", "jackhammer.improved_water");
            RandomizeCogUpgrade(doc, "steampack", "steampack.slayer", null, null, null);

            doc.Save(upgradesFile);
        }
    }
}