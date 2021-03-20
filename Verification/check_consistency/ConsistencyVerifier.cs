using System.Collections.Generic;
using System.Linq;

namespace Verification.package_ver {
    class ConsistencyVerifier {
        public static void Verify(Diagram uc, Diagram ad, Diagram cd, List<Mistake> mistakes) {
            if (uc!=null && ad != null) {
                var ucAd = uc.Actors.Except(ad.Actors).ToList();
                var adUc = ad.Actors.Except(uc.Actors).ToList();
                ucAd.ForEach(x => mistakes.Add(
                    new Mistake(
                    MistakesTypes.WARNING, $"Несогласованность между ДП и АД - в ДП имеется актор {x}, которого нет в АД", 
                    new BoundingBox(-1, -1, -1, -1))));
                adUc.ForEach(x => mistakes.Add(
                    new Mistake(
                    MistakesTypes.WARNING, $"Несогласованность между ДП и АД - в АД имеется актор {x}, которого нет в ДП",
                    new BoundingBox(-1, -1, -1, -1))));
            }
            if(uc!=null && cd != null) {
                var ucCd = uc.Actors.Except(cd.Actors).ToList();
                var cdUc = cd.Actors.Except(uc.Actors).ToList();
                ucCd.ForEach(x => mistakes.Add(
                    new Mistake(
                    MistakesTypes.WARNING, $"Несогласованность между ДП и ДК - в ДП имеется актор {x}, которого нет в ДК",
                    new BoundingBox(-1, -1, -1, -1))));
                cdUc.ForEach(x => mistakes.Add(
                    new Mistake(
                    MistakesTypes.WARNING, $"Несогласованность между ДП и ДК - в ДК имеется актор {x}, которого нет в ДП",
                    new BoundingBox(-1, -1, -1, -1))));
            }
            if(ad!=null && cd != null) {
                var adCd = ad.Actors.Except(cd.Actors).ToList();
                var cdAd = cd.Actors.Except(ad.Actors).ToList();
                adCd.ForEach(x => mistakes.Add(
                    new Mistake(
                    MistakesTypes.WARNING, $"Несогласованность между АД и ДК - в АД имеется актор {x}, которого нет в ДК",
                    new BoundingBox(-1, -1, -1, -1))));
                cdAd.ForEach(x => mistakes.Add(
                    new Mistake(
                    MistakesTypes.WARNING, $"Несогласованность между ДК и АД - в ДК имеется актор {x}, которого нет в АД",
                    new BoundingBox(-1, -1, -1, -1))));
            }
            
            
         
        }
    }
}
