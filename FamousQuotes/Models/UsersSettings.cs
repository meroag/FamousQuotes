﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace FamousQuotes.Models
{
    public partial class UsersSettings
    {
        [Key]
        public long IdUsersSettings { get; set; }
        public long IdUsers { get; set; }
        [Required]
        public bool? SimpleQuizMode { get; set; }

        [ForeignKey(nameof(IdUsers))]
        [InverseProperty(nameof(Users.UsersSettings))]
        public virtual Users IdUsersNavigation { get; set; }
    }
}