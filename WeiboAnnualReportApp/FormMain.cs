using System.Diagnostics;
using System.Data;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;


namespace WeiboAnnualReportApp
{
    public partial class FormMain : Form
    {
        private string connectionString = "server=127.0.0.1;user=root;password=lys150619; database=������΢������";
        
        public FormMain()
        {
            InitializeComponent();
        }

        private void btnOpenReport_Click(object sender, EventArgs e)
        {
            string id = textBox1.Text;
            if (string.IsNullOrEmpty(id))
            {
                MessageBox.Show("Please enter an ID");
                return;
            }

            string basePath = Path.GetDirectoryName(Application.ExecutablePath);  // �ҵ��Ǹ�exe�ļ����ڵ�λ��
            string mainHtmlPath = basePath + Path.DirectorySeparatorChar + "html" +Path.DirectorySeparatorChar + "MainPage.html";
            OpenHtmlFile(mainHtmlPath);
            //webBrowserMap.Navigate(outputFilePath);
            //OpenHtmlFile(outputFilePath);
        }

        private void btnCreateReport_Click(object sender, EventArgs e)
        {
            string id = textBox1.Text;
            if (string.IsNullOrEmpty(id))
            {
                MessageBox.Show("Please enter an ID");
                return;
            }
            string query = $"SELECT latitude, longitude,text FROM ������΢������.geotaggedweibo WHERE ������΢������.geotaggedweibo.userid = @id";
            DataTable dt = new DataTable();
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@id", id);
                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);

                    adapter.Fill(dt);
                    dataGridView1.DataSource = dt;

                    // ����JavaScript��ʾ��ͼ�ͱ�ע��
                    //ShowMapWithPoints(dt);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            if (dt.Rows.Count > 0)
            {
                //ShowMapWithPoints(dt);
                GenerateHtmlFile(dt);
                
            }
            else
            {
                MessageBox.Show("No data found for the provided ID.");
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }


