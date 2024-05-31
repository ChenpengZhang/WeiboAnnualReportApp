using Microsoft.Win32;
using System.Diagnostics;
using System.Reflection;

namespace WeiboAnnualReportApp
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
        }

        private void btnOpenReport_Click(object sender, EventArgs e)
        {
            string basePath = Path.GetDirectoryName(Application.ExecutablePath);  // 找到那个exe文件所在的位置
            string mainHtmlPath = basePath + Path.DirectorySeparatorChar + "html" + Path.DirectorySeparatorChar + "MainPage.html";
            Process.Start("C:\\Program Files (x86)\\Microsoft\\Edge\\Application\\msedge.exe", mainHtmlPath);
        }

        private void btnCreateReport_Click(object sender, EventArgs e)
        {
            string basePath = Path.GetDirectoryName(Application.ExecutablePath);
            string mainHtmlPath = basePath + Path.DirectorySeparatorChar + "html" + Path.DirectorySeparatorChar + "MainPage.html";
            string html = "<!DOCTYPE html>\r\n" +//页面声明
                "<html lang=\"en\">\r\n" +
                "<head>\r\n" +
                "<meta charset=\"UTF-8\">\r\n" +
                "<meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">\r\n" +
                "<title>上拉切换页面</title>\r\n" +

                "<style>\r\n" +//css文件编辑，样式创建
                "body, html {\r\n" +
                "margin: 0;\r\n" +
                "padding: 0;\r\n" +
                "overflow: hidden;\r\n" +
                "height: 100%;\r\n" +
                "font-family: Arial, sans-serif;\r\n" +
                "}\r\n" +

                ".page {\r\n" +
                "position: absolute;\r\n" +
                "width: 100%;\r\n" +
                "height: 100%;\r\n" +
                "display: flex;\r\n" +
                "justify-content: center;\r\n" +
                "align-items: center;\r\n" +
                "transition: transform 0.5s ease-in-out;\r\n" +
                "}\r\n" +

                ".s-fcRed{\r\n" +
                "color: red; /* 设置文字颜色为红色 */\r\n" +
                "font-style: italic; /* 设置文字为斜体 */\r\n" +
                "}\r\n" +
                ".text-wrapper {\r\n" +
                "position: absolute;\r\n" +
                "top: 50%; /* 将文字包裹容器置于页面的垂直中间 */\r\n" +
                "transform: translateY(-50%);\r\n" +
                "width: 100%;\r\n" +
                "text-align: center; /* 文字居中 */\r\n" +
                "}\r\n" +

                "#page1 {\r\n" +
                "background-color: #f1c40f;\r\n" +
                "transform: translateY(0);\r\n" +
                "}\r\n" +

                "#page2 {\r\n" +
                "background-color: #e74c3c;\r\n" +
                "transform: translateY(100%);\r\n" +
                "}\r\n" +

                "#page3 {\r\n" +
                "background-color: #3498db;\r\n" +
                "transform: translateY(200%);\r\n" +
                "}\r\n" +

                "#page4 {\r\n" +
                "background-color: #3498db;\r\n" +
                "transform: translateY(300%);\r\n" +
                "}\r\n" +

                "#page5 {\r\n" +
                "background-color: #3498db;\r\n" +
                "transform: translateY(400%);\r\n" +
                "}\r\n" +

                "#page6 {\r\n" +
                "background-color: #3498db;\r\n" +
                "transform: translateY(500%);\r\n" +
                "}\r\n" +
                "</style>\r\n" +
                "</head>\r\n" +

                "<body>\r\n" +//页面主体编辑

                "<div class=\"container\" id=\"container\">\r\n" +

                "<div class=\"page\" id=\"page1\">\r\n" +//第一张页面
                "<div class=\"text-wrapper\">\r\n" +
                "<h1 style=\"letter-spacing:.2em;font-size:29px;\">我的旅行日记</h1>\r\n" +
                "<p style=\"letter-spacing:.3em;font-size:14px;\">二零一四年度微博使用报告</p>\r\n" +
                "</div>\r\n" +
                "</div>\r\n" +

                "<div class=\"page\" id=\"page2\">\r\n" +//第二张页面
                "<div class=\"text-wrapper\">\r\n" +
                "<h1>这一年里</h1>\r\n" +
                "<p>你一共去了<em class=\"s-fcRed\">13</em>个城市</p>\r\n" +
                "<p>待得最久的是<em class=\"s-fcRed\">南京</em></p>\r\n" +
                "<p>&nbsp;</p>\r\n" +
                "<p>你旅行的风格成谜</p>\r\n" +
                "<p>喜欢在<em class=\"s-fcRed\">晚上</em>发表微博</p>\r\n" +
                "<p>&nbsp;</p>\r\n" +
                "<p>你最喜欢说的词语是<em class=\"s-fcRed\">好热啊</em></p>\r\n" +
                "</div>\r\n" +
                "</div>\r\n" +

                "<div class=\"page\" id=\"page3\">\r\n" +//第三张页面
                "<div class=\"text-wrapper\">\r\n" +
                "<p><em class=\"s-fcRed\">8月19日</em></p>\r\n" +
                "<p>这一天你睡得很晚</p>\r\n" +
                "<p><em class=\"s-fcRed\">4：11</em>还在于微博相伴</p>\r\n" +
                "<p>&nbsp;</p>\r\n" +
                "<p>那一刻你发了个微博</p>\r\n" +
                "<p><em class=\"s-fcRed\">你见过凌晨四点的苏州吗</em></p>\r\n" +
                "</div>\r\n" +
                "</div>\r\n" +

                "<div class=\"page\" id=\"page4\">\r\n" +//第四张页面
                "<div class=\"text-wrapper\">\r\n" +
                "<p>这是第四页</p>\r\n" +
                "</div>\r\n" +
                "</div>\r\n" +

                "<div class=\"page\" id=\"page5\">\r\n" +//第五张页面
                "<div class=\"text-wrapper\">\r\n" +
                "<p>这是第五页</p>\r\n" +
                "</div>\r\n" +
                "</div>\r\n" +

                "<div class=\"page\" id=\"page6\">\r\n" +//第六张页面
                "<div class=\"text-wrapper\">\r\n" +
                "<p>这是第六页</p>\r\n" +
                "</div>\r\n" +
                "</div>\r\n" +

                "</div>\r\n" +

                "<script>\r\n" +//Javascript编辑，滑动动画
                "document.addEventListener('DOMContentLoaded', () => {\r\n" +
                "let currentPage = 0;\r\n" +
                "const pages = document.querySelectorAll('.page');\r\n" +
                "const switchPage = (direction) => {\r\n" +
                "if (direction === 'up' && currentPage > 0) {\r\n" +
                "currentPage--;\r\n" +
                "} else if (direction === 'down' && currentPage < pages.length - 1) {\r\n" +
                "currentPage++;\r\n" +
                "}\r\n" +
                "pages.forEach((page, index) => {\r\n" +
                "page.style.transform = `translateY(${(index - currentPage) * 100}%)`;\r\n" +
                "});\r\n" +
                "};\r\n" +
                "function handleScroll(event) {\r\n" +
                "if (event.deltaY > 0) {\r\n" +
                "switchPage('down');\r\n" +//页面下滑
                "} else {\r\n" +
                "switchPage('up');\r\n" +//页面上滑
                "}\r\n" +
                "showPage(currentPage);\r\n" +
                "}\r\n" +
                "window.addEventListener('wheel', handleScroll);\r\n" +
                "});\r\n" +
                "</script>\r\n" +
                "</body>\r\n" +
                "</html>";
            File.WriteAllText(mainHtmlPath, html);
        }
    }
}
