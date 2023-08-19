﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartHome.TasksManager.Core.Entities;
using SmartHome.TasksManager.Core.Interfaces;

namespace SmartHome.TasksManager.Infrastructure.Data;

/// <summary>
///   A simple repository implementation for EF Core
///   If you don't want changes to be saved immediately, add a SaveChanges method to the interface
///   and remove the calls to _dbContext.SaveChanges from the Add/Update/Delete methods
/// </summary>
public class EfRepository : IRepository
{
  private readonly SmartHomeDbContext _dbContext;

  public EfRepository(SmartHomeDbContext dbContext)
  {
    _dbContext = dbContext;
  }

  public T GetById<T>(int id) where T : BaseEntity
  {
    return _dbContext.Set<T>().SingleOrDefault(e => e.Id == id);
  }

  public List<T> List<T>() where T : BaseEntity
  {
    return _dbContext.Set<T>().ToList();
    
  }

  public T Add<T>(T entity) where T : BaseEntity
  {
    _dbContext.Set<T>().Add(entity);
    _dbContext.SaveChanges();
    
    return entity;
  }

  public void Delete<T>(T entity) where T : BaseEntity
  {
    _dbContext.Set<T>().Remove(entity);
    _dbContext.SaveChanges();
  }

  public void Update<T>(T entity) where T : BaseEntity
  {
    _dbContext.Entry(entity).State = EntityState.Modified;
    _dbContext.SaveChanges();
  }
}