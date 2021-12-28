using RegularX.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RegularX.Objs_auto
{
    // Пофиксить ссылки
    // Сделать проверку получения результата от сайта
    class Controller
    {
        public static List<Model> GetModels(string link = "https://www.ilcats.ru/toyota/?function=getModels&market=EU")
        {
            List<Model> models = new List<Model>();
            string line = "";
            using (WebClient wc = new WebClient())
            {
                line = wc.DownloadString(link);
            }

            //Match match = Regex.Match(line, "<div class=\"List Multilist\">(.*?)</div>");
            //Получение блока со списком авто
            Match match = Regex.Match(line, "<div class='List Multilist\'>(.*?)<div class='Advert Advert2'>");

            var multilist = match.Groups[1].Value;

            // Разделение на объекты моделей
            var m_l = Regex.Matches(multilist, "<div class='List'>(.*?)</div></div></div></div>")
                .Cast<Match>().Select(x => x.Groups[1].Value).ToList<string>();
            //m_l.RemoveRange(5, m_l.Count-6);
            //Разделение моделей на параметры
            foreach (var it in m_l)
            {
                // Нужно будет провести чистку ссылки от "защиты" (мусора)
                var obj = Regex.Match(it + "</div>", Resources.getParams);
                var g = $"name = { obj.Groups[1].Value} \n link = { obj.Groups[3].Value} \n model_code = {obj.Groups[4].Value}" +
                    $"\nperiod = { obj.Groups[6].Value} \n model = { obj.Groups[8].Value}";
                // objs.Groups[4].Value = "28B510" - onlyString
                var model = (new Model(obj.Groups[4].Value, obj.Groups[1].Value,
                    obj.Groups[3].Value, obj.Groups[6].Value.ToString(), obj.Groups[8].Value));
                //Console.WriteLine(g);
                models.Add(model);
                model.Print();
            }

            //Добавление объекта в БД????
            return models;
        }

        public static List<Complectation> GetComplectations(string link = "")
        {
            //todo: Convert Link in class
            //todo: Convert period in class
            List<Complectation> complectations = new List<Complectation>();
            string line = "";
            using (WebClient wc = new WebClient())
            {
                wc.Headers.Add("user-agent", "Mozilla/5.0 (platform; rv:geckoversion) Gecko/geckotrail Firefox/firefoxversion");
                //line = wc.DownloadString("https://www.ilcats.ru/toyota/?function=getComplectations&amp;market=EU&amp;model=671440&amp;startDate=198308&amp;endDate=198903");
                line = wc.DownloadString("https://www.ilcats.ru/toyota/?function=getComplectations&market=EU&model=671440&startDate=198308&endDate=198903");
                line = Program.MyDecoder(line);
            }
            // Разбиение на строки
            var cortages = Regex.Matches(line, "<tr>(.*?)</tr>")
                .Cast<Match>().Select(x => x.Groups[1].Value).ToList<string>();
            cortages.RemoveAt(0); cortages.RemoveRange(5, cortages.Count - 5);

            // Пожертвовал оптимизацией ради читабельности кода
            foreach(var row in cortages)
            {
                List<string> complect = new List<string>();
                var collumns = Regex.Matches(row, "<td>(.*?)</td>").Cast<Match>().Select(x => x.Groups[1].Value).ToList<string>();
                int len = collumns.Count;
                for (int i = 0; i < len - 1; i++)
                {
                    if (i==0)
                    {
                        // Получение id и ссылки
                        var id_lnk = Regex.Match(collumns[i], Resources.getComplIdLnk);
                        complect.Add(id_lnk.Groups[4].Value);
                        complect.Add(id_lnk.Groups[2].Value);
                    }
                    else
                    {
                        // Получение всего остального
                        var param = Regex.Match(collumns[i], Resources.getComplParam);
                        complect.Add(param.Groups[2].Value);
                    }
                }
                //Console.WriteLine();
                complectations.Add(new Complectation(complect[0],complect[1], complect[2],
                    complect[3], complect[4]));
            }
            //Console.WriteLine();
            return complectations;
        }

        public static void GetComplGroup(string link = "")
        {
            List<ComplectationGroups> compGroup = new List<ComplectationGroups>();
            string line = "";
            using (WebClient wc = new WebClient())
            {
                wc.Headers.Add("user-agent", "Mozilla/5.0 (platform; rv:geckoversion) Gecko/geckotrail Firefox/firefoxversion");
                line = wc.DownloadString("https://www.ilcats.ru/toyota/?function=getGroups&market=EU&model=671440&modification=LN51L-KRA&complectation=001");
                line = Program.MyDecoder(line);
            }
            // Выбираем текст для дальнейшей обработки
            var groups_str = Regex.Match(line, "<div class='List '>(.*?)</div></div></div><div").Groups[1].Value + "</div>" + "</div>";

            var groups = Regex.Matches(groups_str, Resources.getGroups);
            var a = groups[0].Groups[0].Value;

            foreach (Match group in groups)
            {
                compGroup.Add(new ComplectationGroups(group.Groups[4].Value, group.Groups[2].Value));
            }

            //var cortages = Regex.Matches(line, "<div id='Body' class='ifListBody'>(.*?)<div><div><div></div")
            //    .Cast<Match>().Select(x => x.Groups[1].Value).ToList<string>();
            //cortages.RemoveAt(0); cortages.RemoveRange(5, cortages.Count - 5);
        }



    }

}
