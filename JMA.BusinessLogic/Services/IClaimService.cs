using System;
using JMA.Mapping.DTOs;

namespace JMA.BusinessLogic.Services
{
    public interface IClaimService
    {
        /// <summary>
        /// Gets a known claimant by claim8 and pin
        /// </summary>
        /// <param name="claim8">The claim8</param>
        /// <param name="pin">The pin</param>
        KnownClaimantDTO GetKnownClaimant(string claim8, string pin);

        /// <summary>
        /// Saves an unknown claim
        /// </summary>
        /// <param name="unknownClaim">The unknown claim dto</param>
        /// <returns>The claim id</returns>
        string SaveUnknownClaim(ClaimantDTO claimantDTO);

        /// <summary>
        /// Updates a known claim
        /// </summary>
        /// <param name="claimantDTO">The known claim dto</param>
        /// <returns>The claim id</returns>
        string UpdateKnownClaim(KnownClaimantDTO knownClaimantDTO);
    }
}
