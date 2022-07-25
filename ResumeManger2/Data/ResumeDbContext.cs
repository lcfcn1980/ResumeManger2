using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ResumeManger2.Models;

namespace ResumeManger2.Data
{
    public class ResumeDbContext : DbContext
    {
        public ResumeDbContext (DbContextOptions<ResumeDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Applicant> Applicants { get; set; }
        public virtual DbSet<Experience> Experiences { get; set; }
    }
}
