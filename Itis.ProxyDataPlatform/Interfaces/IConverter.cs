// DGaripov
// Гарипов Дамир Сагдиевич
// 2015.09.29 10:10

using System.Data.Entity;
using Itis.ProxyDataPlatform.Base;
using Itis.ProxyDataPlatform.DatabaseContext;

namespace Itis.ProxyDataPlatform.Interfaces
{
    internal interface IConverter
    {
        object Convert(object entity);
        object ReConvert(object entityBll, BaseContainer container);
        void ReConvertToDal(object entityBll, object entity, BaseContainer container);
    }

    internal interface IConverter<TDal, TBll> : IConverter
        where TDal: Entity 
        where TBll : EntityBll
    {
        TBll Convert(TDal entity);
        TDal ReConvert(TBll entityBll, BaseContainer container);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityBll"></param>
        /// <param name="entity"></param>
        /// <param name="container">Контекст для загрузки связанных сущностей. Если null то ссылки на связанные сущности будут равны null</param>
        void ReConvertToDal(TBll entityBll, TDal entity, BaseContainer container);
    }
}