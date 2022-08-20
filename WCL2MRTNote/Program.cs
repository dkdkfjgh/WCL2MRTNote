using WCL2MRTNote;
using System;
using System.Collections.Generic;
using System.Text.Json;
/*변수 선언*/
string Url = @"https://api.lorrgs.io/api/spec_ranking/priest-shadow/hungering-destroyer?difficulty=mythic&metric=dps&limit=1";
string JsonStr = string.Empty;



/*Json 가져오기*/
Handler JHandler = new Handler();
JsonStr = JHandler.Request_Json(Url);

Root rootObj = JsonSerializer.Deserialize<Root>(JsonStr); //역직렬화 수행

Console.WriteLine(Handler.UnixTimeStampToDateTime((double)rootObj.updated) + " 업로드 WCL 로그");
Console.WriteLine("킬 타임 : " + Handler.ConvertMilliseconds(rootObj.fights[0].duration));

List<Cast> Casts = rootObj.fights[0].players[0].casts;




const int OneCycleMS = 15000;

int timestamp = Casts[0].ts + OneCycleMS;

Console.Write(string.Format("{{time:{0}}}", Handler.ConvertMilliseconds(Casts[0].ts)));
foreach(var C in Casts)
{
    if(C.ts < timestamp)
    {
        Console.Write(string.Format("{{spell:{0}}}",C.id) + " ");
    }
    else
    {
        Console.WriteLine();
        Console.Write(string.Format("{{time:{0}}}", Handler.ConvertMilliseconds(C.ts)));
        Console.Write(string.Format("{{spell:{0}}}", C.id) + " ");
        timestamp = C.ts + OneCycleMS;
    }
}

Console.ReadLine();