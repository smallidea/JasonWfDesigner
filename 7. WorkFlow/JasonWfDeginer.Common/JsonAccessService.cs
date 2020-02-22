// ******************************************************************
// ** Copyright：Copyright (c) 2020
// ** Project：JasonWfDesigner.Common
// ** Create Date：2020-02-21 14:31
// ** Created by：陈晓平
// ** Blog：http://smallidea.cnblogs.com
// ** Git：http://smallidea.github.com
// ** Email: smallidea@126.com
// ** Version：v 1.0
// ** Last Modified: 2020-02-21 15:55
// ** Desc：JsonAccessService.cs
// ** Log: 每一个坑都源于精心的设计！每段垃圾代码的都是“故意”的！
// ******************************************************************

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using JasonWfDesigner.Common.Lib;

namespace JasonWfDesigner.Common
{
    public class JsonAccessService : IDatabaseAccessService
    {
        private readonly string _folderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data/");

        public int SaveDiagramView(Diagram4Serialize data)
        {
            createDirectory(_folderPath);

            /*            var fileName = data.Id;
                        if (fileName <= 0)
                        {
                            var files = Directory.GetFiles(_folderPath).Where(a => a.EndsWith("json"));
                            var enumerable = files as string[] ?? files.ToArray();
                            if (enumerable.Any())
                            {
                                int[] ids = enumerable.Select(a => int.Parse(Path.GetFileName(a).Split('.')[0])).ToArray();
                                int max = ids.Max();
                                fileName = max + 1;
                            }
                        }*/

            if (data.Id <= 0)
                data.Id = int.Parse(Path.GetFileNameWithoutExtension(data.FileName) ??
                                    throw new InvalidOperationException());

            var filePath = Path.Combine(_folderPath, data.FileName);
            using (var write = new StreamWriter(filePath, false, Encoding.UTF8))
            {
                write.Write(JsonHelper.ConvertToStr(data));
            }

            return data.Id;
        }

        public Diagram4Serialize LoadDiagramView(int diagramId)
        {
            createDirectory(_folderPath);
            var filePath = Path.Combine(_folderPath, diagramId + ".json");
            return JsonHelper.GetJsonObjectFromFile<Diagram4Serialize>(filePath);
        }

        public Diagram4Serialize LoadDiagramView(string fileName)
        {
            createDirectory(_folderPath);
            var filePath = Path.Combine(_folderPath, fileName + ".json");
            return JsonHelper.GetJsonObjectFromFile<Diagram4Serialize>(filePath);
        }

        public void DeleteConnection(int connectionId)
        {
            throw new NotImplementedException();
        }

        public void DeleteDesignerItem<T>(int settingsDesignerItemId) where T : DesignerItemBase
        {
            throw new NotImplementedException();
        }

        public IEnumerable<DiagramItem> FetchAllDiagram()
        {
            createDirectory(_folderPath);

            var list = new List<DiagramItem>();
            var files = Directory.GetFiles(_folderPath).Where(a => a.EndsWith("json"));
            var enumerable = files as string[] ?? files.ToArray();
            if (enumerable.Any())
                foreach (var file in enumerable)
                {
                    var filePath = Path.Combine(_folderPath, file);
                    var item = JsonHelper.GetJsonObjectFromFile<Diagram4Serialize>(filePath);
                    var diagramItem = new DiagramItem();
                    diagramItem.Id = item.Id; //int.Parse(Path.GetFileName(file).Split('.')[0]);
                    diagramItem.FileName = Path.GetFileNameWithoutExtension(file);
                    //  diagramItem.DesignerItems  
                    list.Add(diagramItem);
                }

            return list;
        }

        public IEnumerable<string> FetchAllDiagramFiles()
        {
            createDirectory(_folderPath);

            var list = new List<string>();
            var files = Directory.GetFiles(_folderPath).Where(a => a.EndsWith("json"));
            var enumerable = files as string[] ?? files.ToArray();
            if (enumerable.Any()) list.AddRange(enumerable.Select(Path.GetFileNameWithoutExtension));
            return list;
        }

        public Connection FetchConnection(int connectionId)
        {
            throw new NotImplementedException();
        }

        public T FetchDesignerItem<T>(int designerItemId) where T : DesignerItemBase
        {
            throw new NotImplementedException();
        }

        public DiagramItem FetchDiagram(int diagramId)
        {
            throw new NotImplementedException();
        }

        public int SaveConnection(Connection connectionToSave)
        {
            throw new NotImplementedException();
        }

        public int SaveDesignerItem(DesignerItemBase designerItem)
        {
            throw new NotImplementedException();
        }

        public int SaveDiagram(DiagramItem diagram)
        {
            throw new NotImplementedException();
        }


        private void createDirectory(string directoryPath)
        {
            if (false == Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath); //新建文件夹   
        }
    }
}