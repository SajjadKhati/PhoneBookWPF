using DataAccess.EntityModule.Class.Entity;
using DataAccess.EntityModule.Interface;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.EntityModule.Class;

namespace Model.PhoneBookModule.Class
{
    internal class ContactPhoneBookToDataAccess : IDisposable
    {

        private readonly IRepositoryAggregate<PersonEntity> _repository;



        internal ContactPhoneBookToDataAccess()
        {
            /// نیازی به دانستن کلاس PhoneBook به شیِ مورد استفاده در متغییر _repository نیست .هر چند فرق خاصی هم نمیکند .
            /// بنابراین نیازی به استفاده از "تزریق وابستگی" نیست .
            this._repository = new Repository<PersonEntity>(new PhoneBookDbContext());
        }




        internal async Task InitializeDatabaseAsync(bool force = false)
        {
            await this._repository.InitializeDatabaseAsync(force);
        }


        internal async Task<IList<TQueryEntity>> GetAllAsync<TQueryEntity>() where TQueryEntity : class
        {
            IEnumerable<TQueryEntity> allEntities = await this._repository.GetAllByEagerLoadingModeAsync<TQueryEntity>();
            if(allEntities == null) 
                return null;

            IList<TQueryEntity> allEntityList = await Task.Factory.StartNew<IList<TQueryEntity>>(() => allEntities.ToList());
            if (allEntityList == null || allEntityList.Any() == false)
                return null;

            return allEntityList;
        }


        internal IList<TQueryEntity> GetAllByLazyLoadingMode<TQueryEntity>() where TQueryEntity : class
        {
            return this._repository.GetAllByLazyLoadingMode<TQueryEntity>().ToList();
        }


        internal TQueryEntity FindByLazyLoadingMode<TQueryEntity>(int id) where TQueryEntity : class
        {
            return this._repository.FindByLazyLoadingMode<TQueryEntity>(id);
        }


        internal void AddPersonEntity(PersonEntity personEntity)
        {
            this._repository.Add(personEntity);
        }


        internal void EditPersonEntity(PersonEntity personEntity)
        {
            this._repository.Edit(personEntity.Id, personEntity);
        }


        internal void Remove<TRemoveEntity>(TRemoveEntity personEntity) where TRemoveEntity : class
        {
            this._repository.Remove(personEntity);
        }


        internal bool SaveChanges()
        {
            return this._repository.SaveChanges() > 0;
        }


        public void Dispose()
        {
            this._repository.Dispose();
        }


    }
}
