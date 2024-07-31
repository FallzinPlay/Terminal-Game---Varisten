using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Game.ClassManager
{
    public abstract class ItemCreate : Identifier
    {
        public string Name { get; set; }
        public double MinPrice { get; protected set; }
        public double MaxPrice { get; protected set; }

        public virtual string ShowInfo(LanguagesManager s)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(
                $"[{Name}]\n" +
                $"{s.GetSubtitle("Status", "averagePrice")}: {((MinPrice + MaxPrice) / 2).ToString("F2", CultureInfo.InvariantCulture)}");
            return sb.ToString();
        }
    }
}
