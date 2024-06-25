using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFlex.Model;

namespace CircleByEdge
{
    /// <summary>
	/// T - ссылочный объект класса Node или Variable, который мы оборачиваем
	/// и синхронизируем по коду типа int с владельцем объекта документа,
	/// который имеет тип TFlex.Model.Model2D.ExternalObject
	/// </summary>
	public class ReferenceHolder<T>
        where T : ModelObject
    {
        //содержимое Holder-а для хранения объекта в режиме modeKeepingObject (автономном от owner-а), изначально пусто
        private T _object;

        //ссылка на owner-а звезды, которой принадлежит данный Holder, нужна для режима modeKeepingReference (Holder подключен к owner-у)
        private CreateSplineCommandGeneral _star;

        //код для связи с Owner-ом (владеющим данной звездой External Object-ом)
        private int _code;

       
        

        

        /// <summary>
        /// Хранит обёрнутый объект либо автономно по ссылке, либо используя механизм ссылок родительского объекта
        /// </summary>
        public T Value
        {
            get
            {
                //if (_star.Owner2D != null)
                //    return _star.Owner2D.GetReference(_code) as T;

                return _object;
            }

            set
            {
                _object = value;
            }

        }

        ///// <summary>
        ///// устанавливает Holder-а, возвращает true, если успешно
        ///// </summary>
        //public void SetReference(T source)
        //{
        //    if (_star.Owner2D != null)
        //    {
        //        _star.Owner2D.SetReference(_code, source);
        //    }
        //    else
        //        _object = source;
        //}

        /// <summary>
        /// сбрасывает Holder-а (т.к. используется лишь с автономными звёздами, Owner2D можно не сбрасывать - он null)
        /// </summary>
        public void ReleaseReference()
        {
            _object = null;
        }

    }//class ends
}
