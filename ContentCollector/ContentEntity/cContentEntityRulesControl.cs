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
    public class cContentEntityRulesControl : cContentEntitySimple
    {
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public cContentEntityRulesControl(string name, cContentEntitySimple parent) : base(name, parent) { }
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public override void Parse(cBuild build)
        {
            StreamReader reader = new StreamReader(new FileStream(build.GetManglePath(FileName), FileMode.Open));
            string rulesControlText = reader.ReadToEnd();
            reader.Close();

            //    ToLoad =
            //    {
            //        "aSetBelt"				-- совет пристегнуть ремень
            //        , "rMoveBelt"				-- начало движения с непристегнутым ремнем
            //        , "aSpeeding"				-- совет снизить скорость
            //        ...
            //    }

            Regex regex = new Regex("--[^\\r\\n]+[\\r\\n]");
            rulesControlText = regex.Replace(rulesControlText, "");

            regex = new Regex("ToLoad[^\\}]+\\}");
            Match match = regex.Match(rulesControlText);
            string toLoad = match.Value;

            regex = new Regex("\"([^\\r\\n\"]+)\"");
            match = regex.Match(toLoad);

            while (match.Success)
            {
                build.AddContentEntity(new cContentEntitySimple(@"data\scripts.lua\plugins\" + match.Groups[1].Value + ".lua", this));
                match = match.NextMatch();
            }
        }
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    }   // cContentEntityRulesControl
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
}   // сContentCollector
