﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace InfoChina
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class ChinaTelecomEntities : DbContext
    {
        public ChinaTelecomEntities()
            : base("name=ChinaTelecomEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<ConstructionPersonnelTable> ConstructionPersonnelTable { get; set; }
        public virtual DbSet<Login> Login { get; set; }
        public virtual DbSet<UserState> UserState { get; set; }
    }
}
