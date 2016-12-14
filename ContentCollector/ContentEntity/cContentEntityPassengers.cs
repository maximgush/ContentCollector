using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace ContentCollector
{
    public class cContentEntityPassengers : cContentEntitySimple
    {
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public cContentEntityPassengers(string name, cContentEntitySimple parent) : base(name, parent) { }
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        override public string FileName { get { return null; } }
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public override void Parse(cBuild build)
        {
            string fileName = build.GetManglePath(Name);

            StreamReader reader = new StreamReader(new FileStream(fileName, FileMode.Open));
            string text = reader.ReadToEnd();
            reader.Close();

            {
                // passenger = (model="characters/passangers/man_passanger_03", probability=0.5)
                // driver = (model="characters/passangers/man_driver_03", probability=0.5)
                {
                    string pattern = "model=\"([^\\r\\n]+)\"";
                    Regex regex = new Regex(pattern);
                    Match match = regex.Match(text);

                    while (match.Success)
                    {
                        build.AddContentEntity(new cContentEntityN2(@"export\gfxlib\" + match.Groups[1].Value + ".n2", this));
                        match = match.NextMatch();
                    }
                }               

                // uniqueModel = "characters/passangers/man_passanger_01"
                {
                    string pattern = "uniqueModel\\s=\\s\"([^\\r\\n]+)\"";
                    Regex regex = new Regex(pattern);
                    Match match = regex.Match(text);

                    while (match.Success)
                    {
                        build.AddContentEntity(new cContentEntityN2(@"export\gfxlib\" + match.Groups[1].Value + ".n2", this));
                        match = match.NextMatch();
                    }

                }
            }
        }
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    }
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
}   // сContentCollector
