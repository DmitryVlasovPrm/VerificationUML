using System.Collections.Generic;
using System.Linq;
using Verification.rating_system;

namespace Verification.package_ver
{
    internal class ConsistencyVerifier
    {
        public static void Verify(Diagram uc, Diagram ad, Diagram cd, List<Mistake> mistakes)
        {
            var ucNames = new HashSet<string>();
            var adNames = new HashSet<string>();
            var cdNames = new HashSet<string>();

            if (uc != null)
                ucNames = uc.Actors.Select(x => x.Name).ToHashSet();
            if (ad != null)
                adNames = ad.Actors.Select(x => x.Name).ToHashSet();
            if (cd != null)
                cdNames = cd.Actors.Select(x => x.Name).ToHashSet();
            if (uc != null && ad != null)
            {
                var ucAd = ucNames.Except(adNames).ToList();
                var adUc = adNames.Except(ucNames).ToList();
                ucAd.ForEach(x => mistakes.Add(
                    new Mistake(
                    MistakesTypes.WARNING, $"Несогласованность между ДП и АД - в ДП имеется актор {x}, которого нет в АД",
                    new BoundingBox(-1, -1, -1, -1), ALL_MISTAKES.ALLUCDAD)));
                adUc.ForEach(x => mistakes.Add(
                    new Mistake(
                    MistakesTypes.WARNING, $"Несогласованность между ДП и АД - в АД имеется актор {x}, которого нет в ДП",
                    new BoundingBox(-1, -1, -1, -1), ALL_MISTAKES.ALLUCDAD)));
            }
            if (uc != null && cd != null)
            {
                var ucCd = ucNames.Except(cdNames).ToList();
                var cdUc = cdNames.Except(ucNames).ToList();
                ucCd.ForEach(x => mistakes.Add(
                    new Mistake(
                    MistakesTypes.WARNING, $"Несогласованность между ДП и ДК - в ДП имеется актор {x}, которого нет в ДК",
                    new BoundingBox(-1, -1, -1, -1), ALL_MISTAKES.ALLUCDAD)));
                cdUc.ForEach(x => mistakes.Add(
                    new Mistake(
                    MistakesTypes.WARNING, $"Несогласованность между ДП и ДК - в ДК имеется актор {x}, которого нет в ДП",
                    new BoundingBox(-1, -1, -1, -1), ALL_MISTAKES.ALLUCDAD)));
            }
            if (ad != null && cd != null)
            {
                var adCd = adNames.Except(cdNames).ToList();
                var cdAd = cdNames.Except(adNames).ToList();
                adCd.ForEach(x => mistakes.Add(
                    new Mistake(
                    MistakesTypes.WARNING, $"Несогласованность между АД и ДК - в АД имеется актор {x}, которого нет в ДК",
                    new BoundingBox(-1, -1, -1, -1), ALL_MISTAKES.ALLUCDAD)));
                cdAd.ForEach(x => mistakes.Add(
                    new Mistake(
                    MistakesTypes.WARNING, $"Несогласованность между ДК и АД - в ДК имеется актор {x}, которого нет в АД",
                    new BoundingBox(-1, -1, -1, -1), ALL_MISTAKES.ALLUCDAD)));
            }



        }
    }
}
