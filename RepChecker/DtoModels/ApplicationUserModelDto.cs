using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace RepChecker.DtoModels
{
    public class ApplicationUserModelDto
    {
        [Key]
        public string BattleTag { get; set; }
        public int Id { get; set; }
        public string LastUpdate { get; set; }

        public virtual ICollection<ReputationModelDto> UserReputations { get; set; }
    }
}
