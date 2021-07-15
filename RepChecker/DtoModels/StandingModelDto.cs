using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace RepChecker.DtoModels
{
    public class StandingModelDto
    {
        [Key][DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StandingId { get; set; }
        public int Raw { get; set; }
        public int Max { get; set; }
        public int CurrentValue { get; set; }
        public int Tier { get; set; }
        public string Level { get; set; }

        public int ReputationId { get; set; }

        //[ForeignKey(nameof(ReputationId))]
        public virtual ReputationModelDto Reputation { get; set; }
    }
}
