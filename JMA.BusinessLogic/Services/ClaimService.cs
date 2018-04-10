using System;
using System.Data.Entity.Infrastructure;
using System.Linq;
using JMA.DAL;
using JMA.Mapping.DTOs;
using AutoMapper;

namespace JMA.BusinessLogic.Services
{
    public sealed class ClaimService : IClaimService
    {
        /// <summary>
        /// Gets a known claimant by claim8 and pin
        /// </summary>
        /// <param name="claim8">The claim8</param>
        /// <param name="pin">The pin</param>
        public KnownClaimantDTO GetKnownClaimant(string claim8, string pin)
        {
            using (var context = new Entities())
            {
                var knownClaimant = context.KnownClaimants.AsNoTracking()
                    .Where(c => c.Claim8.Equals(claim8) && c.PIN.Equals(pin))
                    .SingleOrDefault();

                if (knownClaimant != null)
                {
                    var dto = Mapper.Map<KnownClaimant, KnownClaimantDTO>(knownClaimant);

                    return dto;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Saves an unknown claim
        /// </summary>
        /// <param name="unknownClaim">The unknown claim dto</param>
        public string SaveUnknownClaim(ClaimantDTO claimantDTO)
        {
            using (var context = new Entities())
            {
                var unknownClaimant = Mapper.Map<ClaimantDTO, UnknownClaimant>(claimantDTO);

                context.UnknownClaimants.Add(unknownClaimant);
                context.SaveChanges();
                context.Entry(unknownClaimant).Reload();

                return unknownClaimant.ClaimID;
            }
        }

        /// <summary>
        /// Updates a known claim
        /// </summary>
        /// <param name="claimantDTO">The known claim dto</param>
        /// <returns>The claim id</returns>
        public string UpdateKnownClaim(KnownClaimantDTO knownClaimantDTO)
        {
            using (var context = new Entities())
            {
                var knownClaimant = context.KnownClaimants
                    .Where(k => k.Claim8.Equals(knownClaimantDTO.Claim8))
                    .SingleOrDefault();

                Mapper.Map(knownClaimantDTO, knownClaimant);

                var entry = context.Entry<KnownClaimant>(knownClaimant);
                var manager = ((IObjectContextAdapter)context).ObjectContext.ObjectStateManager;
                var state = manager.GetObjectStateEntry(knownClaimant);
                var modifiedProperties = state.GetModifiedProperties();

                var addressChangeFields = new[] { "Addr1", "Addr2", "City", "State", "Zip", "FProv", "FZip", "FCountry" };
                var hasAddressChanged = modifiedProperties.Where(p => addressChangeFields.Contains(p)).Any();

                if (hasAddressChanged)
                {
                    knownClaimant.AddressChange = true;
                }

                context.SaveChanges();

                return knownClaimant.ClaimID;
            }
        }
    }
}