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
    // Переделать первую страницу для разбивки модели на части
    class Controller
    {
        public const string core_lnk = "https://www.ilcats.ru";
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
            //m_l.RemoveRange(5, m_l.Count-6);          ^
            //Разделение моделей на параметры           |
            foreach (var it in m_l)     //              |
            {//                                         |
                // Нужно будет провести чистку ссылки от "защиты" (мусора)
                var obj = Regex.Match(it + "</div></div></div>", Resources.getModName);
                string model_name = obj.Groups[1].Value;
                var modells = obj.Groups[2].Value + "</div></div>";

                var m = Regex.Matches(modells, "");

                var g = $"name = { obj.Groups[1].Value} \n link = { obj.Groups[3].Value} \n model_code = {obj.Groups[4].Value}" +
                    $"\nperiod = { obj.Groups[6].Value} \n model = { obj.Groups[8].Value}";
                // objs.Groups[4].Value = "28B510" - onlyString
                var model = (new Model(obj.Groups[4].Value, obj.Groups[1].Value,
                    obj.Groups[3].Value, obj.Groups[6].Value.ToString(), obj.Groups[8].Value));
                //Console.WriteLine(g);
                models.Add(model);
                //model.Print();
            }
            models.RemoveRange(5, models.Count - 5);
            //Добавление объекта в БД????


            return models;
        }

        public static List<Complectation> GetComplectations(string link = "https://www.ilcats.ru/toyota/?function=getComplectations&market=EU&model=671440&startDate=198308&endDate=198903")
        {
            //todo: Convert Link in class
            //todo: Convert period in class
            List<Complectation> complectations = new List<Complectation>();
            string line = "";
            using (WebClient wc = new WebClient())
            {
                wc.Headers.Add("user-agent", "Mozilla/5.0 (platform; rv:geckoversion) Gecko/geckotrail Firefox/firefoxversion");
                //line = wc.DownloadString("https://www.ilcats.ru/toyota/?function=getComplectations&amp;market=EU&amp;model=671440&amp;startDate=198308&amp;endDate=198903");
                line = wc.DownloadString(link);
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

        public static List<ComplectationGroups> GetComplGroup(string link = "https://www.ilcats.ru/toyota/?function=getGroups&market=EU&model=671440&modification=LN51L-KRA&complectation=001")
        {
            List<ComplectationGroups> compGroup = new List<ComplectationGroups>();
            string line = "";
            using (WebClient wc = new WebClient())
            {
                wc.Headers.Add("user-agent", "Mozilla/5.0 (platform; rv:geckoversion) Gecko/geckotrail Firefox/firefoxversion");
                line = wc.DownloadString(link);
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
            return compGroup;
        }

        public static List<SubGroup> GetSubGroupCompl(string link = "https://www.ilcats.ru/toyota/?function=getSubGroups&market=EU&model=671440&modification=LN51L-KRA&complectation=001&group=1")
        {
            List<SubGroup> subGroups = new List<SubGroup>();
            string line = "";
            using (WebClient wc = new WebClient())
            {
                wc.Headers.Add("user-agent", "Mozilla/5.0 (platform; rv:geckoversion) Gecko/geckotrail Firefox/firefoxversion");
                line = wc.DownloadString(link);
                line = Program.MyDecoder(line);
            }
            // Выбираем текст для дальнейшей обработки
            var subgroups_str = Regex.Match(line, "<div class='Tiles'><div class='List '>(.*?)</div></div></div></div>").Groups[1].Value + "</div>" + "</div>";

            var subgroups = Regex.Matches(subgroups_str, Resources.getSubGroups);

            foreach(Match subgroup in subgroups)
            {
                subGroups.Add(new SubGroup(subgroup.Groups[7].Value, subgroup.Groups[5].Value));
            }
            subGroups.RemoveRange(5, subgroups.Count-5);
            Console.WriteLine();
            return subGroups;


        }

        public static List<Detail> GetDetails(string link = "https://www.ilcats.ru/toyota/?function=getParts&market=EU&model=671440&modification=LN51L-KRA&complectation=001&group=1&subgroup=1904")
        {
            // Todo: разделение id на старый и новый, навести порядок в коде
            // 

            List<Detail> Details = new List<Detail>();
            string line = "";
            using (WebClient wc = new WebClient())
            {
                wc.Headers.Add("user-agent", "Mozilla/5.0 (platform; rv:geckoversion) Gecko/geckotrail Firefox/firefoxversion");
                line = wc.DownloadString(link);
                line = Program.MyDecoder(line);
            }

            // Get detail tabble string
            var details_table_str = Regex.Match(line, "<table(.*?)Data-brand='(.*?)'>(.*?)</table>").Groups[3].Value;

            // Get detail table list of the string
            var details_table = Regex.Matches(details_table_str, "<tr (.*?)>(.*?)</tr>").Cast<Match>().Select(x => x.Groups[2].Value).ToList<string>();
            //details_table.RemoveAt(0);
            int detail_count = details_table.Count;
            // Потому что сайт непредсказуем
            string tmp_code = "", tmp_tree = "";

            foreach (var item in details_table)
            {
                if (item.Substring(0,3) == "<th")
                {
                    // detail_header - ?
                    var header = Regex.Match(item, "<th(.*?)>(.*?)&nbsp;(.*?)</th>");
                    tmp_code = header.Groups[2].Value;
                    tmp_tree = header.Groups[3].Value;
                }
                else
                {
                    var body = Regex.Matches(item, "<td>(.*?)</td>")
                        .Cast<Match>().Select(x => x.Groups[1].Value).ToList();
                    var code = Regex.Match(body[0], "<div (.*?)href='(.*?)'(.*?)>(.*?)</a></div>");
                    var id = code.Groups[4].Value;
                    var lnk = code.Groups[2].Value;
                    var cnt = Regex.Match(body[1], "(.*?)>(.*?)<(.*?)");
                    var cn = cnt.Groups[2].Value;
                    var per = Regex.Match(body[2], "(.*?)>(.*?)<(.*?)");
                    var p = per.Groups[2].Value;
                    var info = Regex.Match(body[3], "<div(.*?)>(.*?)<br />(.*?)</div>");
                    var i = info.Groups[2].Value + "   " +info.Groups[3].Value;
                    Details.Add(new Detail(body[0], Convert.ToInt32(cnt.Groups[2].Value), info.Groups[2].Value, tmp_code, tmp_tree, per.Groups[2].Value));
                }
            }
            Console.WriteLine();
            return Details;
        }

        public static string ConvertPeriod(string raw_period)
        {
            string pattern = "&nbsp;";
            var reg = new Regex(pattern);
            string period = reg.Replace(raw_period, "");
            return period;
        }
    }
}
