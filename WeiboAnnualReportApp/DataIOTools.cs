using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeiboAnnualReportApp
{
    internal class DataIOTools
    {
        #region 程序集方法

        internal static MyMapObjects.moMapLayer LoadMapLayerFromShapeFile(string shapeFileName)
        {
            //说明：
            //（1）仅支持点、多线和多多边形三种图形类型（对应值为1,3,5）,如遇到上述三种类型以外的类型，则报错。
            //（2）根据shp文件路径，自动寻找相同路径下的dbf文件（存储属性数据的文件）。
            //（3）将自动寻找相应路径下的cpg文件，并根据该文件确定dbf文件的字符编码类型。如无cpg文件，
            //则将采用操作系统默认的字符编码类型，亦因此无法保证字符显示正确，如遇此种情况，
            //可以在本函数中设置相应的字符编码类型
            //（4）如果遇到日期型字段，则将其值转换为字符型
            //////////////以下为读取程序///////////////////
            //（1）读取.shp文件，返回几何图形的类型以及所有要素的几何图形
            Int32 sShapeTypeTag;   //几何类型
            List<MyMapObjects.moGeometry> sGeometries = ReadshpFile(shapeFileName, out sShapeTypeTag);
            //（2）读取.dbf文件
            //（2.1）获取同一路径下的.dbf文件和.cpg文件
            string sBodyFileName = Path.GetFileNameWithoutExtension(shapeFileName);//不含路径和扩展名
            string sDirectoryName = Path.GetDirectoryName(shapeFileName); //目录名
            string sdbfFileName = sDirectoryName + Path.DirectorySeparatorChar + sBodyFileName + ".dbf";
            string scpgFileName = sDirectoryName + Path.DirectorySeparatorChar + sBodyFileName + ".cpg";
            //（2.2）读取属性数据，返回字段集合以及所有要素的属性值集合
            MyMapObjects.moFields sFields;
            List<MyMapObjects.moAttributes> sAttributesList;
            ReaddbfFile(scpgFileName, sdbfFileName, out sFields, out sAttributesList);
            //（3）检验shp中的图形数量与dbf中的记录数是否相等，如否则报错，由调用程序处理
            if (sGeometries.Count != sAttributesList.Count)
            {
                throw new Exception("shp文件的图形数与dbf文件的记录数不等！");
            }
            //（3）建立要素集合
            //（3.1）确定要素几何类型
            MyMapObjects.moGeometryTypeConstant sShapeType = MyMapObjects.moGeometryTypeConstant.Point;
            if (sShapeTypeTag == 1)
            {
                sShapeType = MyMapObjects.moGeometryTypeConstant.Point;
            }
            else if (sShapeTypeTag == 3)
            {
                sShapeType = MyMapObjects.moGeometryTypeConstant.MultiPolyline;
            }
            else if (sShapeTypeTag == 5)
            {
                sShapeType = MyMapObjects.moGeometryTypeConstant.MultiPolygon;
            }
            else
            { }
            //（3.2）建立要素集合
            MyMapObjects.moFeatures sFeatures = new MyMapObjects.moFeatures();
            Int32 sFeatureCount = sGeometries.Count;
            for (Int32 i = 0; i <= sFeatureCount - 1; i++)
            {
                MyMapObjects.moFeature sFeature = new MyMapObjects.moFeature
                    (sShapeType, sGeometries[i], sAttributesList[i]);
                sFeatures.Add(sFeature);
            }
            //（4）建立图层
            //（4.1）从文件名获取图层名
            string sLayerName = sBodyFileName;
            //（4.2）建立图层
            MyMapObjects.moMapLayer sMapLayer = new MyMapObjects.moMapLayer
                (sLayerName, sShapeType, sFields);
            sMapLayer.Features = sFeatures;
            sMapLayer.UpdateExtent();
            return sMapLayer;
        }

        #endregion

        #region 私有函数

        //本函数返回所有几何图形集合，并返回参数shapeType的值
        private static List<MyMapObjects.moGeometry> ReadshpFile(string shapeFileName, out Int32 shapeType)
        {
            //（1）设置相关临时变量
            Int32 sTempInt;         //临时变量，仅用于跳过某些字节
            double sTempDouble;     //临时变量，仅用于跳过某些字节
            //（2）打开文件
            FileStream sStream = new FileStream(shapeFileName, FileMode.Open);
            BinaryReader sr = new BinaryReader(sStream);
            //（3）读取文件头部分
            sTempInt = sr.ReadInt32();         //文件代码
            for (Int32 i = 0; i <= 4; i++)
            {
                sTempInt = sr.ReadInt32();      //未被使用
            }
            sTempInt = sr.ReadInt32();          //文件长度
            sTempInt = sr.ReadInt32();          //版本：值为1000
            Int32 sShapeType = sr.ReadInt32();        //图形类型
            //判断图形类型是否为支持的类型，如不是，则报错，由调用程序处理
            if (sShapeType != 1 && sShapeType != 3 && sShapeType != 5)
            {
                sr.Close();
                sStream.Close();
                throw new Exception("目前不支持该类型的图形！");
            }
            for (Int32 i = 0; i <= 7; i++)
            {
                sTempDouble = sr.ReadDouble(); //图层范围，Xmin、Ymin、Xmax、Ymax、Zmin、Zmax、Mmin、Mmax
            }
            //（4）定义几何图形集合，为读取所有要素的坐标做准备
            List<MyMapObjects.moGeometry> sGeometries = new List<MyMapObjects.moGeometry>();
            //（5）顺序读取所有要素的几何图形                                                                                                             
            while (sStream.Position < sStream.Length)
            {
                sTempInt = sr.ReadInt32();         //当前记录号
                sTempInt = sr.ReadInt32();         //当前记录长度
                if (sShapeType == 1)                //点
                {
                    sTempInt = sr.ReadInt32();      //Shape类型
                    double sX = sr.ReadDouble();    //读取当前点的X坐标
                    double sY = sr.ReadDouble();    //读取当前点的Y坐标
                    MyMapObjects.moPoint sPoint = new MyMapObjects.moPoint(sX, sY);//新建点图形
                    sGeometries.Add(sPoint);         //加入point集合
                }
                else if (sShapeType == 3)           //多线
                {
                    sTempInt = sr.ReadInt32();      //Shape类型
                    double sXmin = sr.ReadDouble();       //边界盒，下同
                    double sYmin = sr.ReadDouble();
                    double sXmax = sr.ReadDouble();
                    double sYmax = sr.ReadDouble();
                    Int32 sPartCount = sr.ReadInt32();        //部件的数目（即简单Polyline的数目）
                    Int32 sPointCount = sr.ReadInt32();       //点的数目
                    Int32[] sPartStarts = new Int32[sPartCount]; //当前图形每个部件第一个点在点序列中的索引号
                                                                 //读取所有部件第一个的索引号
                    for (Int32 i = 0; i <= sPartCount - 1; i++)
                    {
                        sPartStarts[i] = sr.ReadInt32();
                    }
                    //读取所有点
                    MyMapObjects.moPoint[] sPoints = new MyMapObjects.moPoint[sPointCount];
                    for (Int32 i = 0; i <= sPointCount - 1; i++)
                    {
                        double sX = sr.ReadDouble();
                        double sY = sr.ReadDouble();
                        MyMapObjects.moPoint sPoint = new MyMapObjects.moPoint(sX, sY);//新建一个点
                        sPoints[i] = sPoint;
                    }
                    //建立部件集合
                    MyMapObjects.moParts sParts = new MyMapObjects.moParts();
                    for (Int32 i = 0; i <= sPartCount - 1; i++)
                    {
                        MyMapObjects.moPoints sPart = new MyMapObjects.moPoints();
                        Int32 sStartIndex, sEndIndex;//当前部件的首点和尾点的索引号
                        sStartIndex = sPartStarts[i];
                        if (i < sPartCount - 1)
                        { sEndIndex = sPartStarts[i + 1] - 1; }
                        else
                        { sEndIndex = sPointCount - 1; }
                        for (Int32 j = sStartIndex; j <= sEndIndex; j++)
                        {
                            sPart.Add(sPoints[j]);
                        }
                        sParts.Add(sPart);
                    }
                    //新建多线并更新范围
                    MyMapObjects.moMultiPolyline sMultiPolyline = new MyMapObjects.moMultiPolyline(sParts);
                    sMultiPolyline.UpdateExtent();
                    //加入多线集合
                    sGeometries.Add(sMultiPolyline);
                }
                else if (sShapeType == 5)           //多多边形
                {
                    sTempInt = sr.ReadInt32();      //Shape类型
                    double sXmin = sr.ReadDouble();       //边界盒，下同
                    double sYmin = sr.ReadDouble();
                    double sXmax = sr.ReadDouble();
                    double sYmax = sr.ReadDouble();
                    Int32 sPartCount = sr.ReadInt32();        //部件的数目（即简单Polygon的数目）
                    Int32 sPointCount = sr.ReadInt32();       //点的数目
                    Int32[] sPartStarts = new Int32[sPartCount]; //当前图形每个部件第一个点在点序列中的索引号
                    //读取所有部件第一个点的索引号
                    for (Int32 i = 0; i <= sPartCount - 1; i++)
                    {
                        sPartStarts[i] = sr.ReadInt32();
                    }
                    //读取所有点
                    MyMapObjects.moPoint[] sPoints = new MyMapObjects.moPoint[sPointCount];
                    for (Int32 i = 0; i <= sPointCount - 1; i++)
                    {
                        double sX = sr.ReadDouble();
                        double sY = sr.ReadDouble();
                        MyMapObjects.moPoint sPoint = new MyMapObjects.moPoint(sX, sY);//新建一个点
                        sPoints[i] = sPoint;
                    }
                    //建立部件集合
                    MyMapObjects.moParts sParts = new MyMapObjects.moParts();
                    for (Int32 i = 0; i <= sPartCount - 1; i++)
                    {
                        MyMapObjects.moPoints sPart = new MyMapObjects.moPoints();
                        Int32 sStartIndex, sEndIndex;//当前部件的首点和尾点的索引号
                        sStartIndex = sPartStarts[i];
                        if (i < sPartCount - 1)
                        { sEndIndex = sPartStarts[i + 1] - 1; }
                        else
                        { sEndIndex = sPointCount - 1; }
                        for (Int32 j = sStartIndex; j <= sEndIndex; j++)
                        {
                            sPart.Add(sPoints[j]);
                        }
                        sParts.Add(sPart);
                    }
                    //新建多多边形并更新范围
                    MyMapObjects.moMultiPolygon sMultiPolygon = new MyMapObjects.moMultiPolygon(sParts);
                    sMultiPolygon.UpdateExtent();
                    //加入多多边形集合
                    sGeometries.Add(sMultiPolygon);
                }
            }
            //（5）返回相关参数
            sr.Close();
            sStream.Close();
            shapeType = sShapeType;
            return sGeometries;
        }

        private static void ReaddbfFile(string cpgFileName, string dbfFileName, out MyMapObjects.moFields fields, out List<MyMapObjects.moAttributes> attributesList)
        {
            //（1）读取cpg文件获得字符编码，如果文件不存在，则使用操作系统默认编码
            Encoding sEncoding;
            if (File.Exists(cpgFileName) == true)
            {
                StreamReader scpgr = new StreamReader(cpgFileName, Encoding.Default);
                string sLine = scpgr.ReadLine();
                if (sLine == "UTF-8")
                { sEncoding = Encoding.UTF8; }
                else if (sLine == "UTF-7")
                { sEncoding = Encoding.UTF7; }
                else if (sLine == "UTF-32")
                { sEncoding = Encoding.UTF32; }
                else
                { sEncoding = Encoding.Default; }
                scpgr.Dispose();
            }
            else
            { sEncoding = Encoding.Default; }
            //（2）打开dbf文件
            FileStream sStream = new FileStream(dbfFileName, FileMode.Open);
            BinaryReader sr = new BinaryReader(sStream);
            //（3）读取文件头部分
            byte sFileType = sr.ReadByte();         //文件类型
            byte sYear = sr.ReadByte();             // + 1900后为末次修改年份
            byte sMonth = sr.ReadByte();            //末次修改月份
            byte sDay = sr.ReadByte();              //末次修改日
            Int32 sRecordCount = sr.ReadInt32();    //记录数
            Int16 sFileHeadLength = sr.ReadInt16(); //文件头长度
            Int16 sRecordLength = sr.ReadInt16();   //记录长度
            byte sTempByte;     //用于读取保留字节
            Int32 sTempInt32;   //用于读取无用字节
            for (Int32 i = 0; i <= 19; i++)
            {
                sTempByte = sr.ReadByte();     //保留字节
            }
            //（4）读取所有字段信息
            Int32 sFieldCount = (sFileHeadLength - 33) / 32;  //根据文件头长度计算出字段数
            string[] sFieldNames = new string[sFieldCount];     //存储所有字段名称
            char[] sFieldTypes = new char[sFieldCount];         //存储所有字段类型字符
            Int32[] sFieldLengths = new Int32[sFieldCount];      //存储所有字段长度 
            for (Int32 i = 0; i <= sFieldCount - 1; i++)
            {
                //（4.1）字段名称
                byte[] sTempBytes = sr.ReadBytes(10);       // 字段名10个字节
                //转换为字符串，并去除首尾所有空字符(编码为0)
                string sFieldName = sEncoding.GetString(sTempBytes).Trim((char)0);
                sFieldNames[i] = sFieldName;
                //（4.2）系统保留
                sTempByte = sr.ReadByte();     //保留字节
                //（4.3）字段类型
                char sFieldTypeChar = sr.ReadChar();        //表示字段类型的字符
                sFieldTypes[i] = sFieldTypeChar;
                //（4.4）内存地址
                sTempInt32 = sr.ReadInt32();
                //（4.5）字段长度
                byte sFieldLength = sr.ReadByte();
                sFieldLengths[i] = sFieldLength;
                //（4.6）小数位
                byte sFieldScale = sr.ReadByte();
                //（4.7）系统保留
                for (Int32 j = 0; j <= 13; j++)
                {
                    sTempByte = sr.ReadByte();
                }
            }
            sTempByte = sr.ReadByte();         //结束标记，回车符 & HD
            //（5）建立moFields对象
            MyMapObjects.moFields sFields = new MyMapObjects.moFields();
            for (Int32 i = 0; i <= sFieldCount - 1; i++)
            {
                //（5.1）建立moField
                char sFieldTypeChar = sFieldTypes[i];
                MyMapObjects.moValueTypeConstant sFieldValueType = MyMapObjects.moValueTypeConstant.dInt32;
                if (sFieldTypeChar == 'C')  //字符型
                {
                    sFieldValueType = MyMapObjects.moValueTypeConstant.dText;
                }
                else if (sFieldTypeChar == 'N')  //数值型
                {
                    sFieldValueType = MyMapObjects.moValueTypeConstant.dDouble;
                }
                else if (sFieldTypeChar == 'D')  //日期型
                {
                    sFieldValueType = MyMapObjects.moValueTypeConstant.dText;
                }
                else if (sFieldTypeChar == 'F')  //单精度浮点型
                {
                    sFieldValueType = MyMapObjects.moValueTypeConstant.dSingle;
                }
                else if (sFieldTypeChar == 'B')  //双精度浮点型
                {
                    sFieldValueType = MyMapObjects.moValueTypeConstant.dDouble;
                }
                else if (sFieldTypeChar == 'I')  //整型
                {
                    sFieldValueType = MyMapObjects.moValueTypeConstant.dInt32;
                }
                else
                {
                    sFieldValueType = MyMapObjects.moValueTypeConstant.dText;
                }
                MyMapObjects.moField sField = new MyMapObjects.moField(sFieldNames[i], sFieldValueType);
                sFields.Append(sField);
            }
            //（5）读取所有记录，并为每条记录建立一个moAttributes对象
            List<MyMapObjects.moAttributes> sAttributesList = new List<MyMapObjects.moAttributes>();
            for (Int32 i = 0; i <= sRecordCount - 1; i++)
            {
                sTempByte = sr.ReadByte();  //记录删除标记，实为空格
                //新建一个属性集合对象moAttributes
                MyMapObjects.moAttributes sAttributes = new MyMapObjects.moAttributes();
                for (Int32 j = 0; j <= sFieldCount - 1; j++)
                {
                    byte[] sTempBytes = sr.ReadBytes(sFieldLengths[j]);
                    //转换为字符串，并去除首尾空格字符（编码为32）                    
                    string sValueString = sEncoding.GetString(sTempBytes).Trim((char)32);
                    char sFieldTypeChar = sFieldTypes[j];
                    if (sFieldTypeChar == 'C')  //字符型
                    {
                        string sValue = sValueString;
                        sAttributes.Append(sValue);
                    }
                    else if (sFieldTypeChar == 'N')  //数值型
                    {
                        double sValue = 0;
                        if (sValueString != "")
                        {
                            sValue = Double.Parse(sValueString);
                        }
                        sAttributes.Append(sValue);
                    }
                    else if (sFieldTypeChar == 'D')  //日期型
                    {
                        string sValue = sValueString;
                        sAttributes.Append(sValue);
                    }
                    else if (sFieldTypeChar == 'F')  //单精度浮点型
                    {
                        float sValue = 0;
                        if (sValueString != "")
                        {
                            sValue = float.Parse(sValueString);
                        }
                        sAttributes.Append(sValue);
                    }
                    else if (sFieldTypeChar == 'B')  //双精度浮点型
                    {
                        double sValue = 0;
                        if (sValueString != "")
                        {
                            sValue = Double.Parse(sValueString);
                        }
                        sAttributes.Append(sValue);
                    }
                    else if (sFieldTypeChar == 'I')  //整型
                    {
                        Int32 sValue = 0;
                        if (sValueString != "")
                        {
                            sValue = Int32.Parse(sValueString);
                        }
                        sAttributes.Append(sValue);
                    }
                    else
                    {
                        string sValue = sValueString;
                        sAttributes.Append(sValue);
                    }
                }
                sAttributesList.Add(sAttributes);
            }
            sTempByte = sr.ReadByte();         //结束标志 & H1A
            //（6）返回对象
            fields = sFields;
            attributesList = sAttributesList;
        }

        #endregion
    }
}
