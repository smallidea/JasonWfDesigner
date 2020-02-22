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
// ** Desc：LockList.cs
// ** Log: 每一个坑都源于精心的设计！每段垃圾代码的都是“故意”的！
// ******************************************************************

using System;
using System.Collections.Generic;
using System.Linq;

namespace JasonWfDesigner.Common
{
    /// <summary>
    ///     锁定列表
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal class LockList<T>
    {
        // 在静态锁面前，线程依旧要排队，虽然不是一个实例，但是锁是唯一的，线程只认锁，所以线程并没有并发！
        // 因为如果在lock代码段中改变obj的值，其它线程就畅通无阻了，因为互斥锁对象变了，object.referenceequals必然返回false。
        // http://blog.csdn.net/yongwuxin/article/details/41222639
        private static readonly object _locker = new object();

        //
        private List<T> _list;

        public LockList()
        {
            _list = new List<T>();
        }

        /// <summary>
        ///     插入数据
        /// </summary>
        /// <param name="item">成员</param>
        /// <param name="index">指定插入的位置</param>
        public void Insert(T item, int index = -1)
        {
            lock (_locker)
            {
                if (index > 0 && index < _list.Count)
                    _list.Insert(index, item);
                else
                    _list.Add(item);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="list"></param>
        public void Concat(IEnumerable<T> list)
        {
            lock (_locker)
            {
                _list = _list.Concat(list).ToList();
            }
        }

        /// <summary>
        ///     找到并出队
        ///     <remarks>先进先出</remarks>
        /// </summary>
        /// <param name="whereExpression"></param>
        /// <returns></returns>
        public T FindAndDequeue(Func<T, bool> whereExpression)
        {
            if (_list == null) return default;

            lock (_locker)
            {
                var single = _list.FirstOrDefault(whereExpression);

                if (single == null)
                    return default;
                _list.Remove(single);
                return single;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="keySelector"></param>
        /// <param name="isAsc"></param>
        public void OrderBy(Func<T, IComparable> keySelector, bool isAsc = true)
        {
            lock (_locker)
            {
                if (isAsc)
                    _list = _list.OrderBy(keySelector).ToList();
                else
                    _list = _list.OrderByDescending(keySelector).ToList();
            }
        }


        /// <summary>
        /// </summary>
        /// <param name="orders"></param>
        public void OrderBy(List<KeyValuePair<Func<T, IComparable>, bool>> orders)
        {
            lock (_locker)
            {
                if (orders.Any())
                    foreach (var keyValuePair in orders)
                        if (keyValuePair.Value)
                            _list = _list.OrderBy(keyValuePair.Key).ToList();
                        else
                            _list = _list.OrderByDescending(keyValuePair.Key).ToList();
            }
        }

        /// <summary>
        ///     移除一个元素
        /// </summary>
        /// <param name="item"></param>
        public void Remove(T item)
        {
            lock (_locker)
            {
                _list.Remove(item);
            }
        }

        /// <summary>
        ///     添加一个元素
        /// </summary>
        /// <param name="whereExpression"></param>
        /// <returns></returns>
        public bool Any(Func<T, bool> whereExpression)
        {
            lock (_locker)
            {
                return _list != null && _list.Any(whereExpression);
            }
        }

/*        public void OrderByDesc<TS>(Expression<Func<T, TS>> orderExpression)
        {
            lock (_locker)
            {
                if (orderExpression != null)
                    _list.OrderByDescending<T, TS>(orderExpression)
            }
        }*/

        /// <summary>
        ///     是否包含成员
        /// </summary>
        /// <returns></returns>
        public bool Any()
        {
            lock (_locker)
            {
                return _list.Any();
            }
        }

        /// <summary>
        ///     获取所有成员
        /// </summary>
        /// <returns></returns>
        public List<T> GetAll(bool isClear = false)
        {
            lock (_locker)
            {
                var relist = _list.ToList();
                if (isClear)
                    _list.Clear();
                return relist;
            }
        }

        /// <summary>
        ///     根据指定的条件查找数据
        /// </summary>
        /// <param name="whereExpression"></param>
        /// <returns></returns>
        public IEnumerable<T> Find(Func<T, bool> whereExpression)
        {
            lock (_locker)
            {
                return _list.Where(whereExpression);
            }
        }

        /// <summary>
        ///     序号
        /// </summary>
        /// <param name="whereExpression"></param>
        /// <returns></returns>
        public int GetIndex(Func<T, bool> whereExpression)
        {
            lock (_locker)
            {
                var single = _list.FirstOrDefault(whereExpression);
                if (single == null) return -1;
                return _list.IndexOf(single);
            }
        }

        public int Count()
        {
            lock (_locker)
            {
                return _list?.Count ?? 0;
            }
        }

        /// <summary>
        ///     序号
        /// </summary>
        /// <param name="whereExpression"></param>
        /// <returns></returns>
        public int GetLastIndex(Func<T, bool> whereExpression)
        {
            lock (_locker)
            {
                var single = _list.LastOrDefault(whereExpression);
                if (single == null) return -1;
                return _list.LastIndexOf(single);
            }
        }

        /// <summary>
        ///     清空
        /// </summary>
        public void Clear()
        {
            lock (_locker)
            {
                _list.Clear();
            }
        }
    }
}