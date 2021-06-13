namespace Verification.settings {
    public interface ISettingsController {
        System.Windows.Forms.Form createView();
        object createSettings();
        void deserializeSettings(string fileName);
        void fillForm();
    }
}