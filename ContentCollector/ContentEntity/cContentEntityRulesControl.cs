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

            string pattern = "ToLoad[\\s\\S]+\"([^\\r\\n\"]+)\"[^\\}]+\\}";
            Regex regex = new Regex(pattern);
            Match match = regex.Match(rulesControlText);

            while (match.Success)
            {
                build.AddContentEntity(typeof(cContentEntityTexture), @"data\scripts.lua\plugins\" + match.Groups[1].Value + ".lua", this);
                match = match.NextMatch();
            }
        }
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    }   // cContentEntityRulesControl
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
}   // сContentCollector
