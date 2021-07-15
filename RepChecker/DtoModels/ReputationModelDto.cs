using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RepChecker.DtoModels
{
    public class ReputationModelDto
    {
        [Key]
        public int ReputationId { get; set; }
        public string ReputationName { get; set; }
        public string Character { get; set; }
        public string Realm { get; set; }
        public string FactionHref { get; set; }
        
        public StandingModelDto Standing { get; set; }
        public string BattleTag { get; set; }

        //[ForeignKey(nameof(BattleTag))]
        public virtual ApplicationUserModelDto ApplicationUser { get; set; }
    }
}
