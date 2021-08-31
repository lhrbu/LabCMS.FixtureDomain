using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace LabCMS.FixtureDomain.Shared.Models
{
    public record Fixture(

        string ProjectShortName,
        TestField TestField,
        string SetIndex)
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int No { get; set; }

        [NotMapped]
        public string Description => $"{ProjectShortName}-{this.TestField.ToString().First()}-{SetIndex}";
        
        public FixtureStatus Status { get; set; }
        public string? StorageInformation { get; set; }
        public int ShelfNo { get; set; }
        public int FloorNo { get; set; }
        [NotMapped]
        public string LocationNo => $"{ShelfNo}-{FloorNo}";
        public string? AssetNo { get; set; }
        public string? Comment { get; set; }

        public bool CanCheckout() =>
            Status != FixtureStatus.CheckedOut &&
            Status != FixtureStatus.InternalCheckoutApply &&
            Status != FixtureStatus.ExternalCheckoutApply &&
            Status != FixtureStatus.ExternalCheckoutApprove;
    }
}
