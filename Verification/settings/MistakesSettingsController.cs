using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Verification.settings {
    public class MistakesSettingsController{
        private MistakesSettingsForm view = null;
        FileSettingHandler<MistakeModel> fileHandler = new FileSettingHandler<MistakeModel>();
        MistakeModel model = new MistakeModel();
        public void createView() {
            // create or focus help form 
            if (view != null && !view.Disposing && view.Text != "") {
                view.Focus();
            } else {
                view = new MistakesSettingsForm(this, model);
                view.Show();
            }
            fillForm();
        }
        private void fillForm() {
            view.fillForm();
        }
        bool validateForm() {
            if (!view.validateValues()) {
                view.ShowMsg("Не все значения являются double", "");
                return false;
            };
            return true;
        }

        internal void onOk() {
            if (!validateForm())
                return;
            view.updateModel();
            view.Dispose();
        }

        private string openSaveFileDialog() {
            var saveDialog = new SaveFileDialog {
                Title = "Экспорт настроек",
                FileName = "Settings.json",
                Filter = "Текстовый документ (*.json)|*.json"
            };
            if (saveDialog.ShowDialog() == DialogResult.OK)
                return saveDialog.FileName;
            return "";
        }

        private string openOpenFileDialog() {
            var openFileDialog = new OpenFileDialog {
                Title = "Выберите json файл",
                Multiselect = false,
                Filter = "Файл json|*.json"
            };
            if (openFileDialog.ShowDialog() == DialogResult.OK)
                return openFileDialog.FileName;
            return "";
        }

        private bool deserializeSettings(string fileName) {
            Func<object, Boolean> checker = (o) => {
                return true;
            };
            try {
                model = fileHandler.readFromFile(fileName, checker);
            } catch (Exception e) {
                view.ShowMsg(e.Message, "Exception!");
                return false;
            }
            return true;
        }
        internal void import() {
            var filename = openOpenFileDialog();
            if (filename == "") return;
            if(deserializeSettings(filename))
                view.fillForm();
        }

        internal void export() {
            if (validateForm()) {
                view.updateModel();
                var filename = openSaveFileDialog();
                if (filename!="")
                    fileHandler.writeInFile(filename, model);
            }
        }
        
    }
}
