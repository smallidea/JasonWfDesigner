// ******************************************************************
// ** Copyright：Copyright (c) 2020
// ** Project：JasonWfDesigner.Common
// ** Create Date：2020-02-21 14:31
// ** Created by：陈晓平
// ** Blog：http://smallidea.cnblogs.com
// ** Git：http://smallidea.github.com
// ** Email: smallidea@126.com
// ** Version：v 1.0
// ** Last Modified: 2020-02-21 15:54
// ** Desc：IDatabaseAccessService.cs
// ** Log: 每一个坑都源于精心的设计！每段垃圾代码的都是“故意”的！
// ******************************************************************

using System.Collections.Generic;

namespace JasonWfDesigner.Common
{
    /// <summary>
    /// </summary>
    public interface IDatabaseAccessService
    {
        /// <summary>
        /// </summary>
        /// <param name="connectionId"></param>
        void DeleteConnection(int connectionId);

        /// <summary>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="settingsDesignerItemId"></param>
        void DeleteDesignerItem<T>(int settingsDesignerItemId) where T : DesignerItemBase;

        /// <summary>
        /// </summary>
        /// <param name="diagram"></param>
        /// <returns></returns>
        int SaveDiagram(DiagramItem diagram);

        /// <summary>
        /// </summary>
        /// <param name="diagram"></param>
        /// <returns></returns>
        int SaveDiagramView(Diagram4Serialize diagram);

        /// <summary>
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Diagram4Serialize LoadDiagramView(int id);

        /// <summary>
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        Diagram4Serialize LoadDiagramView(string fileName);

        /// <summary>
        /// </summary>
        /// <param name="designerItem"></param>
        /// <returns></returns>
        int SaveDesignerItem(DesignerItemBase designerItem);

        /// <summary>
        /// </summary>
        /// <param name="connectionToSave"></param>
        /// <returns></returns>
        int SaveConnection(Connection connectionToSave);

        /// <summary>
        /// </summary>
        /// <returns></returns>
        IEnumerable<DiagramItem> FetchAllDiagram();

        /// <summary>
        /// </summary>
        /// <returns></returns>
        IEnumerable<string> FetchAllDiagramFiles();


        /// <summary>
        /// </summary>
        /// <param name="diagramId"></param>
        /// <returns></returns>
        DiagramItem FetchDiagram(int diagramId);

        /// <summary>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="designerItemId"></param>
        /// <returns></returns>
        T FetchDesignerItem<T>(int designerItemId) where T : DesignerItemBase;

        /// <summary>
        /// </summary>
        /// <param name="connectionId"></param>
        /// <returns></returns>
        Connection FetchConnection(int connectionId);
    }
}