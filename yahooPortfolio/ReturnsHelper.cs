using System.Collections.Generic;
using MathNet.Numerics.Statistics;
using System.Linq;

namespace yahooPortfolio
{
    
    public static class Helpers {

        public static double calculateHoldingPeriodReturn(double start, double end) {
            return (end-start)/start;
        }

        public static double calculateCorrelationOfPortfolioReturns(List<TimeAndPrice> portfolio1, List<TimeAndPrice> portfolio2)
        {
            return Correlation.Pearson(GeneratePortfolioReturnsList(portfolio1).Select(t => t.px).ToArray(), GeneratePortfolioReturnsList(portfolio2).Select(t => t.px).ToArray());
        }

        public static List<TimeAndPrice> GeneratePortfolioReturnsList(List<TimeAndPrice> r)
        {
            var idx = 0;
            List<TimeAndPrice> returns = new List<TimeAndPrice>();
            r.ForEach(ra =>
            {
                if (idx >= 1)
                {
                    var rtn = calculateHoldingPeriodReturn(r.ToArray()[idx - 1].px, ra.px);
                    returns.Add(new TimeAndPrice { ts = ra.ts, px = rtn });
                }
                idx++;
            });
            return returns;
        }
    }
}