using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Queenwood.Models
{
    public class Contact
    {
        public const int MaxFirstNameLength = 30;
        public const int MaxLastNameLength = 30;
        public const int MaxEmailLength = 255;
        public const int MaxSubjectLength = 60;
        public const int MaxMessageLength = 400;

        [Required]
        [StringLength(MaxFirstNameLength)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(MaxLastNameLength)]
        public string LastName { get; set; }

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
