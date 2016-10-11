using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using Itis.ProxyDataPlatform.Base;
using Itis.ProxyDataPlatform.Converting;
using Itis.ProxyDataPlatform.DatabaseContext;
using Itis.ProxyDataPlatform.Linq;
using Dapper;

namespace Itis.ProxyDataPlatform
{
    public abstract class ProxyApi : IDisposable
    {
        private  BaseContainer _container;
        private Assembly _converterAssembly;

        protected ProxyApi()
        {
        }

        protected ProxyApi(Assembly converterAssembly)
        {
            _converterAssembly = converterAssembly;
            Converter.AddDataModelAssembly(_converterAssembly);
        }

        internal BaseContainer Container
        {
            get { return _container ?? (_container = GetContainer()); }
        }

        private BaseContainer GetContainer()
        {
            var container = CreateContainer();

            if (_converterAssembly == null)
            {
                _converterAssembly = container.GetType().Assembly;
                Converter.AddDataModelAssembly(_converterAssembly);
            }

            return container;
        }

        protected abstract BaseContainer CreateContainer();

        public IMapQueryable<T> GetData<T>() where T : EntityBll
        {
            return new SunoQueryable<T>(Container.Set(Converter.BllTypeMap(typeof(T))));
        }

        /// <summary>
        /// Выполняет указанный SQL-запрос, заполняя результат в коллекцию типизированных объектов (по имени соответственных свойств)
        /// </summary>
        /// <typeparam name="T">Тип получаемых объектов</typeparam>
        /// <param name="query">Текст запроса</param>
        /// <returns>Коллекция типизированных объектов, имена свойств которых должны соответствовать именам полей, по которым осуществляется выборка.</returns>
        public System.Collections.Generic.IEnumerable<T> Query<T>(string query)
        {
            return Container.Database.Connection.Query<T>(query);
        }

        /// <summary>
        /// Выполняет указанный SQL-запрос, заполняя результат в коллекцию типизированных объектов (по имени соответственных свойств)
        /// </summary>
        /// <typeparam name="T">Тип получаемых объектов</typeparam>
        /// <param name="query">Текст запроса</param>
        /// <param name="param">Параметры запроса. Передаются в виде анонимного объекта, имена полей которого должны совпадать с именами параметров в запросе.</param>
        /// <returns>Коллекция типизированных объектов, имена свойств которых должны соответствовать именам полей, по которым осуществляется выборка.</returns>
        public System.Collections.Generic.IEnumerable<T> Query<T>(string query, object param)
        {
            return Container.Database.Connection.Query<T>(query, param);
        }

        public IMapQueryable GetData(Type entityBllType)
        {
            var ctor = typeof (SunoQueryable<>).MakeGenericType(new[] {entityBllType}).GetConstructor(new []{typeof(IQueryable)});

            if (ctor == null)
                throw new InvalidOperationException("Не удалось найти конструктор SunoQueryable");

            return (IMapQueryable)ctor.Invoke(new[] { Container.Set(Converter.BllTypeMap(entityBllType)) });
        }

        public void Add<T>(T entityBll) where T : EntityBll
        {
            if (entityBll.Id == Guid.Empty)
                entityBll.Id = Guid.NewGuid();

            var entity = Converter.ReConvert(entityBll, Container);
            Container.Set(entity.GetType()).Add(entity);
        }

        [Obsolete]
        public void Change<T>(T entityBll) where T : EntityBll
        {
            var entity = (Entity)Converter.ReConvert(entityBll, Container);

            var dbSet = Container.Set(entity.GetType());
            dbSet.Local.OfType<object>().ToList().ForEach(e =>
            {
                if (((Entity)e).Id == entity.Id)
                    Container.Entry(e).State = EntityState.Detached;
            });
            Container.Entry(entity).State = EntityState.Modified;
        }

        [Obsolete]
        public void Change2<T>(T entityBll) where T : EntityBll
        {
            var entity = (Entity)Converter.ReConvert(entityBll, Container);
            var oldEntity = Container.Set(entity.GetType()).Find(entity.Id);

            Container.Entry(oldEntity).State = EntityState.Detached;
            Container.Entry(entity).State = EntityState.Modified;
        }

        public void Change3<T>(T entityBll) where T : EntityBll
        {
            var oldEntity = Container.Set(Converter.BllTypeMap(entityBll)).Find(entityBll.Id);
            Converter.ReConvertToDal(entityBll, oldEntity, Container);
        }

        public void Remove<T>(T entityBll) where T : EntityBll
        {
            var entityType = Converter.BllTypeMap(typeof(T));
            var entity = Container.Set(entityType).Find(entityBll.Id);
            Container.Set(entityType).Remove(entity);
        }

        public void Remove2(EntityBll entityBll)
        {
            if (entityBll == null) 
                throw new ArgumentNullException("entityBll");

            var entityType = Converter.BllTypeMap(entityBll.GetType());
            var entity = Container.Set(entityType).Find(entityBll.Id);
            Container.Set(entityType).Remove(entity);
        }

        public void SaveChanges()
        {
            Container.SaveChanges();
        }

        public void Dispose()
        {
            Container.Dispose();
        }

        public SunoDbContextTransaction BeginTransaction(IsolationLevel level)
        {
            return new SunoDbContextTransaction(Container.Database.BeginTransaction(level));
        }
    }
}