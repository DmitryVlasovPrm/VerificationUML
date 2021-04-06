using System;

namespace Verification.help_form
{
    internal class HelpController
    {
        private readonly Helper view;

        public HelpController(Helper view)
        {
            this.view = view ?? throw new ArgumentNullException(nameof(view));
        }
        public void treeSelectedChanged()
        {

        }
    }
}
