using SERP.Domain.Common;
using SERP.Domain.Common.Constants;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SERP.Domain.Transactions.Containers
{
    public class Container : BaseModel
    {
        /// <summary>
        /// Container may be duplicated because different shipment may use the same container no.
        /// </summary>
        [StringLength(DomainDBLength.Containers.ContainerNo)]
        public string container_no { get; set; }
        /// <summary>
        /// Bay No.
        /// </summary>
        [StringLength(DomainDBLength.Containers.BayNo)]
        public string? bay_no { get; set; }

        public DateTime? detention_date { get; set; }
        /// <summary>
        ///  no of packages
        /// </summary>
        public int? no_of_packages { get; set; }
        /// <summary>
        /// no of packages unloaded
        /// </summary>
        public int? no_of_packages_unloaded { get; set; }
        /// <summary>
        /// Status Flag
        /// 0: Incoming
        /// 1: Received
        /// 2: Unloading
        /// 3: Unloaded
        /// 4: Completed
        /// 9: Unverified
        /// </summary>
        [StringLength(DomainDBLength.Containers.StatusFlag)]
        public string status_flag { get; set; } = DomainConstant.Containers.StatusFlag.Incoming;
        /// <summary>
        /// Loose or Full Cargo
        /// L: Loose
        /// F: Full
        /// </summary>
        [StringLength(DomainDBLength.Containers.ShipmentType)]
        public string? shipment_type { get; set; }
        /// <summary>
        /// Describing the type of container.
        /// FCL40 means Full Container of 40 Ton
        /// </summary>
        [StringLength(DomainDBLength.Containers.ContainerType)]
        public string? container_type { get; set; }
        /// <summary>
        /// Weight
        /// </summary>
        [Column(TypeName = "decimal(8,2)")]
        public decimal? weight { get; set; }
        /// <summary>
        /// unload start on
        /// Timestamp of container unloading start
        /// </summary>
        public DateTime? unload_start_on { get; set; }
        /// <summary>
        /// unload end on
        /// Timestamp of container unloading end
        /// </summary>
        public DateTime? unload_end_on { get; set; }
        /// <summary>
        /// unloaded by
        /// User ID of user that unload the container.
        /// </summary>
        [StringLength(DomainDBLength.Containers.UserId)]
        public string? unloaded_by { get; set; }
        /// <summary>
        /// unload remark
        /// </summary>
        [StringLength(DomainDBLength.Containers.UnloadRemark)]
        public string? unload_remark { get; set; }
        /// <summary>
        /// received on
        /// Timestamp of container was received
        /// </summary>
        public DateTime? received_on { get; set; }
        /// <summary>
        /// received by
        /// User ID of user that receive the container.
        /// </summary>
        [StringLength(DomainDBLength.Containers.UserId)]
        public string? received_by { get; set; }
        /// <summary>
        /// released on
        /// Timestamp of container was released
        /// </summary>
        public DateTime? released_on { get; set; }
        /// <summary>
        /// released by
        /// User ID of user that release the container.
        /// </summary>
        [StringLength(DomainDBLength.Containers.UserId)]
        public string? released_by { get; set; }
        /// <summary>
        /// Seal No
        /// </summary>
        [StringLength(DomainDBLength.Containers.SealNo)]
        public string? seal_no { get; set; }
    }
}
