using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SERP.Domain.Common.Constants;
using SERP.Domain.Finance.Groups;
using SERP.Domain.Finance.NaturalAccounts;

namespace SERP.Infrastructure.Common.DBContexts.Config
{
    public class NaturalAccountConfig : IEntityTypeConfiguration<NaturalAccount>
    {
        public void Configure(EntityTypeBuilder<NaturalAccount> builder)
        {
            var validNaturalAccountTypes = new[]
            {
                DomainConstant.NaturalAccount.NaturalAccountType.Asset,
                DomainConstant.NaturalAccount.NaturalAccountType.Liability,
                DomainConstant.NaturalAccount.NaturalAccountType.Shareholders_Equity,
                DomainConstant.NaturalAccount.NaturalAccountType.PAndL
            };

            builder.HasIndex(x => x.natural_account_code).IsUnique();

            builder.HasCheckConstraint("CK_NaturalAccount_NaturalAccountType",
                $"natural_account_type IN ({string.Join(", ", validNaturalAccountTypes.Select(t => $"'{t}'"))})");


            builder.HasOne<Group>() // Specifies that Group has a reference to NaturalAccount (implicitly through GroupId).
                .WithMany()
                .HasForeignKey(od => od.parent_group_id)
                .OnDelete(DeleteBehavior.Restrict); // Parent cannot be deleted if children exist

        }
    }
}
