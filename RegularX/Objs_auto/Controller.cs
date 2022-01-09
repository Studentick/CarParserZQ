using RegularX.Properties;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace RegularX.Objs_auto
{
    //Основной класс логики приложения
    class Controller
    {
        // DataBase
        public static string con_str = ConfigurationManager.ConnectionStrings["CatalogDB"].ConnectionString;
        public static SqlConnection sqlConnection;

        static public int c_left = 0, c_top = 0; 
        static public List<string> ComplectsParas;
        public const string core_lnk = "https://www.ilcats.ru";
        static public List<string> detail_codes = new List<string>();
        public static Stopwatch clock = new Stopwatch(); 
        public static List<string> time_response = new List<string>();

        // Получение строки ответа от сайта и фиксация времени ожидания ответа
        // для просчёта среднего времени во время разработки
        public static string DebugStr(string link, WebClient wc)
        {
            clock.Restart();
            var g = wc.DownloadString(link); ;
            clock.Stop();
            time_response.Add(Convert.ToString(clock.ElapsedMilliseconds));
            return g;
        }

        // Дополнительная проверка подключения к БД
        // на всякий случай
        public static void CheckConnectionDB()
        {
            if (sqlConnection == null || sqlConnection.State != ConnectionState.Open)
            {
                sqlConnection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\CatalogDB.mdf;Integrated Security=True;Connect Timeout=30");
                sqlConnection.Open();
            }
        }

        // Получение списка моделей авто
        public static List<Model> GetModels(string link = "https://www.ilcats.ru/toyota/?function=getModels&market=EU")
        {
            ComplectsParas = new List<string>();
            List<Model> models = new List<Model>();
            string line = "";
            using (WebClient wc = new WebClient())
            {
                line = DebugStr(link, wc);
            }
            
            // Получение блока со списком авто
            Match match = Regex.Match(line, "<div class='List Multilist\'>(.*?)<div class='Advert Advert2'>");

            var multilist = match.Groups[1].Value;

            // Разделение на объекты моделей
            var m_l = Regex.Matches(multilist, "<div class='List'>(.*?)</div></div></div></div>")
                .Cast<Match>().Select(x => x.Groups[1].Value + "</div></div></div>").ToList<string>();
            // Получение параметров моделей       
            foreach (var it in m_l)     //              
            {
                var obj = Regex.Match(it, Resources.getModName);
                string model_name = obj.Groups[1].Value;
                var modells = obj.Groups[2].Value + "</div></div>";

                var m = Regex.Matches(modells, Resources.getModels);

                //Создание объектов моделей авто
                foreach(Match it_model in m)
                {
                    models.Add(new Model(it_model.Groups[2].Value, model_name, it_model.Groups[1].Value,
                        it_model.Groups[3].Value, it_model.Groups[4].Value));
                }
                
            }

            //Усечение списка обрабатываемых данных для ускорения работы программы
            models.RemoveRange(5, models.Count - 5);

            // Установка прогрес-бара
            // ==================================================
            ConsoleProgressBar cpb = new ConsoleProgressBar();
            Console.WriteLine("Прогресс:\n");
            cpb.WritePercent(0, 100, false);
            int step = 0;
            //Обработка списка моделей (запись в БД и 
            //внесение во внутренний список вложенных элементов)
            foreach (var model in models)
            {
                model.InsertIntoDB();
                var c1 = GetComplectations(model.Link, model.ModelCode);
                model.complectationsList.AddRange(c1);
                Console.Write($"{model.ModelCode} parced;\t");
                cpb.WritePercent(step, models.Count);
                step++;
                Thread.Sleep(2500); // Для обхода защиты сайта. 
                // Лучше ждать 2,5-3 сек, чем потом 6-12 и не иметь нормального доступа к сайту
            }
            // ==================================================
            
            return models;
        }

        // Получения списка помплектаций (модификаций) какой-то модели авто
        public static List<Complectation> GetComplectations(string link = "https://www.ilcats.ru/toyota/?function=getComplectations&market=EU&model=671440&startDate=198308&endDate=198903", 
            string parrent_id = null)
        {
            List<Complectation> complectations = new List<Complectation>();
            string line = "";
            using (WebClient wc = new WebClient())
            {
                // на всякий случай для обхода защиты
                // wc.Proxy = new WebProxy("92.255.202.72", 4145);
                // данный метод не пригодился, но не убрал на случай необходимости модификации кода
                wc.Headers.Add("user-agent", "Mozilla/5.0 (platform; rv:geckoversion) Gecko/geckotrail Firefox/firefoxversion");
                Console.Write("(");
                line = DebugStr(link, wc);
                Console.Write(")");
                line = Program.MyDecoder(line);
            }
            // Разбиение на строки таблицы (модификации/комплектации)
            var line1 = Regex.Match(line, "(.*?)<table(.*?)>(.*?)</table>(.*?)").Groups[3].Value;
            line = line1;
            var cortages = Regex.Matches(line, "<tr>(.*?)</tr>")
                .Cast<Match>().Select(x => x.Groups[1].Value).ToList<string>();

            // список ключей параметров. Потекциально у комплектации/модификации обрабатываемой 
            // модели автомобиля может быть 42 свойство, 3 из которых обязательные, остальные могут 
            // отсутствовать в зависимости от того чем автомобиль укомплектован, а так же от года выпуска.
            // создавать 42 свойства мне показалось не логичным поэтому я решил хранить эти данные 
            // в ивде словаря, а передавать их в БД в виде JSON-объекта
            List<string> keys = new List<string>();
            int counter = 0;
            // Пожертвовал оптимизацией ради читабельности кода
            foreach(var row in cortages)
            {
                // Получение ключей словаря
                if (counter == 0)
                {
                    keys = Regex.Matches(row, "<th>(.*?)</th>").Cast<Match>().Select(x => x.Groups[1].Value).ToList<string>();
                }
                // Получение начений словаря
                else
                {
                    List<string> valuesList = new List<string>();
                    var raw_values = Regex.Matches(row, "<td>(.*?)</td>").Cast<Match>()
                        .Select(x => x.Groups[1].Value).ToList<string>();
                    // Отладку довести сюда
                    int _len = raw_values.Count;
                    for (int i = 1; i < _len; i++)
                    {
                        // Получение всего остального
                        var param = Regex.Match(raw_values[i], Resources.getComplParam);
                        valuesList.Add(param.Groups[2].Value);
                    }
                    var _id_lnk = Regex.Match(raw_values[0], Resources.getComplIdLnk);
                    string _modification = _id_lnk.Groups[4].Value;
                    string _lnk = _id_lnk.Groups[2].Value;

                    var compl_params = keys.GetRange(2, keys.Count-2).Zip(valuesList.GetRange(1, valuesList.Count-1), (k, v) => 
                    new { Key = k, Value = v }).ToDictionary(x => x.Key, x => x.Value);

                    complectations.Add(
                        new Complectation(parrent_id, _modification, _lnk, valuesList[0], compl_params, counter));
                }
                counter++;
            }
            complectations.RemoveRange(3, complectations.Count-3);

            // Обработка списка полученых модификаций
            foreach (var complect in complectations)
            {
                complect.InsertIntoDB();
                var c1 = GetComplGroup(complect.Link, complect.Modification);
                complect.complectationGroups.AddRange(c1);
                Thread.Sleep(2500); // Для обхода защиты сайта.
            }
            return complectations;
        }

        // Получение списка групп автомобилей
        public static List<ComplectationGroups> GetComplGroup(string link = "https://www.ilcats.ru/toyota/?function=getGroups&market=EU&model=671440&modification=LN51L-KRA&complectation=001",
            string modif = "")
        {
            List<ComplectationGroups> compGroup = new List<ComplectationGroups>();
            string line = "";
            using (WebClient wc = new WebClient())
            {
                wc.Headers.Add("user-agent", "Mozilla/5.0 (platform; rv:geckoversion) Gecko/geckotrail Firefox/firefoxversion");
                Console.Write("|");
                line = DebugStr(link, wc);
                Console.Write("|");
                line = Program.MyDecoder(line);
            }
            // Выбираем текст для дальнейшей обработки
            var groups_str = Regex.Match(line, "<div class='List '>(.*?)</div></div></div><div").Groups[1].Value + "</div>" + "</div>";

            var groups = Regex.Matches(groups_str, Resources.getGroups);
            var a = groups[0].Groups[0].Value;

            int group_id = 1;
            foreach (Match group in groups)
            {
                compGroup.Add(new ComplectationGroups(group.Groups[4].Value, link, modif, group_id));
                //compGroup.Clear();
                group_id++;
            }
            // Обработка списка групп
            foreach (var group in compGroup)
            {
                var c1 = GetSubGroupCompl(group.GroupLink, group.Modification, group.GroupName);
                group.subgroups.AddRange(c1);
                Thread.Sleep(2500); // Для обхода защиты сайта.
            }
            return compGroup;
        }

        // Получение списка подгрупп автомобилей
        public static List<SubGroup> GetSubGroupCompl(string link = "https://www.ilcats.ru/toyota/?function=getSubGroups&market=EU&model=671440&modification=LN51L-KRA&complectation=001&group=1",
            string modif = "", string group_name = "")
        {
            List<SubGroup> subGroups = new List<SubGroup>();
            string line = "";
            using (WebClient wc = new WebClient())
            {
                wc.Headers.Add("user-agent", "Mozilla/5.0 (platform; rv:geckoversion) Gecko/geckotrail Firefox/firefoxversion");
                Console.Write("{");
                line = DebugStr(link, wc);
                Console.Write("}");
                line = Program.MyDecoder(line);
            }

            if (time_response.Count == 33)
            {
                var gg = time_response.Select(x => Convert.ToInt64(x)).ToList().Sum() / time_response.Count;
            }

            // Выбираем текст для дальнейшей обработки
            var subgroups_str = Regex.Match(line, "<div class='Tiles'><div class='List '>(.*?)</div></div></div></div>").Groups[1].Value + "</div>" + "</div>";
            string tst = @"<div class='List'><div class='image'><a href='(.*?)'\s(.*?)><img src='(.*?)' alt='(.*?)\s(.*?)\s(.*?)'>(.*?)<div class='name'><a href='(.*?)'\s(.*?)>(.*?)</a></div></div>";
            var subgroups = Regex.Matches(subgroups_str, tst);

            foreach(Match subgroup in subgroups)
            {
                subGroups.Add(new SubGroup(subgroup.Groups[10].Value, link, subgroup.Groups[5].Value));
            }
            subGroups.RemoveRange(5, subgroups.Count-5);

            // Обработка списка подгрупп
            foreach (var sg in subGroups)
            { 
                var sg_id = sg.InsertIntoDB(group_name);
                var c1 = GetDetails(sg.Link, sg_id, modif);
                sg.details.AddRange(c1);
                Thread.Sleep(2500); // Для обхода защиты сайта.
            }

            return subGroups;
        }

        // Получение списка деталей авто
        public static List<Detail> GetDetails(string link = "https://www.ilcats.ru/toyota/?function=getParts&market=EU&model=671440&modification=LN51L-KRA&complectation=001&group=1&subgroup=1904",
            int subgroup_id = -1, string modif = "")
        {
            List<Detail> Details = new List<Detail>();
            string line = "";
            using (WebClient wc = new WebClient())
            {
                wc.Headers.Add("user-agent", "Mozilla/5.0 (platform; rv:geckoversion) Gecko/geckotrail Firefox/firefoxversion");
                Console.Write("[");
                line = DebugStr(link, wc);
                Console.Write("]");
                line = Program.MyDecoder(line);
            }

            var img_path = "https:" + Regex.Match(line, "(.*?)<img src='(.*?)' alt='' usemap='#myMap(.*?)'>(.*?)").Groups[2].Value;
            

            // Get detail tabble string
            var details_table_str = Regex.Match(line, "<table(.*?)Data-brand='(.*?)'>(.*?)</table>").Groups[3].Value;

            // Get detail table list of the string
            var details_table = Regex.Matches(details_table_str, "<tr (.*?)>(.*?)</tr>").Cast<Match>().Select(x => x.Groups[2].Value).ToList<string>();
            int detail_count = details_table.Count;
            string tmp_code = "", tmp_tree = "";

            foreach (var item in details_table)
            {
                if (item.Substring(0,3) == "<th")
                {
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
                    var i = "";
                    if (info.Success == false)
                    {
                        info = Regex.Match(body[3], "<div(.*?)>(.*?)</div>");
                        i = info.Groups[2].Value;
                    }
                    else
                    i = info.Groups[2].Value + "   " +info.Groups[3].Value;
                    Details.Add(new Detail(body[0], Convert.ToInt32(cnt.Groups[2].Value), info.Groups[2].Value, tmp_code, 
                        tmp_tree, per.Groups[2].Value, img_path));
                }
            }
            try
            {
                Details.RemoveRange(5, Details.Count - 5);
            }
            catch (Exception) { }
            finally { /*Console.WriteLine("Drop details"); */}
            
            // Добавление деталей в БД
            foreach(var detail in Details)
            {
                detail.InsertIntoDB(subgroup_id, modif);
            }

            return Details;
        }

        // Очистка периода выпуска от служебных символов
        public static string ConvertPeriod(string raw_period)
        {
            string pattern = "&nbsp;";
            var reg = new Regex(pattern);
            string period = reg.Replace(raw_period, "");
            return period;
        }

        
    }
}
