﻿using LeaveNotifierApplication.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LeaveNotifierApplication.Data
{
    public interface ILeaveNotifierRepository
    {
        // Basic DB Operations
        void Add<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;
        Task<bool> SaveAllAsync();

        // Leaves
        IEnumerable<Leave> GetAllLeaves();

        // Users
        IEnumerable<LeaveNotifierUser> GetAllUsers();
    }
}
