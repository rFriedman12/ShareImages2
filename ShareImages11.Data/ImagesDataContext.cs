using Microsoft.EntityFrameworkCore;
using System;


namespace ShareImages11.Data
{
    public class ImagesDataContext :DbContext
    {
        private string _connString;

        public ImagesDataContext(string connString)
        {
            _connString = connString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connString);
        }

        public DbSet<Image> Images { get; set; }
    }
}
