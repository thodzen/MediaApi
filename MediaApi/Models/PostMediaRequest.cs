﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MediaApi.Models
{
    public class PostMediaRequest : IValidatableObject
    {
        [Required]
        [StringLength(150)]
        public string Title { get; set; }
        [Required]
        public string RecommendedBy { get; set; }
        [Required]
        public string Kind { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if(Title.ToLower() == "tiger king")
            {
                yield return new ValidationResult("That show stinks!", new string[] { "Kind" });
            }
            if(Kind != "show" && Kind != "game")
            {
                yield return new ValidationResult("Must be a show or game", new string[] { "Kind" });
            }
        }
    }
}
