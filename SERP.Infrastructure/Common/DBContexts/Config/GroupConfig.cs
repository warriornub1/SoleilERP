using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SERP.Domain.Common.Constants;
using SERP.Domain.Finance.Groups;

namespace SERP.Infrastructure.Common.DBContexts.Config
{
    public class GroupConfig : IEntityTypeConfiguration<Group>
    {
        public void Configure(EntityTypeBuilder<Group> builder)
        {
            var validGroupTypes= new[]
            {
                DomainConstant.Group.GroupType.Company,
                DomainConstant.Group.GroupType.NaturalAccount,
                DomainConstant.Group.GroupType.CostCenter,
                DomainConstant.Group.GroupType.RevenueCenter
            };

            var validStatusTypes = new[]
            {
                DomainConstant.StatusFlag.Enabled,
                DomainConstant.StatusFlag.Disabled
            };


            builder.HasIndex(x => new { x.group_code, x.group_type } ).IsUnique();

            builder.HasCheckConstraint("CK_Group_GroupType",
                $"group_type IN ({string.Join(", ", validGroupTypes.Select(t => $"'{t}'"))})");

            builder.HasOne<Group>()
                .WithMany()
                .HasForeignKey(g => g.parent_group_id)
                .OnDelete(DeleteBehavior.Restrict); // Parent cannot be deleted if children exist


            builder.Property(x => x.status_flag)
                .HasDefaultValue(DomainConstant.StatusFlag.Enabled);

            builder.HasCheckConstraint("CK_Group_StatusFlag",
                $"status_flag IN ({string.Join(", ", validStatusTypes.Select(t => $"'{t}'"))})");

        }
    }
}
