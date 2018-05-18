using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using CodeFirst;
using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Extensions;
using AngleSharp.Parser.Html;


namespace Parsing_ASP.NET_MVC_.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult StartPage()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(string ParsingLink)
        {
            if (ParsingLink == "")
            {
                return HttpNotFound();
            }
            try
            {
                WebClient client = new WebClient();
                Stream data = client.OpenRead(ParsingLink);
                StreamReader reader = new StreamReader(data);

                string st = reader.ReadToEnd();

                var parser = new HtmlParser();
                var document = parser.Parse(st);

                List<string> posList = new List<string>();
                List<string> noList = new List<string>();
                List<string> driverList = new List<string>();
                List<string> carList = new List<string>();
                List<string> lapList = new List<string>();
                List<string> timeList = new List<string>();
                List<string> ptsList = new List<string>();

                string[] s = new string[100];
                int count = 0;

                foreach (var element in document.All.Where(m => m.LocalName == "td" && m.ClassName == "dark"))
                {
                    posList.Add(element.InnerHtml);
                }
                foreach (IElement element in document.QuerySelectorAll("td.dark.hide-for-mobile"))
                {
                    noList.Add(element.InnerHtml);
                }
                foreach (IElement element in document.QuerySelectorAll("span.hide-for-mobile"))
                {
                    driverList.Add(element.InnerHtml);
                }
                foreach (IElement element in document.QuerySelectorAll("td.semi-bold.uppercase.hide-for-tablet"))
                {
                    carList.Add(element.InnerHtml);
                }
                foreach (IElement element in document.QuerySelectorAll("td.bold.hide-for-mobile"))
                {
                    lapList.Add(element.InnerHtml);
                }
                foreach (IElement element in document.QuerySelectorAll("td.dark.bold")) // this method needs to be changed
                {
                    count++;
                    s = element.Text().Split(' ');
                    if (count % 2 == 0)
                        timeList.Add(s[0]);
                }
                foreach (var element in document.All.Where(m => m.LocalName == "td" && m.ClassName == "bold"))
                {
                    ptsList.Add(element.InnerHtml);
                }

                Collection listtt = new Collection();

                listtt.CoList.Add(posList);
                listtt.CoList.Add(noList);
                listtt.CoList.Add(driverList);
                listtt.CoList.Add(carList);
                listtt.CoList.Add(lapList);
                listtt.CoList.Add(timeList);
                listtt.CoList.Add(ptsList);

                data.Close();
                reader.Close();

                TempData["qw"] = listtt;

                return View(listtt);

            }
            catch (Exception e)
            {
                return HttpNotFound();
            }


        }

        public ActionResult Save()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Save(string button)
        {
        ///
        
            Collection qwerty = (Collection)TempData["qw"];

            DBContext context = new DBContext();

            for (int i = 0; i < 20; i++)
            {
                Team team = new Team
                {
                    TeamId = Guid.NewGuid(),
                    TeamName = qwerty.CoList[3][i],
                    DriverNumder = Convert.ToInt32(qwerty.CoList[1][i])
                    //Drivers = new List<Driver>()
                };
                Driver driver = new Driver
                {
                    DriverId = Guid.NewGuid(),
                    TeamId = team.TeamId,
                    DriverName = qwerty.CoList[2][i]

                };
                Result result = new Result
                {
                    //ResultId = Guid.NewGuid(),
                    DriverId = driver.DriverId,
                    Position = qwerty.CoList[0][i],
                    Laps = Convert.ToInt32(qwerty.CoList[4][i]),
                    Pts = Convert.ToInt32(qwerty.CoList[6][i]),
                    Time = qwerty.CoList[5][i]
                };
                context.Teams.AddOrUpdate(team);
                context.Drivers.AddOrUpdate(driver);
                context.Results.AddOrUpdate(result);
                context.SaveChanges();
            }

            ////Team team = new Team();
            ////Driver driver = new Driver();
            ////Result result = new Result();
            
            return View();
        }
    }
}
