using System;
using System.Data.Entity;
using Itis.ProxyDataPlatform.Base;
using Itis.ProxyDataPlatform.DatabaseContext;
using Itis.ProxyDataPlatform.Interfaces;

namespace Itis.ProxyDataPlatform.Converting
{
    public abstract class BaseConverter<TDal, TBll> : IConverter<TDal, TBll>
        where TDal : Entity
        where TBll : EntityBll
    {
        public TBll Convert(TDal entity)
        {
            var bll = (TBll)Converter.CreateInstance(entity);
            Convert(entity, bll);
            Converter.EntityConvert(entity, bll);

            return bll;
        }

        public TDal ReConvert(TBll entityBll, BaseContainer container)
        {
            var dal = (TDal)Converter.CreateInstance(entityBll);
            ReConvert(entityBll, dal, container);

            Converter.EntityConvert(entityBll, dal);
            //Назначаем идентификатор
//            entityBll.Id = dal.Id;
            /////////////////////////

            return dal;
        }

        public void ReConvertToDal(TBll entityBll, TDal to, BaseContainer container)
        {
            ReConvertToDal((object)entityBll, to, container);
        }

        public abstract void Convert(TDal from, TBll to);

        /// <summary>
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="container">Контекст для загрузки связанных сущностей. Если null то ссылки на связанные сущности будут равны null</param>
        public abstract void ReConvert(TBll from, TDal to, BaseContainer container);

        object IConverter.Convert(object entity)
        {
            return Convert((TDal) entity);
        }

        object IConverter.ReConvert(object entityBll, BaseContainer container)
        {
            return ReConvert((TBll) entityBll, container);
        }

        public void ReConvertToDal(object entityBll, object entity, BaseContainer container)
        {
            if (entity == null)
//                return ReConvert((TBll) entityBll, container);
                throw new ArgumentNullException("entity");

            ReConvert((TBll) entityBll, (TDal) entity, container);

            Converter.EntityConvert((IEntity) entityBll, (IEntity) entity);
        }
    }
}
