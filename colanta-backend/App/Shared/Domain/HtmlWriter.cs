namespace colanta_backend.App.Shared.Domain
{
    public class HtmlWriter
    {
        public string h(string order, string innerHtml, string cssClass = "")
        {
            string html = "<h$ class = ''>" + innerHtml + "</h$>";
            html = html.Replace("$", order);
            html = html.Replace("%", cssClass);
            return html;
        }

        public string p(string innerHtml, string cssClass = "")
        {
            string html = "<p class = '%'>" + innerHtml + "</p>";
            html = html.Replace("%", cssClass);
            return html;
        }

        public string ol(string[] items, string cssClass = "")
        {
            string html = "<ol class='%'>";
            foreach (string item in items)
            {
                html += "<li>" + item + "</li>";
            }
            html += "</ol>";
            html = html.Replace("%", cssClass);
            return html;
        }

        public string ul(string[] items, string cssClass = "")
        {
            string html = "<ul class='%'>";
            foreach (string item in items)
            {
                html += "<li>" + item + "</li>";
            }
            html += "</ul>";
            html = html.Replace("%", cssClass);
            return html;
        }

        public string table(string[] headers, string[][] rows, string cssClass = "")
        {
            string html = "<table" + cssClass +">";
            html += "<thead>";
            html += "<tr>";
            foreach (string header in headers)
            {
                html += "<th>" + header + "</th>";
            }
            html += "</tr>";
            html += "</thead>";

            html += "<tbody>";
            foreach(string[] row in rows)
            {
                html += "<tr>";
                foreach(string column in row)
                {
                    html += "<td>" + column + "</td>";
                }
                html += "</tr>";
            }
            html += "</tbody>";
            html += "</table>";
            return html;
        }
    }
}
