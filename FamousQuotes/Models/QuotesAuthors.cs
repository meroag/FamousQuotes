﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace FamousQuotes.Models
{
    public partial class QuotesAuthors
    {
        public QuotesAuthors()
        {
            UsersQuzi = new HashSet<UsersQuzi>();
        }

        [Key]
        public long IdQuotesAuthors { get; set; }
        public long IdQuotes { get; set; }
        [Required]
        [StringLength(250)]
        public string AuthorName { get; set; }
        public bool IsCorrectAnswer { get; set; }

        [ForeignKey(nameof(IdQuotes))]
        [InverseProperty(nameof(Quotes.QuotesAuthors))]
        public virtual Quotes IdQuotesNavigation { get; set; }
        [InverseProperty("IdQuotesAuthorsNavigation")]
        public virtual ICollection<UsersQuzi> UsersQuzi { get; set; }
    }
}