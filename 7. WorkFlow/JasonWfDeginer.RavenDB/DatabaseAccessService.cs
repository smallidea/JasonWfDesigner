#region Header Text

// /******************************************************************
// ** Copyright：广州宁基智能系统有限公司 Copyright (c) 2017
// ** Project：JasonWfDesigner.RavenDB
// ** Create Date：2018-06-03 9:57
// ** Created by：陈晓平
// ** Blog：http://smallidea.cnglogs.com
// ** Version：v 1.0
// ** Last Modified: 2018-06-07 10:31
// ** Desc： DatabaseAccessService.cs
// ******************************************************************/

#endregion

using System.Collections.Generic;
using System.Linq;
using JasonWfDesigner.Common;
using Raven.Client;
using Raven.Client.Embedded;

namespace JasonWfDesigner.RavenDB
{
    /// <summary>
    ///     I decided to use RavenDB instead of SQL, to save people having to have SQL Server, and also
    ///     it just takes less time to do with Raven. This is ALL the CRUD code. Simple no?
    ///     Thing is the IDatabaseAccessService and the items it persists could easily be applied to helper methods that
    ///     use StoredProcedures or ADO code, the data being stored would be exactly the same. You would just need to store
    ///     the individual property values in tables rather than store objects.
    /// </summary>
    public class DatabaseAccessService : IDatabaseAccessService
    {
        private readonly EmbeddableDocumentStore _documentStore = null;

        public DatabaseAccessService()
        {
            _documentStore = new EmbeddableDocumentStore
            {
                DataDirectory = "Data",
            };
            //_documentStore.Conventions.AllowQueriesOnId = true;
            _documentStore.Initialize();

        }

        public void DeleteConnection(int connectionId)
        {
            using (IDocumentSession session = _documentStore.OpenSession())
            {
                IEnumerable<Connection> conns = session.Query<Connection>().Where(x => x.Id == connectionId);
                foreach (var conn in conns)
                {
                    session.Delete<Connection>(conn);
                }
                session.SaveChanges();
            }
        }

        /*  public void DeletePersistDesignerItem(int persistDesignerId)
          {
              using (IDocumentSession session = documentStore.OpenSession())
              {
                  IEnumerable<PersistDesignerItem> persistItems = session.Query<PersistDesignerItem>().Where(x => x.Id == persistDesignerId);
                  foreach (var persistItem in persistItems)
                  {
                      session.Delete<PersistDesignerItem>(persistItem);
                  }
                  session.SaveChanges();
              }
          }

          public void DeleteSettingDesignerItem(int settingsDesignerItemId)
          {
              using (IDocumentSession session = documentStore.OpenSession())
              {
                  IEnumerable<SettingsDesignerItem> settingItems = session.Query<SettingsDesignerItem>().Where(x => x.Id == settingsDesignerItemId);
                  foreach (var settingItem in settingItems)
                  {
                      session.Delete<SettingsDesignerItem>(settingItem);
                  }
                  session.SaveChanges();
              }
          }*/

        public void DeleteDesignerItem<T>(int settingsDesignerItemId) where T : DesignerItemBase
        {
            using (IDocumentSession session = _documentStore.OpenSession())
            {
                IEnumerable<T> settingItems = session.Query<T>().Where(x => x.Id == settingsDesignerItemId);
                foreach (var settingItem in settingItems)
                {
                    session.Delete<T>(settingItem);
                }
                session.SaveChanges();
            }
        }


        public int SaveDiagram(DiagramItem diagram)
        {
            return SaveItem(diagram);
        }

        public int SaveDiagramView(Diagram4Serialize diagram)
        {
            throw new System.NotImplementedException();
        }

        public Diagram4Serialize LoadDiagramView(int id)
        {
            throw new System.NotImplementedException();
        }

        public Diagram4Serialize LoadDiagramView(string fileName)
        {
            throw new System.NotImplementedException();
        }

        public int SaveDesignerItem(DesignerItemBase designerItem)
        {
            return SaveItem(designerItem);
        }

        public int SaveConnection(Connection connectionToSave)
        {
            return SaveItem(connectionToSave);
        }

        public IEnumerable<DiagramItem> FetchAllDiagram()
        {
            using (IDocumentSession session = _documentStore.OpenSession())
            {
                return session.Query<DiagramItem>().ToList();
            }
        }

        public IEnumerable<string> FetchAllDiagramFiles()
        {
            throw new System.NotImplementedException();
        }

        public DiagramItem FetchDiagram(int diagramId)
        {
            using (IDocumentSession session = _documentStore.OpenSession())
            {
                //   _documentStore.Conventions.AllowQueriesOnId = true;
                return session.Query<DiagramItem>().Single(x => x.Id == diagramId);
                //     return session.Load<DiagramItem>("DiagramItems/" + diagramId);
            }
        }

        public T FetchDesignerItem<T>(int designerItemId)
            where T : DesignerItemBase
        {
            using (IDocumentSession session = _documentStore.OpenSession())
            {
                return session.Query<T>().Single(x => x.Id == designerItemId);
            }
        }

        /*public PersistDesignerItem FetchPersistDesignerItem(int settingsDesignerItemId)
        {
            using (IDocumentSession session = documentStore.OpenSession())
            {
                return session.Query<PersistDesignerItem>().Single(x => x.Id == settingsDesignerItemId);
            }
        }

        public SettingsDesignerItem FetchSettingsDesignerItem(int settingsDesignerItemId)
        {
            using (IDocumentSession session = documentStore.OpenSession())
            {
                return session.Query<SettingsDesignerItem>().Single(x => x.Id == settingsDesignerItemId);
            }
        }*/

        public Connection FetchConnection(int connectionId)
        {
            using (IDocumentSession session = _documentStore.OpenSession())
            {
                return session.Query<Connection>().Single(x => x.Id == connectionId);
            }
        }

        /*        public int SavePersistDesignerItem(PersistDesignerItem persistDesignerItemToSave)
                {
                    return SaveItem(persistDesignerItemToSave);
                }

                public int SaveSettingDesignerItem(SettingsDesignerItem settingsDesignerItemToSave)
                {
                    return SaveItem(settingsDesignerItemToSave);
                }*/

        public int SavenodeDesignerItem(NodeDesignerItem nodeDesignerItem)
        {
            return SaveItem(nodeDesignerItem);
        }

        private int SaveItem(PersistableItemBase item)
        {
            using (IDocumentSession session = _documentStore.OpenSession())
            {
                session.Store(item);
                session.SaveChanges();
            }
            return item.Id;
        }
    }
}