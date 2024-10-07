using ExpHub.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpHub.Data
{
    internal class ApplicationDbContext:DbContext
    {
        public DbSet<ClientFile> ClientFiles { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // SQL Server'a bağlanmak için connection string yazılır
            optionsBuilder.UseSqlServer("Server=ACER;Database=ExceptionSolutionDb;Trusted_Connection=True;TrustServerCertificate=True");
        }
    }
}