        private void GenerateHtmlFile(DataTable dt)
        {
            string basePath = Path.GetDirectoryName(Application.ExecutablePath);  // �ҵ��Ǹ�exe�ļ����ڵ�λ��
            string templateFilePath = basePath + Path.DirectorySeparatorChar + "html" + Path.DirectorySeparatorChar + "MainPage_template.html";
            string outputFilePath = basePath + Path.DirectorySeparatorChar + "html" + Path.DirectorySeparatorChar + "MainPage.html";

            var points = dt.AsEnumerable().Select(row => new
            {
                latitude = row.Field<double>("latitude"),
                longitude = row.Field<double>("longitude"),
                text = row.Field<string>("text")
            }).ToList();

            string json = JsonConvert.SerializeObject(points);
            string phrase1 = "  ";//ȥ�����ٳ���
            string phrase2 = "  ";//������õĳ���
            string phrase3 = "  ";//ϲ����ʲôʱ�䷢΢��������/����/���ϣ�
            string phrase4 = "  ";//��ϲ��˵�Ĵ���
            string phrase5 = "  ";//˯�������һ��
            string phrase6 = "  ";//������˯��
            string phrase7 = "  ";//�Ǹ�ʱ��η���΢��

            string template = File.ReadAllText(templateFilePath);
            string outputContent = template.Replace("{data_placeholder}", json)
            .Replace("{phrase1_placeholder}", phrase1)
            .Replace("{phrase2_placeholder}", phrase2)
            .Replace("{phrase3_placeholder}", phrase3)
            .Replace("{phrase4_placeholder}", phrase4)
            .Replace("{phrase5_placeholder}", phrase5)
            .Replace("{phrase6_placeholder}", phrase6)
            .Replace("{phrase7_placeholder}", phrase7);

            File.WriteAllText(outputFilePath, outputContent);
        }
        private void OpenHtmlFile(string filePath)
        {
            try
            {
                Process.Start("C:\\Program Files (x86)\\Microsoft\\Edge\\Application\\msedge.exe",filePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to open the HTML file: " + ex.Message);
            }
        }
    }
}





/*private void ShowMapWithPoints0(DataTable dt)
       {
           StringBuilder htmlBuilder = new StringBuilder();
           htmlBuilder.AppendLine("<!doctype html>");
           htmlBuilder.AppendLine("<html lang=\"en\">");
           htmlBuilder.AppendLine("<head>");
           htmlBuilder.AppendLine("    <meta charset=\"utf-8\">");
           htmlBuilder.AppendLine("    <meta http-equiv=\"X-UA-Compatible\" content=\"chrome=1\">");
           htmlBuilder.AppendLine("    <meta name=\"viewport\" content=\"initial-scale=1.0, user-scalable=yes, width=device-width\">");
           htmlBuilder.AppendLine("    <style type=\"text/css\">");
           htmlBuilder.AppendLine("        body, html, #container { height: 100%; margin: 0px; font: 12px Arial; }");
           htmlBuilder.AppendLine("        .taiwan { border: solid 1px red; color: red; float: left; width: 50px; background-color: rgba(255, 0, 0, 0.1); }");
           htmlBuilder.AppendLine("    </style>");
           htmlBuilder.AppendLine("    <title>Marker example</title>");
           htmlBuilder.AppendLine("</head>");
           htmlBuilder.AppendLine("<body>");
           htmlBuilder.AppendLine("    <div id=\"container\" tabindex=\"0\"></div>");
           htmlBuilder.AppendLine("    <script src=\"https://webapi.amap.com/js/marker.js\"></script>");
           htmlBuilder.AppendLine("    <script src=\"https://webapi.amap.com/maps?v=1.4.15&key=cd6579ead366b5d7c3b6802c2a5af67e\"></script>");
           htmlBuilder.AppendLine("    <script src=\"https://cdn.jsdelivr.net/npm/echarts-extension-amap@1.10.2/dist/echarts-extension-amap.min.js\"></script>");
           htmlBuilder.AppendLine("    <script type=\"text/javascript\">");

           htmlBuilder.AppendLine("        var map = new AMap.Map('container', { resizeEnable: true, zoom: 5 });");
           htmlBuilder.AppendLine("        var markers = [];");
           htmlBuilder.AppendLine("        var path = [];");

           foreach (DataRow row in dt.Rows)
           {
               string lat = row["latitude"].ToString();
               string lng = row["longitude"].ToString();
               string info = row["info"].ToString();

               htmlBuilder.AppendLine($"        var marker = new AMap.Marker({{");
               htmlBuilder.AppendLine($"            position: new AMap.LngLat({lng}, {lat}),");
               htmlBuilder.AppendLine($"            title: '{info}',");
               htmlBuilder.AppendLine("            map: map");
               htmlBuilder.AppendLine("        });");

               htmlBuilder.AppendLine($"        markers.push(marker);");
               htmlBuilder.AppendLine($"        path.push(new AMap.LngLat({lng}, {lat}));");

               htmlBuilder.AppendLine($"        var infoWindow = new AMap.InfoWindow({{");
               htmlBuilder.AppendLine($"            content: '{info}'");
               htmlBuilder.AppendLine("        });");

               htmlBuilder.AppendLine("        AMap.event.addListener(marker, 'click', (function(marker, infoWindow) {");
               htmlBuilder.AppendLine("            return function() {");
               htmlBuilder.AppendLine("                infoWindow.open(map, marker.getPosition());");
               htmlBuilder.AppendLine("            }");
               htmlBuilder.AppendLine("        })(marker, infoWindow));");
           }

           htmlBuilder.AppendLine("        var bezierCurve = new AMap.BezierCurve({");
           htmlBuilder.AppendLine("            path: path,");
           htmlBuilder.AppendLine("            strokeColor: '#FF33FF',");
           htmlBuilder.AppendLine("            strokeWeight: 6,");
           htmlBuilder.AppendLine("            strokeOpacity: 0.5,");
           htmlBuilder.AppendLine("            isOutline: true,");
           htmlBuilder.AppendLine("            outlineColor: '#000000'");

           htmlBuilder.AppendLine("        });");
           htmlBuilder.AppendLine("        bezierCurve.setMap(map);");
           htmlBuilder.AppendLine("        map.setFitView();");
           htmlBuilder.AppendLine("    </script>");
           htmlBuilder.AppendLine("    <script type=\"text/javascript\" src=\"https://webapi.amap.com/demos/js/liteToolbar.js\"></script>");
           htmlBuilder.AppendLine("</body>");
           htmlBuilder.AppendLine("</html>");

           webBrowserMap.DocumentText = htmlBuilder.ToString();
       }*/
/*private void ShowMapWithPoints(DataTable dt)
{
    string html = GenerateHtmlFile(dt);
    File.WriteAllText("map.html", html);
    webBrowserMap.Navigate(new Uri(Path.Combine(Application.StartupPath, "map.html")));
}

private string GenerateHtmlFile(DataTable dt)
{

    string htmlTemplate = $@"
    <!DOCTYPE html>
<html lang=""zh-CN"">
<head>
<meta charset=""utf-8"">
<title>MapVGL</title>
<meta http-equiv=""X-UA-Compatible"" content=""IE=Edge"">
<meta name=""viewport"" content=""initial-scale=1.0, user-scalable=no"">
<style>
html,
body {{
width: 100%;
height: 100%;
margin: 0;
padding: 0;
}}
#map_container {{
width: 100%;
height: 100%;
margin: 0;
}}
</style>
<script src=""https://api.map.baidu.com/api?v=1.0&type=webgl&ak=fpMKa6sGENtDsvgT3iA3MErslqRNK5Di""></script>
<script src=""https://unpkg.com/mapvgl/dist/mapvgl.min.js""></script>
<script src=""https://unpkg.com/mapvgl/dist/mapvgl.threelayers.min.js""></script>
</head>
<body>
<div id=""map_container""></div>
<script>
// 1. ������ͼʵ��
var bmapgl = new BMapGL.Map('map_container');
var point = new BMapGL.Point(116.403748, 39.915055);
bmapgl.centerAndZoom(point, 17);

// 2. ����MapVGLͼ�������
var view = new mapvgl.View({{
map: bmapgl
}});

// 3. �������ӻ�ͼ�㣬����ӵ�ͼ���������
var layer = new mapvgl.PointLayer({{
color: 'rgba(50, 50, 200, 1)',
blend: 'lighter',
size: 15
}});
view.addLayer(layer);

// 4. ׼���ù淶����������
var data = [{{
geometry: {{
type: 'Point',
coordinates: [116.403748, 39.915055]
}}
}}];

// 5. ����ͼ�������ݣ������𺳵Ŀ��ӻ�Ч��
layer.setData(data);
</script>
</body>
</html>
";

    return htmlTemplate;
}
*/


//string basePath = Path.GetDirectoryName(Application.ExecutablePath);
//string mainHtmlPath = basePath + Path.DirectorySeparatorChar + "html" + Path.DirectorySeparatorChar + "MainPage.html";
//string html = "<!DOCTYPE html>\r\n" +//ҳ������
//    "<html lang=\"en\">\r\n" +
//    "<head>\r\n" +
//    "<meta charset=\"UTF-8\">\r\n" +
//    "<meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">\r\n" +
//    "<title>�����л�ҳ��</title>\r\n" +

//    "<style>\r\n" +//css�ļ��༭����ʽ����
//    "body, html {\r\n" +
//    "margin: 0;\r\n" +
//    "padding: 0;\r\n" +
//    "overflow: hidden;\r\n" +
//    "height: 100%;\r\n" +
//    "font-family: Arial, sans-serif;\r\n" +
//    "}\r\n" +

//    ".page {\r\n" +
//    "position: absolute;\r\n" +
//    "width: 100%;\r\n" +
//    "height: 100%;\r\n" +
//    "display: flex;\r\n" +
//    "justify-content: center;\r\n" +
//    "align-items: center;\r\n" +
//    "transition: transform 0.5s ease-in-out;\r\n" +
//    "}\r\n" +

//    ".s-fcRed{\r\n" +
//    "color: red; /* ����������ɫΪ��ɫ */\r\n" +
//    "font-style: italic; /* ��������Ϊб�� */\r\n" +
//    "}\r\n" +
//    ".text-wrapper {\r\n" +
//    "position: absolute;\r\n" +
//    "top: 50%; /* �����ְ�����������ҳ��Ĵ�ֱ�м� */\r\n" +
//    "transform: translateY(-50%);\r\n" +
//    "width: 100%;\r\n" +
//    "text-align: center; /* ���־��� */\r\n" +
//    "}\r\n" +

//    "#page1 {\r\n" +
//    "background-color: #f1c40f;\r\n" +
//    "transform: translateY(0);\r\n" +
//    "}\r\n" +

//    "#page2 {\r\n" +
//    "background-color: #e74c3c;\r\n" +
//    "transform: translateY(100%);\r\n" +
//    "}\r\n" +

//    "#page3 {\r\n" +
//    "background-color: #3498db;\r\n" +
//    "transform: translateY(200%);\r\n" +
//    "}\r\n" +

//    "#page4 {\r\n" +
//    "background-color: #3498db;\r\n" +
//    "transform: translateY(300%);\r\n" +
//    "}\r\n" +

//    "#page5 {\r\n" +
//    "background-color: #3498db;\r\n" +
//    "transform: translateY(400%);\r\n" +
//    "}\r\n" +

//    "#page6 {\r\n" +
//    "background-color: #3498db;\r\n" +
//    "transform: translateY(500%);\r\n" +
//    "}\r\n" +
//    "</style>\r\n" +
//    "</head>\r\n" +

//    "<body>\r\n" +//ҳ������༭

//    "<div class=\"container\" id=\"container\">\r\n" +

//    "<div class=\"page\" id=\"page1\">\r\n" +//��һ��ҳ��
//    "<div class=\"text-wrapper\">\r\n" +
//    "<h1 style=\"letter-spacing:.2em;font-size:29px;\">�ҵ������ռ�</h1>\r\n" +
//    "<p style=\"letter-spacing:.3em;font-size:14px;\">����һ�����΢��ʹ�ñ���</p>\r\n" +
//    "</div>\r\n" +
//    "</div>\r\n" +

//    "<div class=\"page\" id=\"page2\">\r\n" +//�ڶ���ҳ��
//    "<div class=\"text-wrapper\">\r\n" +
//    "<h1>��һ����</h1>\r\n" +
//    "<p>��һ��ȥ��<em class=\"s-fcRed\">13</em>������</p>\r\n" +
//    "<p>������õ���<em class=\"s-fcRed\">�Ͼ�</em></p>\r\n" +
//    "<p>&nbsp;</p>\r\n" +
//    "<p>�����еķ�����</p>\r\n" +
//    "<p>ϲ����<em class=\"s-fcRed\">����</em>����΢��</p>\r\n" +
//    "<p>&nbsp;</p>\r\n" +
//    "<p>����ϲ��˵�Ĵ�����<em class=\"s-fcRed\">���Ȱ�</em></p>\r\n" +
//    "</div>\r\n" +
//    "</div>\r\n" +

//    "<div class=\"page\" id=\"page3\">\r\n" +//������ҳ��
//    "<div class=\"text-wrapper\">\r\n" +
//    "<p><em class=\"s-fcRed\">8��19��</em></p>\r\n" +
//    "<p>��һ����˯�ú���</p>\r\n" +
//    "<p><em class=\"s-fcRed\">4��11</em>������΢�����</p>\r\n" +
//    "<p>&nbsp;</p>\r\n" +
//    "<p>��һ���㷢�˸�΢��</p>\r\n" +
//    "<p><em class=\"s-fcRed\">������賿�ĵ��������</em></p>\r\n" +
//    "</div>\r\n" +
//    "</div>\r\n" +

//    "<div class=\"page\" id=\"page4\">\r\n" +//������ҳ��
//    "<div class=\"text-wrapper\">\r\n" +
//    "<p>���ǵ���ҳ</p>\r\n" +
//    "</div>\r\n" +
//    "</div>\r\n" +

//    "<div class=\"page\" id=\"page5\">\r\n" +//������ҳ��
//    "<div class=\"text-wrapper\">\r\n" +
//    "<p>���ǵ���ҳ</p>\r\n" +
//    "</div>\r\n" +
//    "</div>\r\n" +

//    "<div class=\"page\" id=\"page6\">\r\n" +//������ҳ��
//    "<div class=\"text-wrapper\">\r\n" +
//    "<p>���ǵ���ҳ</p>\r\n" +
//    "</div>\r\n" +
//    "</div>\r\n" +

//    "</div>\r\n" +

//    "<script>\r\n" +//Javascript�༭����������
//    "document.addEventListener('DOMContentLoaded', () => {\r\n" +
//    "let currentPage = 0;\r\n" +
//    "const pages = document.querySelectorAll('.page');\r\n" +
//    "const switchPage = (direction) => {\r\n" +
//    "if (direction === 'up' && currentPage > 0) {\r\n" +
//    "currentPage--;\r\n" +
//    "} else if (direction === 'down' && currentPage < pages.length - 1) {\r\n" +
//    "currentPage++;\r\n" +
//    "}\r\n" +
//    "pages.forEach((page, index) => {\r\n" +
//    "page.style.transform = `translateY(${(index - currentPage) * 100}%)`;\r\n" +
//    "});\r\n" +
//    "};\r\n" +
//    "function handleScroll(event) {\r\n" +
//    "if (event.deltaY > 0) {\r\n" +
//    "switchPage('down');\r\n" +//ҳ���»�
//    "} else {\r\n" +
//    "switchPage('up');\r\n" +//ҳ���ϻ�
//    "}\r\n" +
//    "showPage(currentPage);\r\n" +
//    "}\r\n" +
//    "window.addEventListener('wheel', handleScroll);\r\n" +
//    "});\r\n" +
//    "</script>\r\n" +
//    "</body>\r\n" +
//    "</html>";


//File.WriteAllText(mainHtmlPath, html);
