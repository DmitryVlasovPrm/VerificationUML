using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Verification.help_form {
    class HelpController {
        Helper view;

        public HelpController(Helper view) {
            this.view = view ?? throw new ArgumentNullException(nameof(view));
        }
        public void treeSelectedChanged() {

        }
    }
}
