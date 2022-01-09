using RegularX.Properties;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RegularX.Objs_auto
{
    class Detail
    {
        public string Tree_Code { get; }
        public string Tree { get; }
        public string Code { get; private set; }
        public string OldCode { get; private set; }
        public int Count { get; }
        public string Period { get; }
        public string Info { get; }
        public string Link { get; private set; }
        public string OldLink { get; private set; }
        public string ImgLink { get; private set; }

        public Detail(string code, int count, string info, string tree_code, string tree, string period, string imgLink)
        {
            Count = count; Info = info; Tree_Code = tree_code;
            Tree = tree;  Period = Controller.ConvertPeriod(period); OldLink = OldLink;
            ConvertCode(code);
            ImgLink = imgLink;
            Controller.detail_codes.Add(Code);
        }

        // Получение кода и ссылки, а так же старой ссылки и старого кода
        // на случай, если они имеются
        protected void ConvertCode(string raw_code)
        {
            string pattern = "replaceNumber";

            bool a = raw_code.Contains(pattern);
            if (a)
            {
                var code = Regex.Match(raw_code,
                    //1                                 2       3   4                                           5           6       7   8
                    "(.*?)<div class='number'><a href='(.*?)'(.*?)>(.*?)</a></div><div class='replaceNumber'>(.*?)<a href='(.*?)'(.*?)>(.*?)</a></div>"
                    );
                Code = code.Groups[4].Value;
                OldCode = code.Groups[8].Value;
                Link = code.Groups[6].Value;
                OldLink = code.Groups[2].Value;
            }
            else
            {
                var code = Regex.Match(raw_code,
                    "(.*?)<div class='number'><a href='(.*?)'(.*?)>(.*?)</a></div>"
                    );
                Code = code.Groups[4].Value;
                Link = code.Groups[2].Value;
            }
        }

        // Внесение в БД строки деталей
        public void InsertIntoDB(int subgroup_id, string modification)
        {
            string img_path_str = GetImgPath();
            string str_comand = "INSERT INTO details (detail_code, count, f_period, info, link, old_code, old_link, img_path, tree_code, tree, subgroup)" +
                "VALUES (@detail_code, @count, @f_period, @info, @link, @old_code, @old_link, @img_path, @tree_code, @tree, @subgroup)";
            SqlCommand sqlComand = new SqlCommand(str_comand, Controller.sqlConnection);

            sqlComand.Parameters.AddWithValue("detail_code", this.Code);
            sqlComand.Parameters.AddWithValue("count", this.Count);
            sqlComand.Parameters.AddWithValue("f_period", this.Period);
            sqlComand.Parameters.AddWithValue("info", this.Info);
            sqlComand.Parameters.AddWithValue("link", this.Link);
            sqlComand.Parameters.AddWithValue("old_code", this.OldCode == null ? "null" : this.OldCode);
            sqlComand.Parameters.AddWithValue("old_link", this.OldLink == null ? "null" : this.OldLink);
            sqlComand.Parameters.AddWithValue("img_path", img_path_str);
            sqlComand.Parameters.AddWithValue("tree_code", this.Tree_Code);
            sqlComand.Parameters.AddWithValue("tree", this.Tree);
            sqlComand.Parameters.AddWithValue("subgroup", subgroup_id);
            try
            {
                sqlComand.ExecuteNonQuery();
            } // в дальнейшем этоту обработку планирую перенести в хранимую процедуру или в триггер
            catch (Exception ex) {
                var m = ex.Message;
            }

            str_comand = "INSERT INTO detail_complectation_s (detail_code, complectation_code)"
                + "VALUES (@detail_code, @complectation_code)";
            sqlComand = new SqlCommand(str_comand, Controller.sqlConnection);

            sqlComand.Parameters.AddWithValue("detail_code", this.Code);
            sqlComand.Parameters.AddWithValue("complectation_code", modification);
            try
            {
                sqlComand.ExecuteNonQuery();
            } // в дальнейшем этоту обработку планирую перенести в хранимую процедуру или в триггер
            catch (Exception ex)
            {}

        }

        // Получение уникального имени изображения для его внесения в БД
        // если файл не существует, то он скачивается с сайта
        private string GetImgPath()
        {
            string path = "";
            try
            {
                string catalog = "images";
                Directory.CreateDirectory(catalog);
                path = $@"{catalog}\{GetHash(this.ImgLink)}.png";

                if (!File.Exists(path))
                {
                    using (WebClient client = new WebClient())
                    {
                        //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                        client.DownloadFile(this.ImgLink, path);
                    }
                }
            }
            catch (Exception ex) { path = "null"; }

            return path;
        }

        // Получение хеш-суммы
        static string GetHash(string plaintext)
        {
            return string.Join("", (new SHA1Managed().ComputeHash(Encoding.UTF8.GetBytes(plaintext))).Select(x => x.ToString("X2")).ToArray());
        }

    }
}
