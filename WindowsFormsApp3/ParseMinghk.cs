using HtmlAgilityPack;
using System.Collections.Generic;
using System.Text;


namespace WindowsFormsApp3
{
    class ParseMinghk
    {
        ParseAvito parse = new ParseAvito();
        string adress;
        string avitoLink;
        string request;
        int pageNumber = 1;

        //Парсинг ссылке с Дом.МинЖКХ
        void mingkhLink()
        {
            if (pageNumber == 1) adress = parse.AvitoParse(avitoLink);
            string linkAdress;
            request = "https://dom.mingkh.ru/search?address=";
            linkAdress = adress.Replace(" ", "+");
            string[] forSearch = linkAdress.Split(',');
            for (int i = 0; i < forSearch.Length; i++)
            {
                if (i == forSearch.Length - 1)
                {
                    request += forSearch[i] + "&searchtype=house";
                    break;
                }
                request += forSearch[i] + "%2C";
            }
        }

        //Парсинг страницы
        List<string> listInfo = new List<string>();
        void parseMingkh()
        {
            HtmlAgilityPack.HtmlDocument hd = new HtmlAgilityPack.HtmlDocument();
            var web = new HtmlWeb()
            {
                AutoDetectEncoding = false,
                OverrideEncoding = Encoding.UTF8
            };
            if (request == null) mingkhLink();
            hd = web.Load(request);
            foreach (HtmlNode item in hd.DocumentNode.Descendants("td")) //Информация о домах 
            {
                listInfo.Add(item.InnerText);
            }
        }


        // Проверка 
        public string InfoHomeList(string avitoLink)
        {
            this.avitoLink = avitoLink; //Передаем ссылку на ваито для получения адреса 
            parseMingkh();
            List<string> infoHome = new List<string>();
            int coincidence = 0;

            string[] adress_MingkhSR = adress_Mingkh(); //Получаем отформатированный 
            for (int i = 0; i < listInfo.Count; i += 7) //Добаавляем в 1 список спаршенные адреса
            {
                coincidence = 0;
                string home = (listInfo[i + 1] + " " + listInfo[i + 2]);    //Формируем 1 список для 1 дома
                home = home.Replace("д.", "");
                home = home.Replace(", ", "");
                home = (home + " ");
                //поиск совпадений
                for (int j = 0; j < adress_MingkhSR.Length; j++)
                {
                    if (home.Contains((adress_MingkhSR[j] + " ")) == true) coincidence++;
                    if (coincidence == 3) //При совпадении 
                    {
                        return ("Адрес: " + listInfo[i + 1] + " " + listInfo[i + 2] + "\nПлощадь м2: " + listInfo[i + 3] +
                                  "\nГод год постройки: " + listInfo[i + 4] + "\nКоличество этажей: " + listInfo[i + 5] +
                                  "\nЖилых помещений: " + listInfo[i + 6]);
                    }
                }
            }
            //Если на 1 странице нет нужного дома
            if (listInfo.Count > 0 && coincidence != 3)
            {
                listInfo.Clear();
                request += $"&page={pageNumber++}";
                InfoHomeList(avitoLink);
            }
            else if (listInfo.Count > 0 && pageNumber > 2 && coincidence != 3)
            {
                listInfo.Clear();
                request = request.Replace($"&page={pageNumber}", $"&page={pageNumber++}");
            }
            return "Не найдено";
        }


        //Форматирование адреса полученного с Авито для поиска
        string[] adress_Mingkh()
        {
            string adressCorrect = adress.Substring(adress.IndexOf("область") + 9);
            adressCorrect = adressCorrect.Replace("пр-т ", "");
            adressCorrect = adressCorrect.Replace("пер.", "");
            adressCorrect = adressCorrect.Replace("пр-д ", "");
            adressCorrect = adressCorrect.Replace("ул. ", "");
            adressCorrect = adressCorrect.Replace("б-р", "");
            adressCorrect = adressCorrect.Replace(" ул. ", " ");

            string[] adressMinghk = adressCorrect.Split(',');
            return adressMinghk;
        }
    }
}

//listInfo[i + 1] + " " + listInfo[i + 2] + " " + listInfo[i + 3] + " " +
//listInfo[i + 4] + " " + listInfo[i + 5] + " " + listInfo[i + 6] + " " +listInfo[i + 7]