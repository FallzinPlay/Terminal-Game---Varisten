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
        public double Price {  get; protected set; }

        public void SetPrice(double value)
        {
            Price = Math.Round(value, 2);
        }

        public virtual string ShowInfo(LanguagesManager s)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(
                $"[{Name}]\n" +
                $"{s.GetSubtitle("Status", "price")}: {Price.ToString("F2", CultureInfo.InvariantCulture)}");
            return sb.ToString();
        }
    }
}
