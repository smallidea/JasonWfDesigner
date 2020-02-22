// ******************************************************************
// ** Copyright：Copyright (c) 2020
// ** Project：JasonWfDesigner.Core
// ** Create Date：2020-02-21 14:31
// ** Created by：陈晓平
// ** Blog：http://smallidea.cnblogs.com
// ** Git：http://smallidea.github.com
// ** Email: smallidea@126.com
// ** Version：v 1.0
// ** Last Modified: 2020-02-21 15:54
// ** Desc：SelectableDesignerItemViewModelBase.cs
// ** Log: 每一个坑都源于精心的设计！每段垃圾代码的都是“故意”的！
// ******************************************************************

using System.Collections.Generic;
using System.Linq;
using JasonWfDesigner.Core.ViewModels;

namespace JasonWfDesigner.Core
{
    public interface ISelectItems
    {
        SimpleCommand SelectItemCommand { get; }
    }


    public abstract class SelectableDesignerItemViewModelBase : INPCBase, ISelectItems
    {
        private bool isSelected;

        public SelectableDesignerItemViewModelBase(int id, IDiagramViewModel parent)
        {
            Id = id;
            Parent = parent;
            Init();
        }

        public SelectableDesignerItemViewModelBase()
        {
            Init();
        }

        public List<SelectableDesignerItemViewModelBase> SelectedItems => Parent.SelectedItems;

        public IDiagramViewModel Parent { get; set; }
        public int Id { get; set; }

        public bool IsSelected
        {
            get => isSelected;
            set
            {
                if (isSelected != value)
                {
                    isSelected = value;
                    NotifyChanged("IsSelected");
                }
            }
        }

        public SimpleCommand SelectItemCommand { get; private set; }

        private void ExecuteSelectItemCommand(object param)
        {
            SelectItem((bool) param, !IsSelected);
        }

        private void SelectItem(bool newselect, bool select)
        {
            if (newselect)
                foreach (var designerItemViewModelBase in Parent.SelectedItems.ToList())
                    designerItemViewModelBase.isSelected = false;

            IsSelected = select;
        }

        private void Init()
        {
            SelectItemCommand = new SimpleCommand(ExecuteSelectItemCommand);
        }
    }
}