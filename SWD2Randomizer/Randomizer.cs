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
        public List<string> upgrades;

        public Randomizer(string baseDir, int seed, List<Location> locations)
        {
            random = new SeedRandom(seed);
            this.locations = locations;
            this.seed = seed;
            this.baseDir = baseDir;
            haveFlags = new List<string>();
            upgrades = new List<string>();

            GetUpgradeFlags();
//            upgrades = mem.GetUpgradeNames();
        }

        public void GetUpgradeFlags()
        {
            upgrades.Clear();

            string[] patchFiles = Directory.GetFiles(Path.Combine(baseDir, "Patchsets"), "*.le", SearchOption.AllDirectories);

            foreach (string patchFile in patchFiles)
            {
                //Console.WriteLine("Looking into file {0}", patchFile);
                XmlDocument doc = new XmlDocument();
                try
                {
                    doc.Load(patchFile);
                }
                catch(Exception e)
                {
                    Console.WriteLine("caught exception");
                }

                XmlNodeList upgradeNodes = doc.SelectNodes("//CustomEntity[Name='upgrade_podium' or Name='upgrade_podium1'] | //ScriptEntity[Name='GiveBlueprint' or Name='GiveBlueprint1']");
                foreach (XmlNode upgradeNode in upgradeNodes)
                {
                    string upgrade = upgradeNode["Property"]["Value"].InnerText;
                    upgrades.Add(upgrade);
                }
            }
        }

        public int Randomize()
        {
            bool ret;

            ret = PermuteFlags(upgrades, Location.RandomizeType.Upgrade);
            if (!ret) return -1;

            ret = CheckValid();
            if (!ret) return 0;

            /* Write into the game */
            string[] patchFiles = Directory.GetFiles(Path.Combine(baseDir, "Patchsets"), "*.le", SearchOption.AllDirectories);

            foreach (string patchFile in patchFiles)
            {
                XmlDocument doc = new XmlDocument();
                try
                {
                    doc.Load(patchFile);
                }
                catch (Exception e)
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

                doc.Save(patchFile);
            }

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

        private bool PermuteFlags(List<string> flags, Location.RandomizeType type)
        {
            List<string> randomized_flags = new List<string>(flags);
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

    }
}
