﻿using DAL.Interfaces.DTO;
using DAL.Interfaces.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using BLL.Interface.Exceptions;

namespace BLL.Services
{
    public abstract class BaseService<BllType,DalType> 
        where DalType : IEntity
    {
        protected abstract BllType ToBll(DalType dal);
        protected abstract DalType ToDal(BllType bll);
        protected abstract void Check(BllType entity);

        protected readonly IUnitOfWork unitOfWork;
        protected readonly IRepository<DalType> repository;

        public BaseService(IUnitOfWork unitOfWork, IRepository<DalType> repository)
        {
            this.unitOfWork = unitOfWork;
            this.repository = repository;
        }

        public BllType Get(int id)
            => ToBll(repository.GetById(id));

        public IEnumerable<BllType> GetAllEntities()
            => repository.GetAll().Select(ToBll);

        public void Create(BllType entity)
        {
            Check(entity);
            try
            {
                repository.Create(ToDal(entity));
                unitOfWork.Commit();
            }
            catch (Exception ex)
            {
                throw new ServiceException(ex.Message, ex);
            }
        }

        public void Delete(BllType entity)
        {
            Check(entity);
            try
            {
                repository.Delete(ToDal(entity));
                unitOfWork.Commit();
            }
            catch (Exception ex)
            {
                throw new ServiceException(ex.Message, ex);
            }
        }

        public void Update(BllType entity)
        {
            Check(entity);
            try
            {
                repository.Update(ToDal(entity));
                unitOfWork.Commit();
            }
            catch(Exception ex)
            {
                throw new ServiceException(ex.Message, ex);
            }
        }
       
    }
}
