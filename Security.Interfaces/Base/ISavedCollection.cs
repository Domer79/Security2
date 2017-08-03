namespace Security.Interfaces.Base
{
    public interface ISavedCollection
    {
        /// <summary>
        /// Производит сохранение всех изменений в базу данных
        /// </summary>
        /// <returns></returns>
        int SaveChanges();
    }
}
