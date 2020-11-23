using System;
using System.Collections.Generic;
namespace yahooPortfolio
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Clear();
            Console.WriteLine("   For a portfolio consisting of 1 share of each stock");
            PrintPortfolioReturns(OneShareOfEachStock());
            Console.WriteLine("Daily Returns on 25% of each stock");
            PrintPortfolioReturns(percentagePortfolio());
            double correlation = Helpers.calculateCorrelationOfPortfolioReturns(OneShareOfEachStock(), percentagePortfolio());
            Console.WriteLine("Correlation between the two portfolios {0}", correlation);
        }

        private static DateTime fromUnixToDateTime(double unixtime)
        {
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddMilliseconds(unixtime * 1000).ToLocalTime();
            return dtDateTime;
        }

        private static void PrintPortfolioReturns(List<TimeAndPrice> r)
        {
            var idx = 0;
            r.ForEach(ra =>
            {
                if (idx >= 1)
                {
                    Console.WriteLine("{0} {1} {2}", fromUnixToDateTime(ra.ts).Date.ToShortDateString(), ra.px, Helpers.calculateHoldingPeriodReturn(r.ToArray()[idx - 1].px, ra.px).ToString("P"));
                }
                else
                {
                    Console.WriteLine("{0} {1}", fromUnixToDateTime(ra.ts).Date.ToShortDateString(), ra.px);
                }
                idx++;
            });
            var monthlyReturn = Helpers.calculateHoldingPeriodReturn(r.ToArray()[0].px, r.ToArray()[r.Count - 1].px);
            Console.WriteLine("MONTHLY RETURN = {0}", monthlyReturn.ToString("P"));
        }

        private static List<TimeAndPrice> OneShareOfEachStock()
        {
            List<TimeAndPrice> amzn = StockData.getAMZN();
            List<TimeAndPrice> fb = StockData.getFB();
            List<TimeAndPrice> goog = StockData.getGOOG();
            List<TimeAndPrice> nflx = StockData.getNFLX();
            
            List<TimeAndPrice> combinedPortfolio = new List<TimeAndPrice>();
            amzn.ForEach(a =>
            {
                // calculate total of 1 share of each.
                var total = a.px + fb.Find(f => f.ts == a.ts).px + goog.Find(f => f.ts == a.ts).px + nflx.Find(f => f.ts == a.ts).px;
                combinedPortfolio.Add(new TimeAndPrice { ts = a.ts, px = total });
            });
            return combinedPortfolio;
        }

        private static List<TimeAndPrice> percentagePortfolio()
        {
            List<TimeAndPrice> amzn = StockData.getAMZN();
            List<TimeAndPrice> fb = StockData.getFB();
            List<TimeAndPrice> goog = StockData.getGOOG();
            List<TimeAndPrice> nflx = StockData.getNFLX();
            
            List<TimeAndPrice> combinedPortfolio = new List<TimeAndPrice>();
            var idx = 0;
            var amznShares = 250 / amzn.ToArray()[0].px;
            var fbShares = 250 / fb.ToArray()[0].px;
            var googShares = 250 / goog.ToArray()[0].px;
            var nflxShares = 250 / nflx.ToArray()[0].px;
            amzn.ForEach(a =>
            {
                // calculate total of 1 share of each.
                var total = (a.px * amznShares) +
                fb.Find(f => f.ts == a.ts).px * fbShares +
                goog.Find(f => f.ts == a.ts).px * googShares +
                nflx.Find(f => f.ts == a.ts).px * nflxShares;
                combinedPortfolio.Add(new TimeAndPrice { ts = a.ts, px = total });
                
            });
            return combinedPortfolio;
        }
    }
}
