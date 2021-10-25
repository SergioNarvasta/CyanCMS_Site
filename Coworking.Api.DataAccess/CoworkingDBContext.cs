using Coworking.Api.DataAccess.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Coworking.Api.DataAccess
{
    public class CoworkingDBContext : DbContext, CoworkingDBContextContracts
    {
        public CoworkingDBContext()
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }
    }
}
