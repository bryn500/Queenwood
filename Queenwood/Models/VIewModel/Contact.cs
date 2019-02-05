using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Queenwood.Models.ViewModel
{
    public class Contact
    {
        public const int MaxNameLength = 100;
        public const int MaxEmailLength = 255;
        public const int MaxSubjectLength = 60;
        public const int MaxMessageLength = 400;

        [Required]
        [StringLength(MaxNameLength)]
        public string Name { get; set; }

        [Required]
        [StringLength(MaxEmailLength)]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(MaxSubjectLength)]
        public string Subject { get; set; }

        [Required]
        [MaxLength(MaxMessageLength)]
        public string Message { get; set; }

        public bool Success { get; set; }
    }
}
