using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Verification.settings {
    class SettingsController {
        public void createView(ISettingsController c, System.Windows.Forms.Form view) {
            // create or focus help form 
            if (view != null && !view.Disposing && view.Text != "") {
                view.Focus();
            } else {
                view = c.createView();
                view.Show();
            }
        }

        private void openSaveFileDialog(ISettingsController c) {
            var saveDialog = new SaveFileDialog {
                Title = "Сохранение списка ошибок",
                FileName = "Settings.json",
                Filter = "Текстовый документ (*.json)|*.json"
            };
            if (saveDialog.ShowDialog() == DialogResult.OK) {
                // заполнить настройки ...
                //var settings = c.createSettings();
                //fileHandler.writeInFile(saveDialog.FileName, settings);
            }
        }
        private void openOpenFileDialog() {
            var openFileDialog = new OpenFileDialog {
                Title = "Выберите json файл",
                Multiselect = false,
                Filter = "Файл json|*.json"
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK) {
                var fileName = openFileDialog.FileName;
                // дессериализация ...
                // заполнить форму ...
            }
        }
    }
}
