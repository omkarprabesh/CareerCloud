﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CareerCloud.Pocos
{
    [Table("Security_Logins_Roles")]
    public class SecurityLoginsRolePoco: IPoco
    {
        [Key]
        public Guid Id{get;set;}
        public Guid Login{get;set;}
        public Guid Role{get;set;}
        [Column("Time_Stamp")]
        [Timestamp]
        public byte[] TimeStamp{get;set;}
        [ForeignKey("Login")]
        public virtual SecurityLoginPoco SecurityLogin { get; set; }
        [ForeignKey("Role")]
        public virtual SecurityRolePoco SecurityRole { get; set; }
    }
}
