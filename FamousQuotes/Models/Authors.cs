﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace FamousQuotes.Models
{
    public partial class Authors
    {
        [Key]
        public long IdAuthors { get; set; }
        [Required]
        [StringLength(250)]
        public string AuthorName { get; set; }
    }
}