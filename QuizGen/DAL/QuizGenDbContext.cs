using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using QuizGen.Models;
using QuizGen.ValueObjects;
using QuizGen.Services;

namespace QuizGen.DAL
{
    internal class QuizGenDbContext : DbContext
    {
        public DbSet<Models.Question> Questions { get; set; }
        public DbSet<Quiz> Quizzes { get; set; }
        public DbSet<Answer> Answers { get; set; }

        private static readonly object _lock = new object();
        private static QuizGenDbContext _instance = null;


        public static QuizGenDbContext GetInstance()
        {
            lock(_lock)
            {
                if (_instance is null) _instance = new QuizGenDbContext();
                return _instance;
            }
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(@"Data Source=./mydb.db;");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Quiz>()
                .HasMany(x => x.Questions)
                .WithOne(x => x.Quiz)
                .HasForeignKey(x => x.QuizId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Models.Question>()
                .Property(x => x.Body)
                .HasConversion(x => Encryption.Encrypt(x.Value), x => Encryption.Decrypt(new QuestionBody(x)));

            modelBuilder.Entity<Models.Answer>()
                .Property(x => x.Value)
                .HasConversion(x => Encryption.Encrypt(x.Value), x => Encryption.Decrypt(new AnswerValue(x)));


            modelBuilder.Entity<Models.Question>()
                .HasMany(x => x.Answers)
                .WithOne(x => x.Question)
                .HasForeignKey(x => x.QuestionId);
        }
    }
}
